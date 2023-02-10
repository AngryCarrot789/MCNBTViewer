using MCNBTViewer.NBT.Explorer.Base;
using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTIntArrayItemViewModel : BaseNBTArrayViewModel {
        public int[] Data { get; set; }

        public NBTIntArrayItemViewModel() {
            this.NBTType = NBTType.IntArray;
        }

        public override NBTBase ToNBT() {
            return new NBTTagIntArray(this.Name, this.Data);
        }
    }
}