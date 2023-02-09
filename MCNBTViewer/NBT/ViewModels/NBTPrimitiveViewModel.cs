using System;
using MCNBTViewer.NBT.Structure;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.NBT.ViewModels {
    /// <summary>
    /// A view model for primitive NBT data (end, byte, short, int, long, string, float, double)
    /// </summary>
    public class NBTPrimitiveViewModel : NBTBaseViewModel {
        private string data;
        public string Data {
            get => this.data;
            set => this.RaisePropertyChanged(ref this.data, value);
        }

        public override NBTBase WriteToNBT() {
            switch (this.NBTType) {
                case NBTType.End: return new NBTTagEnd();
                case NBTType.Byte: return new NBTTagByte(this.Name, byte.Parse(this.data));
                case NBTType.Short: return new NBTTagShort(this.Name, short.Parse(this.data));
                case NBTType.Int: return new NBTTagInt(this.Name, int.Parse(this.data));
                case NBTType.Long: return new NBTTagLong(this.Name, long.Parse(this.data));
                case NBTType.Float: return new NBTTagFloat(this.Name, float.Parse(this.data));
                case NBTType.Double: return new NBTTagDouble(this.Name, double.Parse(this.data));
                case NBTType.String: return new NBTTagString(this.Name, this.data);
            }

            throw new Exception($"Unsupported. This = {this.GetType()}, NBT Type = {this.NBTType}");
        }

        public override void ReadFromNBT(NBTBase nbt) {
            switch (nbt) {
                case NBTTagEnd _: return;
                case NBTTagByte t: this.data = t.data.ToString(); break;
                case NBTTagShort t: this.data = t.data.ToString(); break;
                case NBTTagInt t: this.data = t.data.ToString(); break;
                case NBTTagLong t: this.data = t.data.ToString(); break;
                case NBTTagFloat t: this.data = t.data.ToString(); break;
                case NBTTagDouble t: this.data = t.data.ToString(); break;
                case NBTTagString t: this.data = t.data; break;
                default: throw new Exception($"Unsupported. This = {this.GetType()}, NBT Type = {this.NBTType}");
            }

            this.Name = nbt.tagName;
        }
    }

    public class NBTTagByteArrayViewModel : NBTBaseViewModel {
        public byte[] ToByteArray() {
            return null;
        }

        public override NBTBase WriteToNBT() {
            return new NBTTagByteArray(this.Name, ToByteArray());
        }

        public override void ReadFromNBT(NBTBase nbt) {
            throw new NotImplementedException();
        }
    }
}