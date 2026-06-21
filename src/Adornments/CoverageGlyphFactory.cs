using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using CoverTree.VS.Coverage;

namespace CoverTree.VS.Adornments
{
    internal class CoverageGlyphFactory : IGlyphFactory
    {
        private static readonly Brush Covered  = Freeze(new SolidColorBrush(Color.FromArgb(200, 78, 201, 78)));
        private static readonly Brush Uncovered = Freeze(new SolidColorBrush(Color.FromArgb(200, 241, 76, 76)));
        private static readonly Brush Partial  = Freeze(new SolidColorBrush(Color.FromArgb(200, 204, 167, 0)));

        private static Brush Freeze(SolidColorBrush b) { b.Freeze(); return b; }

        public UIElement GenerateGlyph(IWpfTextViewLine line, IGlyphTag tag)
        {
            var ct = tag as CoverageTag;
            if (ct == null) return null;

            Brush brush;
            switch (ct.Status)
            {
                case LineCoverageStatus.Covered:   brush = Covered;   break;
                case LineCoverageStatus.Uncovered: brush = Uncovered; break;
                case LineCoverageStatus.Partial:   brush = Partial;   break;
                default: return null;
            }

            return new Rectangle
            {
                Width = 4,
                Height = line.TextHeight > 2 ? line.TextHeight - 2 : line.TextHeight,
                Fill = brush,
                Margin = new Thickness(0, 1, 0, 1),
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
}
