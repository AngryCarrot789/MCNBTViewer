using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTByteArrayViewModel : BaseNBTArrayViewModel {
        public byte[] Data { get; set; }

        public NBTByteArrayViewModel(string name = null) : base(name, NBTType.ByteArray) {

        }

        public override NBTBase ToNBT() {
            return new NBTTagByteArray(this.Data);
        }
    }
}