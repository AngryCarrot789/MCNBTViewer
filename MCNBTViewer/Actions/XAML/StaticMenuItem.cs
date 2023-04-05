using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;

namespace MCNBTViewer.DynUI.XAML {
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    public class StaticMenuItem : StaticMenuElement {
        private StaticMenuItemCollection items;

        public string Header { get; set; }

        public string InputGestureText { get; set; }

        public ImageSource Icon { get; set; }

        public string ActionID { get; set; }

        public StaticMenuItemCollection Items {
            get => this.items ?? (this.items = new StaticMenuItemCollection());
        }
    }
}