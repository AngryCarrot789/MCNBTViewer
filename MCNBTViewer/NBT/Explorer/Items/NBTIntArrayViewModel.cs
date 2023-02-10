using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTIntArrayViewModel : BaseNBTArrayViewModel {
        public int[] Data { get; set; }

        public NBTIntArrayViewModel() : base(NBTType.IntArray) {

        }

        public override NBTBase ToNBT() {
            return new NBTTagIntArray(this.Name, this.Data);
        }
    }
}