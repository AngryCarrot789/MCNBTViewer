using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using TextRange = MCNBTViewer.Core.Explorer.Finding.TextRange;

namespace MCNBTViewer.NBT.Explorer.Dialogs.Inlines {
    public class BaseInlineHighlightConverter {
        public Style NormalRunStyle { get; set; }
        public Style HighlightedRunStyle { get; set; }

        public Run CreateNormalRun(string text = null) {
            return this.NormalRunStyle != null ? new Run(text) { Style = this.NormalRunStyle } : new Run(text);
        }

        public Run CreateHighlightedRun(string text = null) {
            return this.HighlightedRunStyle != null ? new Run(text) { Style = this.HighlightedRunStyle } : new Run(text) {
                Background = new SolidColorBrush(Colors.Goldenrod),
                Foreground = new SolidColorBrush(Colors.Black)
            };
        }

        public IEnumerable<Run> CreateString(string text, IEnumerable<TextRange> ranges) {
            return InlineHelper.CreateHighlight(text, ranges, this.CreateNormalRun, this.CreateHighlightedRun);
        }
    }
}