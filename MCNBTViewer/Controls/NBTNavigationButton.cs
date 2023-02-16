using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Controls {
    public class NBTNavigationButton : Control {
        public static readonly DependencyProperty TargetItemProperty = DependencyProperty.Register("TargetItem", typeof(BaseNBTViewModel), typeof(NBTNavigationButton), new PropertyMetadata(null));

        public BaseNBTViewModel TargetItem {
            get => (BaseNBTViewModel) this.GetValue(TargetItemProperty);
            set => this.SetValue(TargetItemProperty, value);
        }
    }
}