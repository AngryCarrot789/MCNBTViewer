using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTListViewModel : BaseNBTCollectionViewModel {
        public NBTListViewModel(string name = null) : base(name, NBTType.List) {

        }

        public override NBTBase ToNBT() {
            NBTTagList list = new NBTTagList();
            foreach (BaseNBTViewModel item in this.Children) {
                list.tags.Add(item.ToNBT());
            }
            return list;
        }
    }
}