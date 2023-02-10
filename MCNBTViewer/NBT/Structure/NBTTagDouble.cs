using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagDouble : NBTBase {
        public double data;

        public NBTTagDouble(String name) : base(name) {
        }

        public NBTTagDouble(String name, double var2) : base(name) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteDouble(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadDouble();
        }

        public override byte GetId() {
            return 6;
        }

        public override string ToString() {
            return "" + this.data;
        }

        public override NBTBase Copy() {
            return new NBTTagDouble(this.GetName(), this.data);
        }

        public override bool Equals(object var1) {
            if (base.Equals(var1)) {
                NBTTagDouble var2 = (NBTTagDouble)var1;
                return this.data == var2.data;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            ulong var1 = Bits.DoubleBitsToU64(this.data);
            return base.GetHashCode() ^ (int)(var1 ^ var1 >> 32);
        }
    }
}