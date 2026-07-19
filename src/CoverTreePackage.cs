using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using CoverTree.VS.Coverage;
using CoverTree.VS.Options;
using Task = System.Threading.Tasks.Task;

namespace CoverTree.VS
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ToolWindow.CoverTreeToolWindow), Style = VsDockStyle.Tabbed,
        Window = "3ae79031-e1bc-11d0-8f78-00a0c9110057")]
    [ProvideOptionPage(typeof(CoverTreeOptionsPage), "CoverTree", "General", 0, 0, true)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    // Coverage projects (Jest/Vitest/NYC) are typically opened via File > Open Folder rather
    // than a .sln, which does not raise SolutionExists — load on that context too.
    [ProvideAutoLoad(VSConstants.UICONTEXT.FolderOpened_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class CoverTreePackage : AsyncPackage
    {
        public const string PackageGuidString = "9F3A7B2C-D4E5-4F1B-A8C6-3E9F2B4D7A1C";

        private static CoverTreePackage _instance;
        public static CoverTreePackage Instance => _instance;

        private CoverageService _coverageService;
        public CoverageService CoverageService => _coverageService;

        public CoverTreeOptionsPage Options =>
            GetDialogPage(typeof(CoverTreeOptionsPage)) as CoverTreeOptionsPage;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            _instance = this;
            await base.InitializeAsync(cancellationToken, progress);
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await Commands.RefreshCoverageCommand.InitializeAsync(this);
            await Commands.NavigateUncoveredCommand.InitializeAsync(this);
            await Commands.ShowCoverageCommand.InitializeAsync(this);
            await Commands.ShowToolWindowCommand.InitializeAsync(this);

            var solution = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
            if (solution != null)
            {
                string solutionDir, solutionFile, userFile;
                solution.GetSolutionInfo(out solutionDir, out solutionFile, out userFile);
                if (!string.IsNullOrEmpty(solutionDir))
                    InitCoverageService(solutionDir);
            }

            await ShowCoverTreeToolWindowAsync();
        }

        public async Task ShowCoverTreeToolWindowAsync() =>
            await ShowToolWindowAsync(typeof(ToolWindow.CoverTreeToolWindow), 0, true, DisposalToken);

        public void InitCoverageService(string projectPath)
        {
            _coverageService?.Dispose();
            _coverageService = new CoverageService(projectPath);
            _coverageService.DataChanged += OnCoverageChanged;
        }

        private void OnCoverageChanged(object sender, CoverageDataChangedEventArgs e)
        {
            JoinableTaskFactory.RunAsync(async () =>
            {
                await JoinableTaskFactory.SwitchToMainThreadAsync();

                var win = FindToolWindow(typeof(ToolWindow.CoverTreeToolWindow), 0, false) as ToolWindow.CoverTreeToolWindow;
                win?.Refresh();

                // Push updated coverage items into open Solution Explorer sources.
                SolutionExplorer.CoverageCollectionSourceProvider.Current?.NotifyChanged();

                UpdateStatusBar();
            }).FileAndForget(nameof(OnCoverageChanged));
        }

        private void UpdateStatusBar()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (GetService(typeof(SVsStatusbar)) is not IVsStatusbar statusbar) return;

            statusbar.IsFrozen(out int frozen);
            if (frozen != 0) return;

            var all = _coverageService?.GetAllCoverage();
            if (all == null)
            {
                statusbar.SetText("CoverTree: No data");
                return;
            }

            var files = all.Where(kv => kv.Key != "total").ToList();
            if (files.Count == 0)
            {
                statusbar.SetText("CoverTree: No files");
                return;
            }

            var avg = files.Average(kv => CoverageParser.GetOverallPct(kv.Value));
            statusbar.SetText($"CoverTree: {avg:F1}% avg ({files.Count} files)");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _coverageService?.Dispose();
            base.Dispose(disposing);
        }
    }
}
