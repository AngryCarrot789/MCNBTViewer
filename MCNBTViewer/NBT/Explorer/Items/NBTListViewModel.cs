using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTListViewModel : BaseNBTCollectionViewModel {
        public NBTListViewModel() : base(NBTType.List) {

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