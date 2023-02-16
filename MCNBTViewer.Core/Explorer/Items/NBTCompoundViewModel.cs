using System;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTCompoundViewModel : BaseNBTCollectionViewModel {
        public NBTCompoundViewModel(string name = null) : base(name, NBTType.Compound) {

        }

        public override NBTBase ToNBT() {
            NBTTagCompound tag = new NBTTagCompound();
            foreach (BaseNBTViewModel item in this.Children) {
                NBTBase nbt = item.ToNBT();
                if (nbt.Id != 0) {
                    if (item.Name == null) {
                        throw new Exception("Tag name cannot be null: " + item);
                    }

                    tag.map[item.Name] = nbt;
                }
            }

            return tag;
        }
    }
}