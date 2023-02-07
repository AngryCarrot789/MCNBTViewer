using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagByte : NBTBase {
        public byte data;

        public NBTTagByte(String var1) : base(var1) {
        }

        public NBTTagByte(String var1, byte var2) : base(var1) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteByte(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadByte();
        }

        public override byte GetId() {
            return 1;
        }

        public override string ToString() {
            return "" + this.data;
        }

        public override NBTBase Copy() {
            return new NBTTagByte(this.GetName(), this.data);
        }

        public override bool Equals(object var1) {
            if (base.Equals(var1)) {
                return this.data == ((NBTTagByte) var1).data;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.data;
        }
    }
}