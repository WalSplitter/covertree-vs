using System;
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

            var solution = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
            if (solution != null)
            {
                string solutionDir, solutionFile, userFile;
                solution.GetSolutionInfo(out solutionDir, out solutionFile, out userFile);
                if (!string.IsNullOrEmpty(solutionDir))
                    InitCoverageService(solutionDir);
            }

            await ShowToolWindowAsync(typeof(ToolWindow.CoverTreeToolWindow), 0, true, cancellationToken);
        }

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
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _coverageService?.Dispose();
            base.Dispose(disposing);
        }
    }
}
