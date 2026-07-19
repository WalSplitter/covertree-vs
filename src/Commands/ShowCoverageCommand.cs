using System;
using System.ComponentModel.Design;
using System.IO;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using CoverTree.VS.Coverage;
using CoverTree.VS.Options;
using Task = System.Threading.Tasks.Task;

namespace CoverTree.VS.Commands
{
    internal sealed class ShowCoverageCommand
    {
        public const int CommandId = 0x0103;
        public static readonly Guid CommandSet = new Guid("4C8E1F3A-7D2B-4F6A-9B3E-5D1C8F2A4E7B");

        private readonly AsyncPackage _package;

        private ShowCoverageCommand(AsyncPackage package, OleMenuCommandService svc)
        {
            _package = package;
            var cmd  = new OleMenuCommand(Execute, new CommandID(CommandSet, CommandId));
            cmd.BeforeQueryStatus += OnBeforeQueryStatus;
            svc.AddCommand(cmd);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var svc = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (svc != null) new ShowCoverageCommand(package, svc);
        }

        // Hide the command when the selected item has no coverage data.
        private void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            if (sender is OleMenuCommand cmd)
                cmd.Visible = GetSelectedFilePath() != null;
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var path = GetSelectedFilePath();
            if (path == null) return;

            var pkg      = CoverTreePackage.Instance;
            var coverage = pkg?.CoverageService?.GetFileCoverage(path);
            var settings = pkg?.Options?.ToSettings() ?? new CoverTreeSettings();
            var fileName = Path.GetFileName(path);

            if (coverage == null)
            {
                ShowInStatusBar($"CoverTree: No coverage data for {fileName}");
                return;
            }

            var pct    = CoverageParser.GetOverallPct(coverage);
            var status = CoverageParser.GetStatus(pct, settings);
            var icon   = status == CoverageStatus.Passing ? "✓" : "⚠";
            ShowInStatusBar(
                $"CoverTree {icon}  {fileName}: {pct:F1}%  " +
                $"Lines {coverage.Lines?.Pct}%  " +
                $"Fn {coverage.Functions?.Pct}%  " +
                $"Br {coverage.Branches?.Pct}%");
        }

        private static void ShowInStatusBar(string text)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var statusbar = ServiceProvider.GlobalProvider.GetService(typeof(SVsStatusbar)) as IVsStatusbar;
            if (statusbar == null) return;
            statusbar.IsFrozen(out int frozen);
            if (frozen == 0) statusbar.SetText(text);
        }

        // Returns the absolute path of the single file selected in Solution Explorer,
        // or null when nothing suitable is selected.
        private string GetSelectedFilePath()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var pkg = CoverTreePackage.Instance;
            if (pkg == null) return null;

            var monSvc = ServiceProvider.GlobalProvider.GetService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
            if (monSvc == null) return null;

            monSvc.GetCurrentSelection(
                out var hierPtr, out uint itemId,
                out var multiSel, out var containerPtr);

            try
            {
                if (hierPtr == IntPtr.Zero) return null;
                var hier = System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(hierPtr) as IVsHierarchy;
                if (hier == null) return null;

                hier.GetCanonicalName(itemId, out var path);
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;

                var ext = Path.GetExtension(path);
                return SolutionExplorer.CoverageCollectionSourceProvider.IsSourceFile(ext) ? path : null;
            }
            finally
            {
                if (hierPtr    != IntPtr.Zero) System.Runtime.InteropServices.Marshal.Release(hierPtr);
                if (containerPtr != IntPtr.Zero) System.Runtime.InteropServices.Marshal.Release(containerPtr);
            }
        }
    }
}
