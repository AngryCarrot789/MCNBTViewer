using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagString : NBTBase {
        public String data;

        public NBTTagString(String name) : base(name) {
        }

        public NBTTagString(String name, String var2) : base(name) {
            this.data = var2;
            if (var2 == null) {
                throw new ArgumentNullException(nameof(var2), "Empty string not allowed");
            }
        }

        public override void Write(DataOutputStream output) {
            output.WriteStringLabelledUTF8(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadStringUTF8Labelled();
        }

        public override byte GetId() {
            return 8;
        }

        public override string ToString() {
            return "" + this.data;
        }

        public override NBTBase Copy() {
            return new NBTTagString(this.GetName(), this.data);
        }

        public override bool Equals(object var1) {
            if (!base.Equals(var1)) {
                return false;
            } else {
                NBTTagString var2 = (NBTTagString)var1;
                return this.data == null && var2.data == null || this.data != null && this.data.Equals(var2.data);
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.data.GetHashCode();
        }
    }
}