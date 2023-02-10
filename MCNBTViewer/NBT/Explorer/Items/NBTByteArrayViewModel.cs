using MCNBTViewer.NBT.Explorer.Base;
using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTByteArrayItemViewModel : BaseNBTArrayViewModel {
        public byte[] Data { get; set; }

        public NBTByteArrayItemViewModel() {
            this.NBTType = NBTType.ByteArray;
        }

        public override NBTBase ToNBT() {
            return new NBTTagByteArray(this.Name, this.Data);
        }
    }
}