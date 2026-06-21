using Microsoft.VisualStudio.Text.Tagging;
using CoverTree.VS.Coverage;

namespace CoverTree.VS.Adornments
{
    public class CoverageTag : IGlyphTag
    {
        public LineCoverageStatus Status { get; }

        public CoverageTag(LineCoverageStatus status)
        {
            Status = status;
        }
    }
}
