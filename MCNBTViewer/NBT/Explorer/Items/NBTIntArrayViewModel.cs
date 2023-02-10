using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public class NBTIntArrayViewModel : BaseNBTArrayViewModel {
        public int[] Data { get; set; }

        public NBTIntArrayViewModel(string name = null) : base(name, NBTType.IntArray) {

        }

        public override NBTBase ToNBT() {
            return new NBTTagIntArray(this.Data);
        }
    }
}