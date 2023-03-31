using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.Core.Explorer.Finding;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.NBT.Explorer.Dialogs {
    public class FindResultTemplateSelector : DataTemplateSelector {
        public DataTemplate PrimitiveNBTTemplate { get; set; }
        public DataTemplate ArrayNBTTemplate { get; set; }
        public DataTemplate ListNBTTemplate { get; set; }
        public DataTemplate CompoundNBTTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            if (item is NBTMatchResult result) {
                switch (result.NBT) {
                    case NBTPrimitiveViewModel _: return this.PrimitiveNBTTemplate;
                    case BaseNBTArrayViewModel _: return this.ArrayNBTTemplate;
                    case NBTListViewModel _: return this.ListNBTTemplate;
                    case NBTCompoundViewModel _: return this.CompoundNBTTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}