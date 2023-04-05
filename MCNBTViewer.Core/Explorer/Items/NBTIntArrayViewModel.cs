using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTIntArrayViewModel : BaseNBTArrayViewModel {
        public int[] Data { get; set; }

        public NBTIntArrayViewModel(string name = null) : base(name, NBTType.IntArray) {

        }

        public override NBTBase ToNBT() {
            return new NBTTagIntArray(this.Data);
        }

        protected override void SetData(NBTBase nbt) {
            this.Data = ((NBTTagIntArray) nbt).data;
        }
    }
}