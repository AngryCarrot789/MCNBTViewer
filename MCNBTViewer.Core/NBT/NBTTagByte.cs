using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagByte : NBTBase {
        public byte data;

        public NBTTagByte() {
        }

        public NBTTagByte(byte var2) {
            this.data = var2;
        }

        public override void Write(IDataOutput output) {
            output.WriteByte(this.data);
        }

        public override void Read(IDataInput input, int deep) {
            this.data = input.ReadByte();
        }

        public override byte Id => 1;

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagByte(this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                return this.data == ((NBTTagByte) obj).data;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.data;
        }
    }
}