using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace CoverTree.VS.Options
{
    public class CoverTreeOptionsPage : DialogPage
    {
        [Category("Coverage")]
        [DisplayName("Threshold (%)")]
        [Description("Minimum coverage percentage to consider a file passing.")]
        [DefaultValue(75.0)]
        public double Threshold { get; set; } = 75.0;

        [Category("Files")]
        [DisplayName("Coverage Summary File")]
        [Description("Path to coverage-summary.json relative to solution root.")]
        [DefaultValue("coverage/coverage-summary.json")]
        public string CoverageFile { get; set; } = "coverage/coverage-summary.json";

        [Category("Files")]
        [DisplayName("Coverage Detail File")]
        [Description("Path to coverage-final.json relative to solution root (for gutter markers).")]
        [DefaultValue("coverage/coverage-final.json")]
        public string DetailFile { get; set; } = "coverage/coverage-final.json";

        [Category("Display")]
        [DisplayName("Show Gutter Markers")]
        [Description("Display line-level coverage indicators in the editor margin.")]
        [DefaultValue(true)]
        public bool ShowGutterMarkers { get; set; } = true;

        public CoverTreeSettings ToSettings() => new CoverTreeSettings
        {
            Threshold = Threshold,
            CoverageFile = CoverageFile,
            DetailFile = DetailFile,
            ShowGutterMarkers = ShowGutterMarkers
        };
    }
}
