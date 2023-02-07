using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagIntArray : NBTBase {
        public int[] data;

        public NBTTagIntArray(String var1) : base(var1) {
        }

        public NBTTagIntArray(String var1, int[] var2) : base(var1) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteInt(this.data.Length);

            for(int var2 = 0; var2 < this.data.Length; ++var2) {
                output.WriteInt(this.data[var2]);
            }

        }

        public override void Read(DataInputStream input, int deep) {
            int var3 = input.ReadInt();
            this.data = new int[var3];

            for(int var4 = 0; var4 < var3; ++var4) {
                this.data[var4] = input.ReadInt();
            }

        }

        public override byte GetId() {
            return 11;
        }

        public override string ToString() {
            return "[" + this.data.Length + " bytes]";
        }

        public override NBTBase Copy() {
            int[] var1 = new int[this.data.Length];
            Array.Copy(this.data, 0, var1, 0, (int) this.data.Length);
            return new NBTTagIntArray(this.GetName(), var1);
        }

        public override bool Equals(object var1) {
            if (!base.Equals(var1)) {
                return false;
            } else {
                NBTTagIntArray var2 = (NBTTagIntArray)var1;
                return this.data == null && var2.data == null || this.data != null && Arrays.Equals(this.data, var2.data);
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ Arrays.Hash(this.data);
        }
    }
}