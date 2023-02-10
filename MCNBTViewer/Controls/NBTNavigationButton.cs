using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.NBT.Explorer.Items;

namespace MCNBTViewer.Controls {
    public class NavigationButton : Control {
        public static readonly DependencyProperty TargetItemProperty = DependencyProperty.Register("TargetItem", typeof(BaseNBTViewModel), typeof(NavigationButton), new PropertyMetadata(null));

        public BaseNBTViewModel TargetItem {
            get => (BaseNBTViewModel) this.GetValue(TargetItemProperty);
            set => this.SetValue(TargetItemProperty, value);
        }
    }
}