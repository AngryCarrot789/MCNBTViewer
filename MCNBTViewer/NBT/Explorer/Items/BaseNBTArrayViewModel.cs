using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public abstract class BaseNBTArrayViewModel : BaseNBTViewModel {
        protected BaseNBTArrayViewModel(string name, NBTType type) : base(name, type) {

        }
    }
}