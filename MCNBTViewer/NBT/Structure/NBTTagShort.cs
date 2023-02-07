using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagShort : NBTBase {
        public short data;

        public NBTTagShort(String var1) : base(var1) {
        }

        public NBTTagShort(String var1, short var2) : base(var1) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteShort(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadShort();
        }

        public override byte GetId() {
            return 2;
        }

        public override string ToString() {
            return "" + this.data;
        }

        public override NBTBase Copy() {
            return new NBTTagShort(this.GetName(), this.data);
        }

        public override bool Equals(object var1) {
            if (base.Equals(var1)) {
                NBTTagShort var2 = (NBTTagShort)var1;
                return this.data == var2.data;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.data;
        }
    }
}