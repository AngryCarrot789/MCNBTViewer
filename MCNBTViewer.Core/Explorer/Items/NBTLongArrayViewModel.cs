using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public class NBTLongArrayViewModel : BaseNBTArrayViewModel {
        public long[] Data { get; set; }

        public NBTLongArrayViewModel(string name = null) : base(name, NBTType.LongArray) {

        }

        public override NBTBase ToNBT() {
            return new NBTTagLongArray(this.Data);
        }

        protected override void SetData(NBTBase nbt) {
            this.Data = ((NBTTagLongArray) nbt).data;
        }
    }
}