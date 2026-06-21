using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.Shell;
using CoverTree.VS.Coverage;

namespace CoverTree.VS.ToolWindow
{
    public partial class CoverTreeControl : UserControl
    {
        private readonly CoverageViewModel _vm;

        public CoverTreeControl()
        {
            InitializeComponent();
            _vm = new CoverageViewModel();
            DataContext = _vm;

            var svc = CoverTreePackage.Instance?.CoverageService;
            if (svc != null)
            {
                svc.DataChanged += (s, e) => Dispatcher.Invoke(RefreshData);
                RefreshData();
            }
        }

        public void Refresh()
        {
            Dispatcher.Invoke(RefreshData);
        }

        private void RefreshData()
        {
            var pkg = CoverTreePackage.Instance;
            var svc = pkg?.CoverageService;
            if (svc == null) return;

            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            var solutionDir = string.Empty;
            try { solutionDir = Path.GetDirectoryName(dte?.Solution?.FullName ?? "") ?? ""; }
            catch { }

            _vm.Update(svc.GetAllCoverage(), solutionDir, pkg.Options?.Threshold ?? 75);
        }

        private void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            CoverTreePackage.Instance?.CoverageService?.Refresh();
        }

        private void OnTreeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as TreeView)?.SelectedItem as CoverageFileItem;
            if (item == null || item.IsFolder) return;

            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            dte?.ItemOperations?.OpenFile(item.FullPath);
        }
    }
}
