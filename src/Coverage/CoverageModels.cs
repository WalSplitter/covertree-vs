namespace CoverTree.VS.Coverage
{
    public class CoverageMetric
    {
        public int Total { get; set; }
        public int Covered { get; set; }
        public int Skipped { get; set; }
        public double Pct { get; set; }
    }

    public class FileCoverage
    {
        public CoverageMetric Lines { get; set; }
        public CoverageMetric Functions { get; set; }
        public CoverageMetric Statements { get; set; }
        public CoverageMetric Branches { get; set; }
    }

    public enum CoverageStatus { None, Passing, Warning, Failing }

    public enum LineCoverageStatus { Covered, Uncovered, Partial }
}
