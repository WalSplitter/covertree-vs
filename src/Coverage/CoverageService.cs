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
        private readonly string _projectPath;
        private FileSystemWatcher _summaryWatcher;
        private FileSystemWatcher _detailWatcher;
        private Dictionary<string, FileCoverage> _summary;
        private Dictionary<string, JObject> _detail;

        public event EventHandler<CoverageDataChangedEventArgs> DataChanged;

        private CoverTreeSettings Settings => CoverTreePackage.Instance?.Options?.ToSettings() ?? new CoverTreeSettings();

        public CoverageService(string projectPath)
        {
            _projectPath = projectPath;
            Refresh();
            SetupWatchers();
        }

        private void SetupWatchers()
        {
            _summaryWatcher = Watch(GetSummaryPath(), OnFileChanged);
            _detailWatcher = Watch(GetDetailPath(), OnFileChanged);
        }

        private FileSystemWatcher Watch(string path, FileSystemEventHandler handler)
        {
            var dir = Path.GetDirectoryName(path);
            var file = Path.GetFileName(path);
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return null;

            var w = new FileSystemWatcher(dir, file)
            {
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
            _summary = CoverageParser.Parse(GetSummaryPath());
            _detail = DetailParser.Parse(GetDetailPath());
            DataChanged?.Invoke(this, new CoverageDataChangedEventArgs());
        }

        public Dictionary<string, FileCoverage> GetAllCoverage() => _summary;

        public FileCoverage GetFileCoverage(string path) =>
            CoverageParser.GetFileCoverage(_summary, path);

        public Dictionary<int, LineCoverageStatus> GetLineCoverage(string path)
        {
            var fc = DetailParser.GetFileCoverage(_detail, path);
            return DetailParser.GetLineCoverageMap(fc);
        }

        private string GetSummaryPath() =>
            Path.Combine(_projectPath, Settings.CoverageFile.Replace('/', Path.DirectorySeparatorChar));

        private string GetDetailPath() =>
            Path.Combine(_projectPath, Settings.DetailFile.Replace('/', Path.DirectorySeparatorChar));

        public void Dispose()
        {
            _summaryWatcher?.Dispose();
            _detailWatcher?.Dispose();
        }
    }
}
