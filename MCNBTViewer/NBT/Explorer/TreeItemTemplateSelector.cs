using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.NBT.Explorer.Items;

namespace MCNBTViewer.NBT.Explorer {
    public class TreeItemTemplateSelector : DataTemplateSelector {
        public DataTemplate TagPrimitive { get; set; }
        public DataTemplate TagArray { get; set; }
        public DataTemplate TagList { get; set; }
        public DataTemplate TagCompound { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            switch (item) {
                case NBTPrimitiveViewModel _: return this.TagPrimitive;
                case BaseNBTArrayViewModel _: return this.TagArray;
                case NBTListViewModel _: return this.TagList;
                case NBTCompoundViewModel _: return this.TagCompound;
                default: return this.TagPrimitive;
            }
        }
    }
}