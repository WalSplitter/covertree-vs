using System;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using CoverTree.VS.Coverage;
using Task = System.Threading.Tasks.Task;

namespace CoverTree.VS.Commands
{
    internal sealed class NavigateUncoveredCommand
    {
        public const int NextCommandId = 0x0101;
        public const int PrevCommandId = 0x0102;
        public static readonly Guid CommandSet = new Guid("4C8E1F3A-7D2B-4F6A-9B3E-5D1C8F2A4E7B");

        private NavigateUncoveredCommand(OleMenuCommandService svc)
        {
            svc.AddCommand(new MenuCommand((s, e) => Navigate(true),  new CommandID(CommandSet, NextCommandId)));
            svc.AddCommand(new MenuCommand((s, e) => Navigate(false), new CommandID(CommandSet, PrevCommandId)));
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var svc = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (svc != null) new NavigateUncoveredCommand(svc);
        }

        private void Navigate(bool forward)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var svc = CoverTreePackage.Instance?.CoverageService;
            if (svc == null) return;

            var dte = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            var doc = dte?.ActiveDocument;
            if (doc == null) return;

            var lineMap = svc.GetLineCoverage(doc.FullName);
            var uncovered = lineMap
                .Where(kv => kv.Value == LineCoverageStatus.Uncovered)
                .Select(kv => kv.Key)
                .OrderBy(l => l)
                .ToList();

            if (uncovered.Count == 0) return;

            var sel = doc.Selection as EnvDTE.TextSelection;
            if (sel == null) return;

            int current = sel.CurrentLine;
            int target;

            if (forward)
            {
                target = uncovered.FirstOrDefault(l => l > current);
                if (target == 0) target = uncovered.First();
            }
            else
            {
                target = uncovered.LastOrDefault(l => l < current);
                if (target == 0) target = uncovered.Last();
            }

            sel.GotoLine(target, true);
        }
    }
}
