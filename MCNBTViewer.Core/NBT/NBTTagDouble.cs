using MCNBTViewer.Core.Utils;
using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagDouble : NBTBase {
        public double data;

        public override byte Id => 6;

        public NBTTagDouble() {
        }

        public NBTTagDouble(double var2) {
            this.data = var2;
        }

        public override void Write(IDataOutput output) {
            output.WriteDouble(this.data);
        }

        public override void Read(IDataInput input, int deep) {
            this.data = input.ReadDouble();
        }

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagDouble(this.data);
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