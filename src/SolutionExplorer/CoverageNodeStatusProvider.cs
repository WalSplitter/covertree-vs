using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using CoverTree.VS.Coverage;
using CoverTree.VS.Options;

// IAttachedCollectionSourceProvider is the public VS SDK mechanism for adding
// expandable child rows to Solution Explorer nodes (same API as Git's "Incoming
// Changes" sub-items).  Each watched source file gets one collapsible child row:
//   🟢 CoverTree: 87.3%  Lines 90%  Fn 82%  Br 85%
// When coverage data refreshes the ObservableCollection is updated in-place so
// VS re-renders without requiring a manual collapse/expand.
namespace CoverTree.VS.SolutionExplorer
{
    [Export(typeof(IAttachedCollectionSourceProvider))]
    [Name("CoverTree.CoverageProvider")]
    [Order(Before = "Default")]
    internal sealed class CoverageCollectionSourceProvider : IAttachedCollectionSourceProvider
    {
        private const string RelationshipName = "CoverTree.Coverage";

        internal static CoverageCollectionSourceProvider Current { get; private set; }

        // All live sources keyed by file path so we can push updates.
        private readonly Dictionary<string, CoverageCollectionSource> _sources =
            new Dictionary<string, CoverageCollectionSource>(StringComparer.OrdinalIgnoreCase);

        public CoverageCollectionSourceProvider()
        {
            Current = this;
        }

        // Called by CoverTreePackage when coverage JSON is reloaded.
        internal void NotifyChanged()
        {
            lock (_sources)
            {
                foreach (var src in _sources.Values)
                    src.Refresh();
            }
        }

        // ── IAttachedCollectionSourceProvider ──────────────────────────────

        public IEnumerable<IAttachedRelationship> GetRelationships(object item)
        {
            var path = CanonicalName(item);
            if (!string.IsNullOrEmpty(path) && IsWatchedFile(path))
                yield return CoverageRelationship.Instance;
        }

        public IAttachedCollectionSource CreateCollectionSource(
            object item, string relationshipName)
        {
            if (relationshipName != RelationshipName) return null;

            var path = CanonicalName(item);
            if (string.IsNullOrEmpty(path) || !IsWatchedFile(path)) return null;

            var src = new CoverageCollectionSource(item, path);
            lock (_sources) { _sources[path] = src; }
            return src;
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private static string CanonicalName(object item)
        {
            // In VS Solution Explorer the item is an IVsHierarchyItem.
            if (item is IVsHierarchyItem hi)
                return hi.HierarchyIdentity?.CanonicalName;
            return null;
        }

        private static readonly string[] WatchedExts =
            { ".ts", ".tsx", ".js", ".jsx", ".mjs", ".cjs" };

        // Also called by ShowCoverageCommand to decide whether to show the menu item.
        internal static bool IsSourceFile(string ext) =>
            WatchedExts.Any(e => e.Equals(ext, StringComparison.OrdinalIgnoreCase));

        private static bool IsWatchedFile(string path)
        {
            if (!File.Exists(path)) return false;
            if (!IsSourceFile(Path.GetExtension(path))) return false;
            var stem = Path.GetFileNameWithoutExtension(path);
            return !stem.EndsWith(".test",   StringComparison.OrdinalIgnoreCase)
                && !stem.EndsWith(".spec",   StringComparison.OrdinalIgnoreCase)
                && !stem.EndsWith(".config", StringComparison.OrdinalIgnoreCase)
                && !stem.EndsWith(".rc",     StringComparison.OrdinalIgnoreCase);
        }

        // Singleton relationship descriptor.
        private sealed class CoverageRelationship : IAttachedRelationship
        {
            public static readonly CoverageRelationship Instance = new CoverageRelationship();
            public string Name        => RelationshipName;
            public string DisplayName => "Coverage";
        }
    }

    // ── Collection source ────────────────────────────────────────────────────

    internal sealed class CoverageCollectionSource : IAttachedCollectionSource
    {
        private readonly string _path;
        private readonly ObservableCollection<CoverageStatusItem> _items =
            new ObservableCollection<CoverageStatusItem>();

        public object SourceItem { get; }

        public CoverageCollectionSource(object sourceItem, string path)
        {
            SourceItem = sourceItem;
            _path      = path;
            Refresh();
        }

        public bool HasItems => _items.Count > 0;

        public IEnumerable Items => _items;

        internal void Refresh()
        {
            var pkg = CoverTreePackage.Instance;
            if (pkg?.CoverageService == null)
            {
                _items.Clear();
                return;
            }

            var coverage = pkg.CoverageService.GetFileCoverage(_path);
            if (coverage == null)
            {
                _items.Clear();
                return;
            }

            var settings = pkg.Options?.ToSettings() ?? new CoverTreeSettings();
            var pct      = CoverageParser.GetOverallPct(coverage);
            var status   = CoverageParser.GetStatus(pct, settings);
            var item     = new CoverageStatusItem(coverage, pct, status);

            if (_items.Count == 0)
                _items.Add(item);
            else
                _items[0] = item;
        }
    }

    // ── Item rendered as a child row in Solution Explorer ────────────────────

    internal sealed class CoverageStatusItem
    {
        private readonly FileCoverage   _coverage;
        private readonly double         _pct;
        private readonly CoverageStatus _status;

        internal CoverageStatusItem(FileCoverage coverage, double pct, CoverageStatus status)
        {
            _coverage = coverage;
            _pct      = pct;
            _status   = status;
        }

        // WPF DataTemplate (registered via Application.Current.Resources in the
        // package) binds to these properties.  Without a registered template VS
        // falls back to ToString().
        public ImageMoniker Icon => _status switch
        {
            CoverageStatus.Passing => KnownMonikers.StatusOK,
            CoverageStatus.Warning => KnownMonikers.StatusWarning,
            _                      => KnownMonikers.StatusHelp,
        };

        public string Text =>
            $"CoverTree: {_pct:F1}%  " +
            $"Lines {_coverage.Lines?.Pct}%  " +
            $"Fn {_coverage.Functions?.Pct}%  " +
            $"Br {_coverage.Branches?.Pct}%";

        public override string ToString() => Text;
    }
}
