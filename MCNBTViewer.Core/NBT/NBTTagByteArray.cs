using System;
using MCNBTViewer.Core.Utils;
using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagByteArray : NBTBase {
        public byte[] data;

        public override byte Id => 7;

        public NBTTagByteArray() {
        }

        public NBTTagByteArray(byte[] var2) {
            this.data = var2;
        }

        public override void Write(IDataOutput output) {
            output.WriteInt(this.data.Length);
            output.Write(this.data);
        }

        public override void Read(IDataInput input, int deep) {
            int size = input.ReadInt();
            this.data = new byte[size];
            input.ReadFully(this.data);
        }

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