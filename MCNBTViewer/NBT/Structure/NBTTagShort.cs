using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagShort : NBTBase {
        public short data;

        public NBTTagShort(string name) : base(name) {
        }

        public NBTTagShort(string name, short data) : base(name) {
            this.data = data;
        }

        public override void Write(DataOutputStream output) {
            output.WriteShort(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadShort();
        }

        public override byte Id => 2;

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagShort(this.Name, this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                NBTTagShort var2 = (NBTTagShort) obj;
                return this.data == var2.data;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.data;
        }
    }
}