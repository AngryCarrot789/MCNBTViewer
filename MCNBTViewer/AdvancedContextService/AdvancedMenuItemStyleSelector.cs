using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.Core.AdvancedContextService;

namespace MCNBTViewer.AdvancedContextService {
    public class AdvancedMenuItemStyleSelector : StyleSelector {
        public Style NonCheckableMenuItemStyle { get; set; }
        public Style CheckableMenuItemStyle { get; set; }

        public Style SeparatorStyle { get; set; }

        public AdvancedMenuItemStyleSelector() {

        }

        public override Style SelectStyle(object item, DependencyObject container) {
            if (container is MenuItem) {
                return item is ContextEntryCheckable ? this.CheckableMenuItemStyle : this.NonCheckableMenuItemStyle;
            }
            else if (container is Separator) {
                return this.SeparatorStyle;
            }
            else {
                return base.SelectStyle(item, container);
            }
        }
    }
}