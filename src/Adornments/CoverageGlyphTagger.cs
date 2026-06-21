using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using CoverTree.VS.Coverage;

namespace CoverTree.VS.Adornments
{
    internal class CoverageGlyphTagger : ITagger<CoverageTag>, IDisposable
    {
        private readonly ITextBuffer _buffer;
        private readonly string _filePath;
        private Dictionary<int, LineCoverageStatus> _lineCoverage = new Dictionary<int, LineCoverageStatus>();

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public CoverageGlyphTagger(ITextBuffer buffer)
        {
            _buffer = buffer;

            if (buffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument doc))
                _filePath = doc.FilePath;

            var svc = CoverTreePackage.Instance?.CoverageService;
            if (svc != null)
            {
                svc.DataChanged += OnDataChanged;
                LoadCoverage();
            }
        }

        private void OnDataChanged(object sender, CoverageDataChangedEventArgs e)
        {
            LoadCoverage();
            var snap = _buffer.CurrentSnapshot;
            TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(snap, 0, snap.Length)));
        }

        private void LoadCoverage()
        {
            if (string.IsNullOrEmpty(_filePath)) return;
            var svc = CoverTreePackage.Instance?.CoverageService;
            _lineCoverage = svc?.GetLineCoverage(_filePath) ?? new Dictionary<int, LineCoverageStatus>();
        }

        public IEnumerable<ITagSpan<CoverageTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var opts = CoverTreePackage.Instance?.Options;
            if (opts == null || !opts.ShowGutterMarkers) yield break;

            foreach (var span in spans)
            {
                int start = span.Start.GetContainingLine().LineNumber;
                int end = span.End.GetContainingLine().LineNumber;

                for (int ln = start; ln <= end; ln++)
                {
                    LineCoverageStatus status;
                    if (_lineCoverage.TryGetValue(ln + 1, out status))
                    {
                        var line = span.Snapshot.GetLineFromLineNumber(ln);
                        yield return new TagSpan<CoverageTag>(
                            new SnapshotSpan(line.Start, line.End),
                            new CoverageTag(status));
                    }
                }
            }
        }

        public void Dispose()
        {
            var svc = CoverTreePackage.Instance?.CoverageService;
            if (svc != null) svc.DataChanged -= OnDataChanged;
        }
    }
}
