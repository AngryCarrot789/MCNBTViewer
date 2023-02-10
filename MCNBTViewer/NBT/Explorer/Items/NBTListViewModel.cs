using MCNBTViewer.NBT.Explorer.Base;
using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTListItemViewModel : BaseNBTCollectionViewModel {
        public NBTListItemViewModel() {
            this.NBTType = NBTType.List;
        }

        public override NBTBase ToNBT() {
            NBTTagList list = new NBTTagList(this.Name);
            foreach (BaseNBTViewModel item in this.Children) {
                list.list.Add(item.ToNBT());
            }
            return list;
        }
    }
}