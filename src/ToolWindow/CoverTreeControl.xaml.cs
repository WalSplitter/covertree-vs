using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoverTree.VS.ToolWindow
{
    public partial class CoverTreeControl : UserControl
    {
        private readonly CoverageViewModel _vm;

        internal event EventHandler RefreshRequested;
        internal event EventHandler<CoverageFileItem> FileDoubleClicked;

        public CoverTreeControl()
        {
            InitializeComponent();
            _vm = new CoverageViewModel();
            DataContext = _vm;
        }

        internal CoverageViewModel ViewModel => _vm;

        private void OnRefreshClick(object sender, RoutedEventArgs e) =>
            RefreshRequested?.Invoke(this, e);

        private void OnTreeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as TreeView)?.SelectedItem as CoverageFileItem;
            if (item?.IsFolder == false)
                FileDoubleClicked?.Invoke(this, item);
        }
    }
}
