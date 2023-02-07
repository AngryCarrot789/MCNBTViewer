using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagLong : NBTBase {
        public long data;

        public NBTTagLong(String var1) : base(var1) {
        }

        public NBTTagLong(String var1, long var2) : base(var1) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteLong(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadLong();
        }

        public override byte GetId() {
            return 4;
        }

        public override string ToString() {
            return "" + this.data;
        }

        public override NBTBase Copy() {
            return new NBTTagLong(this.GetName(), this.data);
        }

        public override bool Equals(object var1) {
            if (base.Equals(var1)) {
                NBTTagLong var2 = (NBTTagLong)var1;
                return this.data == var2.data;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            ulong x = (ulong) this.data;
            return base.GetHashCode() ^ (int)(x ^ x >> 32);
        }
    }
}