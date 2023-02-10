using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTCompoundViewModel : BaseNBTCollectionViewModel {
        public NBTCompoundViewModel() : base(NBTType.Compound) {

        }

        public override NBTBase ToNBT() {
            NBTTagCompound tag = new NBTTagCompound(this.Name);
            foreach (BaseNBTViewModel item in this.Children) {
                NBTBase nbt = item.ToNBT();
                if (nbt.GetId() != 0) {
                    tag.tagMap[nbt.name] = nbt;
                }
            }

            return tag;
        }
    }
}