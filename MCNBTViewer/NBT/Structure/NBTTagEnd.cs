using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagEnd : NBTBase {
        public NBTTagEnd() : base() {
        }

        public override void Write(DataOutputStream output) {

        }

        public override void Read(DataInputStream input, int deep) {

        }

        public override byte GetId() {
            return 0;
        }

        public override NBTBase Copy() {
            return new NBTTagEnd();
        }

        public override string ToString() {
            return "END";
        }
    }
}