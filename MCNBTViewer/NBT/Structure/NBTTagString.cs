using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagString : NBTBase {
        public override byte Id => 8;

        public string Data { get; set; }

        public NBTTagString(string name) : base(name) {

        }

        public NBTTagString(string name, string data) : base(name) {
            this.Data = data;
        }

        public override void Write(DataOutputStream output) {
            if (this.Data == null) {
                output.WriteUShort(0);
            }
            else {
                output.WriteStringLabelledUTF8(this.Data);
            }
        }

        public override void Read(DataInputStream input, int deep) {
            this.Data = input.ReadStringUTF8Labelled();
        }

        public override string ToString() {
            return this.Data;
        }

        public override NBTBase CloneTag() {
            return new NBTTagString(this.Name, this.Data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj) && obj is NBTTagString str) {
                return this.Data == null && str.Data == null || string.Equals(this.Data, str.Data);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.Data.GetHashCode();
        }
    }
}