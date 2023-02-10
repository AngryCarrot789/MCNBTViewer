using System;
using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    /// <summary>
    /// A view model for tags end, byte, short, int, long, float, double and string
    /// </summary>
    public class NBTPrimitiveViewModel : BaseNBTViewModel {
        private string data;
        public string Data {
            get => this.data;
            set => this.RaisePropertyChanged(ref this.data, value);
        }

        public NBTPrimitiveViewModel() : this(NBTType.End) {

        }

        public NBTPrimitiveViewModel(NBTType type) : base(type) {

        }

        public override NBTBase ToNBT() {
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
    }
}