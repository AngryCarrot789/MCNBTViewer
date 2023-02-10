using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagIntArray : NBTBase {
        public int[] data;

        public NBTTagIntArray(string name) : base(name) {
        }

        public NBTTagIntArray(string name, int[] data) : base(name) {
            this.data = data;
        }

        public override void Write(DataOutputStream output) {
            output.WriteInt(this.data.Length);
            foreach (int value in this.data) {
                output.WriteInt(value);
            }
        }

        public override void Read(DataInputStream input, int deep) {
            int var3 = input.ReadInt();
            this.data = new int[var3];

            for (int var4 = 0; var4 < var3; ++var4) {
                this.data[var4] = input.ReadInt();
            }
        }

        public override byte Id => 11;

        public override string ToString() {
            return "[" + this.data.Length + " bytes]";
        }

        public override NBTBase CloneTag() {
            int[] var1 = new int[this.data.Length];
            Array.Copy(this.data, 0, var1, 0, (int) this.data.Length);
            return new NBTTagIntArray(this.Name, var1);
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