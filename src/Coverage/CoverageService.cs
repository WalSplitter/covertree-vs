using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using CoverTree.VS.Options;

namespace CoverTree.VS.Coverage
{
    public class CoverageDataChangedEventArgs : EventArgs { }

    public class CoverageService : IDisposable
    {
        // Coverage files typically sit under a sub-project's own folder (e.g.
        // "ClientApp/coverage/coverage-summary.json"), not the solution root, and
        // that nesting varies per project - so instead of a fixed configured path,
        // the whole tree is scanned for files with the configured name. These
        // directories are skipped because they can be huge and never contain
        // coverage output themselves.
        private static readonly string[] IgnoredDirNames =
            { "node_modules", ".git", ".vs", "bin", "obj", "packages", "dist", "out" };

        private readonly string _projectPath;
        private FileSystemWatcher? _summaryWatcher;
        private FileSystemWatcher? _detailWatcher;
        private Dictionary<string, FileCoverage>? _summary;
        private Dictionary<string, JObject>? _detail;

        public event EventHandler<CoverageDataChangedEventArgs>? DataChanged;

        private CoverTreeSettings Settings => CoverTreePackage.Instance?.Options?.ToSettings() ?? new CoverTreeSettings();

        public CoverageService(string projectPath)
        {
            _projectPath = projectPath;
            Refresh();
            SetupWatchers();
        }

        private void SetupWatchers()
        {
            _summaryWatcher = WatchRecursive(SummaryFileName, OnFileChanged);
            _detailWatcher = WatchRecursive(DetailFileName, OnFileChanged);
        }

        private FileSystemWatcher? WatchRecursive(string fileName, FileSystemEventHandler handler)
        {
            if (string.IsNullOrEmpty(fileName) || !Directory.Exists(_projectPath)) return null;

            var w = new FileSystemWatcher(_projectPath, fileName)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };
            w.Changed += handler;
            w.Created += handler;
            w.Deleted += handler;
            return w;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(150);
            Refresh();
        }

        public void Refresh()
        {
            _summary = MergeSummaries(FindCoverageFiles(SummaryFileName));
            _detail = MergeDetails(FindCoverageFiles(DetailFileName));
            DataChanged?.Invoke(this, new CoverageDataChangedEventArgs());
        }

        public Dictionary<string, FileCoverage>? GetAllCoverage() => _summary;

        public FileCoverage? GetFileCoverage(string path) =>
            CoverageParser.GetFileCoverage(_summary, path);

        public Dictionary<int, LineCoverageStatus> GetLineCoverage(string path)
        {
            var fc = DetailParser.GetFileCoverage(_detail, path);
            return DetailParser.GetLineCoverageMap(fc);
        }

        private string SummaryFileName => Path.GetFileName(Settings.CoverageFile);
        private string DetailFileName => Path.GetFileName(Settings.DetailFile);

        private List<string> FindCoverageFiles(string fileName)
        {
            var results = new List<string>();
            if (string.IsNullOrEmpty(fileName) || !Directory.Exists(_projectPath)) return results;

            var stack = new Stack<string>();
            stack.Push(_projectPath);

            while (stack.Count > 0)
            {
                var dir = stack.Pop();

                try { results.AddRange(Directory.GetFiles(dir, fileName)); }
                catch { continue; }

                string[] subDirs;
                try { subDirs = Directory.GetDirectories(dir); }
                catch { continue; }

                foreach (var sub in subDirs)
                {
                    var name = Path.GetFileName(sub);
                    var ignored = false;
                    foreach (var ignoredName in IgnoredDirNames)
                    {
                        if (string.Equals(name, ignoredName, StringComparison.OrdinalIgnoreCase)) { ignored = true; break; }
                    }
                    if (!ignored) stack.Push(sub);
                }
            }

            return results;
        }

        private Dictionary<string, FileCoverage>? MergeSummaries(List<string> paths)
        {
            if (paths.Count == 0) return null;

            var merged = new Dictionary<string, FileCoverage>(StringComparer.OrdinalIgnoreCase);
            foreach (var path in paths)
            {
                var parsed = CoverageParser.Parse(path);
                if (parsed == null) continue;

                foreach (var kv in parsed)
                {
                    if (kv.Key == "total") continue;
                    merged[kv.Key] = kv.Value;
                }
            }

            return merged.Count > 0 ? merged : null;
        }

        private Dictionary<string, JObject>? MergeDetails(List<string> paths)
        {
            if (paths.Count == 0) return null;

            var merged = new Dictionary<string, JObject>(StringComparer.OrdinalIgnoreCase);
            foreach (var path in paths)
            {
                var parsed = DetailParser.Parse(path);
                if (parsed == null) continue;

                foreach (var kv in parsed)
                    merged[kv.Key] = kv.Value;
            }

            return merged.Count > 0 ? merged : null;
        }

        public void Dispose()
        {
            _summaryWatcher?.Dispose();
            _detailWatcher?.Dispose();
        }
    }
}
