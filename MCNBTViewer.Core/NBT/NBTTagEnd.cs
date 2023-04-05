using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public class NBTTagEnd : NBTBase {
        public override byte Id => 0;

        public NBTTagEnd() {

        }

        public override void Write(IDataOutput output) {
        }

        public override void Read(IDataInput input, int deep) {
        }

        public override NBTBase CloneTag() {
            return new NBTTagEnd();
        }

        public override string ToString() {
            return "END";
        }
    }
}