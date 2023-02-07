using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.ViewModels {
    public class NBTBaseViewModel {
        public NBTBase NBT { get; set; }

        public NBTBaseViewModel() {

        }

        public T GetNBT<T>() where T : NBTBase {
            return (T) this.NBT;
        }
    }
}