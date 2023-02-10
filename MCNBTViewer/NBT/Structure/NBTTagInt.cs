using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagInt : NBTBase {
        public int data;

        public NBTTagInt(String name) : base(name) {
        }

        public NBTTagInt(String name, int var2) : base(name) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteInt(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadInt();
        }

        public override byte GetId() {
            return 3;
        }

        public override string ToString() {
            return "" + this.data;
        }

        public override NBTBase Copy() {
            return new NBTTagInt(this.GetName(), this.data);
        }

        public override bool Equals(object var1) {
            if (base.Equals(var1)) {
                NBTTagInt var2 = (NBTTagInt)var1;
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