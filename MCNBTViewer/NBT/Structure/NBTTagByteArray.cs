using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagByteArray : NBTBase {
        public byte[] data;

        public NBTTagByteArray(String var1) : base(var1) {
        }

        public NBTTagByteArray(String var1, byte[] var2) : base(var1) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteInt(this.data.Length);
            output.Write(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            int var3 = input.ReadInt();
            this.data = new byte[var3];
            input.ReadFully(this.data);
        }

        public override byte GetId() {
            return 7;
        }

        public override string ToString() {
            return "[" + this.data.Length + " bytes]";
        }

        public override NBTBase Copy() {
            byte[] var1 = new byte[this.data.Length];
            Array.Copy(this.data, 0, var1, 0, (int) this.data.Length);
            return new NBTTagByteArray(this.GetName(), var1);
        }

        public override bool Equals(object var1) {
            return base.Equals(var1) ? Arrays.Equals(this.data, ((NBTTagByteArray)var1).data) : false;
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ Arrays.Hash(this.data);
        }
    }
}