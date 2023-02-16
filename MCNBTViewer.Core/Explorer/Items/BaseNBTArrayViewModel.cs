using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Items {
    public abstract class BaseNBTArrayViewModel : BaseNBTViewModel {
        protected BaseNBTArrayViewModel(string name, NBTType type) : base(name, type) {

        }
    }
}