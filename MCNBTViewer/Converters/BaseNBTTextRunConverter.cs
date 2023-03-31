using System.Windows;
using System.Windows.Documents;

namespace MCNBTViewer.Converters {
    public abstract class BaseNBTTextRunConverter {
        public Style NormalTextRunStyle { get; set; }
        public Style ExtraDataTextRunStyle { get; set; }

        public Run CreateNormalRun(string text) {
            Run run = this.NormalTextRunStyle != null ? new Run() { Style = this.NormalTextRunStyle } : new Run();
            run.Text = text;
            return run;
        }

        public Run CreateExtraRun(string text) {
            Run run = this.ExtraDataTextRunStyle != null ? new Run() { Style = this.ExtraDataTextRunStyle } : new Run() {
                FontStyle = FontStyles.Italic
            };

            run.Text = text;
            return run;
        }
    }
}