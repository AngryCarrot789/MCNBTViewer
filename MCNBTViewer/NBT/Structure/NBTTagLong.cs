using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagLong : NBTBase {
        public long data;

        public NBTTagLong(string name) : base(name) {
        }

        public NBTTagLong(string name, long data) : base(name) {
            this.data = data;
        }

        public override void Write(DataOutputStream output) {
            output.WriteLong(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadLong();
        }

        public override byte Id => 4;

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagLong(this.Name, this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                NBTTagLong var2 = (NBTTagLong) obj;
                return this.data == var2.data;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            ulong x = (ulong) this.data;
            return base.GetHashCode() ^ (int) (x ^ x >> 32);
        }
    }
}