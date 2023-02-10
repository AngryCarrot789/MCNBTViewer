using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagByte : NBTBase {
        public byte data;

        public NBTTagByte(string name) : base(name) {
        }

        public NBTTagByte(string name, byte var2) : base(name) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteByte(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadByte();
        }

        public override byte Id => 1;

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagByte(this.Name, this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                return this.data == ((NBTTagByte) obj).data;
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