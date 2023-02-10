using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagDouble : NBTBase {
        public double data;

        public NBTTagDouble(string name) : base(name) {
        }

        public NBTTagDouble(string name, double var2) : base(name) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteDouble(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadDouble();
        }

        public override byte Id => 6;

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagDouble(this.Name, this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                NBTTagDouble var2 = (NBTTagDouble) obj;
                return this.data == var2.data;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            ulong var1 = Bits.DoubleBitsToU64(this.data);
            return base.GetHashCode() ^ (int) (var1 ^ var1 >> 32);
        }
    }
}