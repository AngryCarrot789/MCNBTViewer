using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagInt : NBTBase {
        public int data;

        public NBTTagInt(string name) : base(name) {
        }

        public NBTTagInt(string name, int var2) : base(name) {
            this.data = var2;
        }

        public override void Write(DataOutputStream output) {
            output.WriteInt(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadInt();
        }

        public override byte Id => 3;

        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagInt(this.Name, this.data);
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) {
                NBTTagInt var2 = (NBTTagInt) obj;
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