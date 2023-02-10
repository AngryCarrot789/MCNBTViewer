using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagByteArray : NBTBase {
        public byte[] data;

        public NBTTagByteArray() {
        }

        public NBTTagByteArray(byte[] var2) {
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
            Array.Copy(this.data, 0, var1, 0, this.data.Length);
            return new NBTTagByteArray(var1);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj) && obj is NBTTagByteArray arr) {
                return this.data == null && arr.data == null || this.data != null && Arrays.Equals(this.data, arr.data);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ Arrays.Hash(this.data);
        }
    }
}