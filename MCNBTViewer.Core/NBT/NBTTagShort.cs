using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagShort : NBTBase {
        public short data;

        public override byte Id => 2;

        public NBTTagShort() {
        }

        public NBTTagShort(short data) {
            this.data = data;
        }

        public override void Write(IDataOutput output) {
            output.WriteShort(this.data);
        }

        public override void Read(IDataInput input, int deep) {
            this.data = input.ReadShort();
        }

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagShort(this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                NBTTagShort var2 = (NBTTagShort) obj;
                return this.data == var2.data;
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