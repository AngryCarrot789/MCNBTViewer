using System;
using MCNBTViewer.Core.Utils;
using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagIntArray : NBTBase {
        public int[] data;

        public override byte Id => 11;

        public NBTTagIntArray() {
        }

        public NBTTagIntArray(int[] data) {
            this.data = data;
        }

        public override void Write(IDataOutput output) {
            output.WriteInt(this.data.Length);
            foreach (int value in this.data) {
                output.WriteInt(value);
            }
        }

        public override void Read(IDataInput input, int deep) {
            int size = input.ReadInt();
            this.data = new int[size];
            for (int var4 = 0; var4 < size; ++var4) {
                this.data[var4] = input.ReadInt();
            }
        }

        public override string ToString() {
            return "[" + this.data.Length + " ints]";
        }

        public override NBTBase CloneTag() {
            int[] copy = new int[this.data.Length];
            Array.Copy(this.data, 0, copy, 0, this.data.Length);
            return new NBTTagIntArray(copy);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj) && obj is NBTTagIntArray arr) {
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