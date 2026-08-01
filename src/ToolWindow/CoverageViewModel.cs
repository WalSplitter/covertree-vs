using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CoverTree.VS.Coverage;

namespace CoverTree.VS.ToolWindow
{
    public class CoverageViewModel : INotifyPropertyChanged
    {
        private string _statusText = "No coverage data";
        private double _overallPct;

        public ObservableCollection<CoverageFileItem> Items { get; } = new ObservableCollection<CoverageFileItem>();

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        public string OverallPctDisplay => $"Overall: {_overallPct:F1}%";

        public void Update(Dictionary<string, FileCoverage>? summary, string projectRoot, double threshold)
        {
            Items.Clear();

            if (summary == null)
            {
                StatusText = "No coverage data found. Run Jest/Vitest first.";
                _overallPct = 0;
                OnPropertyChanged(nameof(OverallPctDisplay));
                return;
            }

            var files = summary.Where(kv => kv.Key != "total").ToList();

            foreach (var kv in files)
            {
                var pct = CoverageParser.GetOverallPct(kv.Value);
                var status = pct >= threshold ? CoverageStatus.Passing : CoverageStatus.Warning;
                var rel = GetRelativePath(kv.Key, projectRoot);
                var parts = rel.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                AddToTree(Items, parts, 0, kv.Key, kv.Value, pct, status);
            }

            var allPcts = files.Select(kv => CoverageParser.GetOverallPct(kv.Value)).ToList();
            _overallPct = allPcts.Count > 0 ? allPcts.Average() : 0;
            StatusText = $"{files.Count} file{(files.Count == 1 ? "" : "s")}";
            OnPropertyChanged(nameof(OverallPctDisplay));
        }

        private void AddToTree(ObservableCollection<CoverageFileItem> col, string[] parts, int idx,
            string fullPath, FileCoverage coverage, double pct, CoverageStatus status)
        {
            if (idx >= parts.Length) return;

            if (idx == parts.Length - 1)
            {
                col.Add(new CoverageFileItem
                {
                    Name = parts[idx],
                    FullPath = fullPath,
                    IsFolder = false,
                    Pct = pct,
                    Status = status,
                    CoverageData = coverage
                });
                return;
            }

            var folder = null as CoverageFileItem;
            foreach (var item in col)
            {
                if (item.IsFolder && item.Name == parts[idx]) { folder = item; break; }
            }

            if (folder == null)
            {
                folder = new CoverageFileItem { Name = parts[idx], IsFolder = true };
                col.Add(folder);
            }

            AddToTree(folder.Children, parts, idx + 1, fullPath, coverage, pct, status);
            UpdateFolderPct(folder);
        }

        private void UpdateFolderPct(CoverageFileItem folder)
        {
            var pcts = CollectFilePcts(folder);
            if (pcts.Count > 0) folder.Pct = pcts.Average();
        }

        private List<double> CollectFilePcts(CoverageFileItem item)
        {
            var result = new List<double>();
            foreach (var child in item.Children)
            {
                if (child.IsFolder) result.AddRange(CollectFilePcts(child));
                else result.Add(child.Pct);
            }
            return result;
        }

        private string GetRelativePath(string path, string basePath)
        {
            path = path.Replace('\\', '/');
            basePath = (basePath ?? "").Replace('\\', '/').TrimEnd('/');

            // Cut only the levels ABOVE the workspace root folder, keeping the root
            // folder itself as the returned path's first segment - Solution Explorer
            // always shows the opened folder as its own top-level node, and the tree
            // here should match that 1:1 instead of starting one level lower at "src".
            var slashIdx = basePath.LastIndexOf('/');
            var parentPath = slashIdx >= 0 ? basePath.Substring(0, slashIdx) : "";
            if (!string.IsNullOrEmpty(parentPath) && path.StartsWith(parentPath + "/", StringComparison.OrdinalIgnoreCase))
                return path.Substring(parentPath.Length).TrimStart('/');

            // Coverage JSON can carry absolute paths recorded on a different machine
            // or location than where the project currently sits (moved/re-cloned repo,
            // OneDrive-synced path, CI runner, ...), so the prefix above never matches.
            // Anchor on the workspace root folder's own name instead - the same name
            // Solution Explorer shows as the top node - and keep it plus everything
            // after its rightmost occurrence in the recorded path.
            var rootName = basePath.Substring(slashIdx + 1);
            if (!string.IsNullOrEmpty(rootName))
            {
                var parts = path.Split('/');
                for (int i = parts.Length - 2; i >= 0; i--)
                {
                    if (string.Equals(parts[i], rootName, StringComparison.OrdinalIgnoreCase))
                        return string.Join("/", parts, i, parts.Length - i);
                }
            }

            return path;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
