using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagEnd : NBTBase {
        public NBTTagEnd() {

        }

        public override void Write(DataOutputStream output) {
        }

        public override void Read(DataInputStream input, int deep) {
        }

        public override byte Id => 0;

        public override NBTBase CloneTag() {
            return new NBTTagEnd();
        }

        public override string ToString() {
            return "END";
        }
    }
}