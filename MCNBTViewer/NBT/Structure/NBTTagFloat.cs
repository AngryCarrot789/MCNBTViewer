using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagFloat : NBTBase {
        public float data;

        public NBTTagFloat(string name) : base(name) {
        }

        public NBTTagFloat(string name, float var2) : base(name) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteFloat(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadFloat();
        }

        public override byte Id => 5;

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagFloat(this.Name, this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                NBTTagFloat var2 = (NBTTagFloat) obj;
                return this.data == var2.data;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ Bits.FloatBitsToI32(this.data);
        }
    }
}