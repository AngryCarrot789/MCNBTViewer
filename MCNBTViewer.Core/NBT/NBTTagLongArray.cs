using System;
using MCNBTViewer.Core.Utils;
using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagLongArray : NBTBase {
        public long[] data;

        public override byte Id => 12;

        public NBTTagLongArray() {
        }

        public NBTTagLongArray(long[] data) {
            this.data = data;
        }

        public override void Write(IDataOutput output) {
            output.WriteInt(this.data.Length);
            foreach (long value in this.data) {
                output.WriteLong(value);
            }
        }

        public override void Read(IDataInput input, int deep) {
            int size = input.ReadInt();
            this.data = new long[size];
            for (int var4 = 0; var4 < size; ++var4) {
                this.data[var4] = input.ReadLong();
            }
        }

        public override string ToString() {
            return "[" + this.data.Length + " longs]";
        }

        public override NBTBase CloneTag() {
            long[] copy = new long[this.data.Length];
            Array.Copy(this.data, 0, copy, 0, this.data.Length);
            return new NBTTagLongArray(copy);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj) && obj is NBTTagLongArray arr) {
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