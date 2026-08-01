using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using CoverTree.VS.Coverage;

namespace CoverTree.VS.ToolWindow
{
    [Guid("7B1E4A8C-2D5F-4C9A-6E3B-8F4A1D7C2E5B")]
    public class CoverTreeToolWindow : ToolWindowPane
    {
        private CoverTreeControl _control;

        public CoverTreeToolWindow() : base(null)
        {
            Caption = "CoverTree";
            _control = new CoverTreeControl();
            _control.RefreshRequested += (s, e) =>
                CoverTreePackage.Instance?.CoverageService?.Refresh();
            _control.FileDoubleClicked += OnFileDoubleClicked;
            Content = _control;
        }

        // The constructor can run on a background thread when VS recreates a
        // persisted tool window during startup layout restore, before this
        // package (and CoverTreePackage.Instance) even exists. OnToolWindowCreated
        // is called later but is guaranteed by VS to run on the UI thread, so all
        // UI-thread-affine wiring happens here instead of in the constructor.
        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();
            ThreadHelper.ThrowIfNotOnUIThread();

            var svc = CoverTreePackage.Instance?.CoverageService;
            if (svc != null)
            {
                svc.DataChanged += (s, e) => Refresh();
                RefreshData();
            }
        }

        public void Refresh()
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                RefreshData();
            }).FileAndForget(nameof(CoverTreeToolWindow));
        }

        private void RefreshData()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var pkg = CoverTreePackage.Instance;
            if (pkg?.CoverageService == null) return;

            // pkg.ProjectPath is the root CoverageService actually resolved the coverage
            // files against (via IVsSolution.GetSolutionInfo, which also covers Open
            // Folder mode). dte.Solution.FullName is empty for folder-opened projects,
            // so falling back to it here would leave paths unstripped and show every
            // filesystem folder level up to the drive root in the tree.
            var projectRoot = pkg.ProjectPath ?? string.Empty;

            _control.ViewModel.Update(pkg.CoverageService.GetAllCoverage(), projectRoot, pkg.Options?.Threshold ?? 75);
        }

        private void OnFileDoubleClicked(object sender, CoverageFileItem item)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            dte?.ItemOperations?.OpenFile(item.FullPath);
        }
    }
}
