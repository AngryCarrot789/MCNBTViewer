using System;
using System.Linq;
using System.Threading.Tasks;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTCompoundViewModel : BaseNBTCollectionViewModel {
        public NBTCompoundViewModel(string name = null) : base(name, NBTType.Compound) {
        }

        public override async Task PasteNBTBinaryDataAction(string name, NBTBase nbt) {
            if (string.IsNullOrEmpty(name)) {
                await IoC.MessageDialogs.ShowMessageAsync("Invalid NBT", "No name associated with the tag. Cannot add it to a compound");
                return;
            }

            if (this.Children.Any(x => x.Name == name)) {
                await IoC.MessageDialogs.ShowMessageAsync("Already exists", "A tag already exists with the name: " + name);
                return;
            }

            this.Children.Add(CreateFrom(name, nbt));
        }

        public override NBTBase ToNBT() {
            NBTTagCompound tag = new NBTTagCompound();
            foreach (BaseNBTViewModel item in this.Children) {
                NBTBase nbt = item.ToNBT();
                if (nbt.Id != 0) {
                    if (string.IsNullOrEmpty(item.Name)) {
                        throw new Exception("Tag name cannot be null or empty: " + item);
                    }

                    tag.map[item.Name] = nbt;
                }
            }

            return tag;
        }
    }
}