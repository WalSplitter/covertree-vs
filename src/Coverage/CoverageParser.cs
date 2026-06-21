using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using CoverTree.VS.Options;

namespace CoverTree.VS.Coverage
{
    public static class CoverageParser
    {
        public static Dictionary<string, FileCoverage> Parse(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return null;
                return JsonConvert.DeserializeObject<Dictionary<string, FileCoverage>>(File.ReadAllText(filePath));
            }
            catch { return null; }
        }

        public static FileCoverage GetFileCoverage(Dictionary<string, FileCoverage> summary, string absolutePath)
        {
            if (summary == null || string.IsNullOrEmpty(absolutePath)) return null;

            var slash = absolutePath.Replace('\\', '/');
            var back = absolutePath.Replace('/', '\\');

            FileCoverage r;
            if (summary.TryGetValue(slash, out r)) return r;
            if (summary.TryGetValue(back, out r)) return r;
            if (summary.TryGetValue(slash.ToUpperInvariant(), out r)) return r;
            if (summary.TryGetValue(slash.ToLowerInvariant(), out r)) return r;
            if (summary.TryGetValue(back.ToUpperInvariant(), out r)) return r;
            if (summary.TryGetValue(back.ToLowerInvariant(), out r)) return r;
            return null;
        }

        public static double GetOverallPct(FileCoverage c)
        {
            if (c == null) return 0;
            double sum = 0; int count = 0;
            foreach (var v in new[] { c.Lines?.Pct ?? double.NaN, c.Functions?.Pct ?? double.NaN,
                                      c.Statements?.Pct ?? double.NaN, c.Branches?.Pct ?? double.NaN })
            {
                if (!double.IsNaN(v)) { sum += v; count++; }
            }
            return count > 0 ? sum / count : 0;
        }

        public static CoverageStatus GetStatus(double pct, CoverTreeSettings opts)
        {
            if (pct < 0) return CoverageStatus.None;
            return pct >= (opts?.Threshold ?? 75) ? CoverageStatus.Passing : CoverageStatus.Warning;
        }
    }
}
