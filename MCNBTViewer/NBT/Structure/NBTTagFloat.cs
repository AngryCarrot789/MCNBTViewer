using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagFloat : NBTBase {
        public float data;

        public NBTTagFloat(String name) : base(name) {
        }

        public NBTTagFloat(String name, float var2) : base(name) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteFloat(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadFloat();
        }

        public override byte GetId() {
            return 5;
        }

        public override string ToString() {
            return "" + this.data;
        }

        public override NBTBase Copy() {
            return new NBTTagFloat(this.GetName(), this.data);
        }

        public override bool Equals(object var1) {
            if (base.Equals(var1)) {
                NBTTagFloat var2 = (NBTTagFloat)var1;
                return this.data == var2.data;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ Bits.FloatBitsToI32(this.data);
        }
    }
}