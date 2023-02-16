using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagString : NBTBase {
        public override byte Id => 8;

        public string data;

        public NBTTagString() {

        }

        public NBTTagString(string data) {
            this.data = data;
        }

        public override void Write(DataOutputStream output) {
            if (this.data == null) {
                output.WriteUShort(0);
            }
            else {
                output.WriteStringLabelledUTF8(this.data);
            }
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadStringUTF8Labelled();
        }

        public override string ToString() {
            return this.data;
        }

        public override NBTBase CloneTag() {
            return new NBTTagString(this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj) && obj is NBTTagString str) {
                return this.data == null && str.data == null || string.Equals(this.data, str.data);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.data.GetHashCode();
        }
    }
}