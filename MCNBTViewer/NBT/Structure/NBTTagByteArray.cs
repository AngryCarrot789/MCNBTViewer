using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagByteArray : NBTBase {
        public byte[] data;

        public NBTTagByteArray(string name) : base(name) {
        }

        public NBTTagByteArray(string name, byte[] var2) : base(name) {
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

        public override byte Id => 7;

        public override string ToString() {
            return "[" + this.data.Length + " bytes]";
        }

        public override NBTBase CloneTag() {
            byte[] var1 = new byte[this.data.Length];
            Array.Copy(this.data, 0, var1, 0, (int) this.data.Length);
            return new NBTTagByteArray(this.Name, var1);
        }

        public override bool Equals(object obj) {
            return base.Equals(obj) ? Arrays.Equals(this.data, ((NBTTagByteArray) obj).data) : false;
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ Arrays.Hash(this.data);
        }
    }
}