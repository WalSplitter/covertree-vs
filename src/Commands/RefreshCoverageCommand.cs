using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CoverTree.VS.Commands
{
    internal sealed class RefreshCoverageCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("4C8E1F3A-7D2B-4F6A-9B3E-5D1C8F2A4E7B");

        private RefreshCoverageCommand(OleMenuCommandService svc)
        {
            svc.AddCommand(new MenuCommand(Execute, new CommandID(CommandSet, CommandId)));
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var svc = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (svc != null) new RefreshCoverageCommand(svc);
        }

        private void Execute(object sender, EventArgs e)
        {
            CoverTreePackage.Instance?.CoverageService?.Refresh();
        }
    }
}
