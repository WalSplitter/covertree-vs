namespace CoverTree.VS.Options
{
    public class CoverTreeSettings
    {
        public double Threshold { get; set; } = 75.0;
        public string CoverageFile { get; set; } = "coverage/coverage-summary.json";
        public string DetailFile { get; set; } = "coverage/coverage-final.json";
        public bool ShowGutterMarkers { get; set; } = true;
    }
}
