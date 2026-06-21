using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoverTree.VS.Coverage
{
    public static class DetailParser
    {
        public static Dictionary<string, JObject> Parse(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return null;
                return JsonConvert.DeserializeObject<Dictionary<string, JObject>>(File.ReadAllText(filePath));
            }
            catch { return null; }
        }

        public static JObject GetFileCoverage(Dictionary<string, JObject> detail, string absolutePath)
        {
            if (detail == null || string.IsNullOrEmpty(absolutePath)) return null;

            var slash = absolutePath.Replace('\\', '/');
            var back = absolutePath.Replace('/', '\\');

            JObject r;
            if (detail.TryGetValue(slash, out r)) return r;
            if (detail.TryGetValue(back, out r)) return r;
            if (detail.TryGetValue(slash.ToUpperInvariant(), out r)) return r;
            if (detail.TryGetValue(slash.ToLowerInvariant(), out r)) return r;
            if (detail.TryGetValue(back.ToUpperInvariant(), out r)) return r;
            if (detail.TryGetValue(back.ToLowerInvariant(), out r)) return r;
            return null;
        }

        public static Dictionary<int, LineCoverageStatus> GetLineCoverageMap(JObject fc)
        {
            var result = new Dictionary<int, LineCoverageStatus>();
            if (fc == null) return result;

            var stmtMap = fc["statementMap"] as JObject;
            var stmts = fc["s"] as JObject;
            var branchMap = fc["branchMap"] as JObject;
            var branches = fc["b"] as JObject;

            if (stmtMap != null && stmts != null)
            {
                foreach (var prop in stmts.Properties())
                {
                    var hits = prop.Value.Value<int>();
                    var loc = stmtMap[prop.Name];
                    if (loc == null) continue;

                    int line = loc["start"]?["line"]?.Value<int>() ?? 0;
                    if (line <= 0) continue;

                    var status = hits > 0 ? LineCoverageStatus.Covered : LineCoverageStatus.Uncovered;
                    if (!result.ContainsKey(line) || result[line] == LineCoverageStatus.Uncovered)
                        result[line] = status;
                }
            }

            if (branchMap != null && branches != null)
            {
                foreach (var prop in branches.Properties())
                {
                    var branchHits = prop.Value as JArray;
                    if (branchHits == null) continue;

                    var bLoc = branchMap[prop.Name];
                    int line = bLoc?["loc"]?["start"]?["line"]?.Value<int>()
                            ?? bLoc?["locations"]?[0]?["start"]?["line"]?.Value<int>()
                            ?? 0;
                    if (line <= 0) continue;

                    bool anyHit = false, anyMiss = false;
                    foreach (var h in branchHits)
                    {
                        if (h.Value<int>() > 0) anyHit = true; else anyMiss = true;
                    }

                    if (anyHit && anyMiss && result.ContainsKey(line))
                        result[line] = LineCoverageStatus.Partial;
                }
            }

            return result;
        }
    }
}
