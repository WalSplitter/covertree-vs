using System;
using System.IO;
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

            var svc = CoverTreePackage.Instance?.CoverageService;
            if (svc != null)
            {
                svc.DataChanged += (s, e) => _control.Dispatcher.InvokeAsync(RefreshData);
                RefreshData();
            }
        }

        public void Refresh() => _control?.Dispatcher.InvokeAsync(RefreshData);

        private void RefreshData()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var pkg = CoverTreePackage.Instance;
            if (pkg?.CoverageService == null) return;

            var dte = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            var solutionDir = string.Empty;
            try { solutionDir = Path.GetDirectoryName(dte?.Solution?.FullName ?? "") ?? ""; }
            catch { }

            _control.ViewModel.Update(pkg.CoverageService.GetAllCoverage(), solutionDir, pkg.Options?.Threshold ?? 75);
        }

        private void OnFileDoubleClicked(object sender, CoverageFileItem item)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            dte?.ItemOperations?.OpenFile(item.FullPath);
        }
    }
}
