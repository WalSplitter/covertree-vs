using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

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
            Content = _control;
        }

        public void Refresh()
        {
            _control?.Refresh();
        }
    }
}
