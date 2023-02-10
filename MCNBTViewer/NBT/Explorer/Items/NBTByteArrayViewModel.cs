using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTByteArrayViewModel : BaseNBTArrayViewModel {
        public byte[] Data { get; set; }

        public NBTByteArrayViewModel() : base(NBTType.ByteArray) {

        }

        public override NBTBase ToNBT() {
            return new NBTTagByteArray(this.Name, this.Data);
        }
    }
}