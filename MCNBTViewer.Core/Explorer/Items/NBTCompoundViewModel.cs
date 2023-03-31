using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FramePFX.Core;
using MCNBTViewer.Core.AdvancedContextService;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTCompoundViewModel : BaseNBTCollectionViewModel {
        public RelayCommandParam<int> CreateTagCommand { get; }

        public NBTCompoundViewModel(string name = null) : base(name, NBTType.Compound) {
            this.CreateTagCommand = new RelayCommandParam<int>(async (x) => await this.NewTagAction(x));
        }

        private async Task NewTagAction(int type) {
            throw new NotImplementedException();
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

        public override IEnumerable<IBaseContextEntry> GetContextEntries() {
            yield return new ContextEntry("New", null, this.GetNewItemsEntries());
            yield return ContextEntrySeparator.Instance;
            foreach (IBaseContextEntry contextEntry in base.GetContextEntries()) {
                yield return contextEntry;
            }
        }

        public IEnumerable<IBaseContextEntry> GetNewItemsEntries() {
            /*
             *case 0: return new NBTTagEnd();
              case 1: return new NBTTagByte();
              case 2: return new NBTTagShort();
              case 3: return new NBTTagInt();
              case 4: return new NBTTagLong();
              case 5: return new NBTTagFloat();
              case 6: return new NBTTagDouble();
              case 7: return new NBTTagByteArray();
              case 8: return new NBTTagString();
              case 9: return new NBTTagList();
              case 10: return new NBTTagCompound();
              case 11: return new NBTTagIntArray();
             *
             *
             */

            yield return new ContextEntry("End", this.CreateTagCommand, 0);
            yield return new ContextEntry("Byte", this.CreateTagCommand, 1);
            yield return new ContextEntry("Short", this.CreateTagCommand, 2);
            yield return new ContextEntry("Int", this.CreateTagCommand, 3);
            yield return new ContextEntry("Long", this.CreateTagCommand, 4);
            yield return new ContextEntry("Float", this.CreateTagCommand, 5);
            yield return new ContextEntry("Double", this.CreateTagCommand, 6);
            yield return new ContextEntry("String", this.CreateTagCommand, 8);
            yield return new ContextEntry("Byte Array", this.CreateTagCommand, 7);
            yield return new ContextEntry("Int Array", this.CreateTagCommand, 11);
            yield return new ContextEntry("List", this.CreateTagCommand, 9);
            yield return new ContextEntry("Compound", this.CreateTagCommand, 10);
        }
    }
}