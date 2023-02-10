using MCNBTViewer.NBT.Structure;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.NBT.Explorer {
    public abstract class NBTBaseItemViewModel : BaseViewModel {
        private NBTType nbtType;
        public NBTType NBTType {
            get => this.nbtType;
            set => this.RaisePropertyChanged(ref this.nbtType, value);
        }

        private bool canExpand;
        public bool CanExpand {
            get => this.canExpand = this.CanExpandTreeItem();
            set => this.RaisePropertyChanged(ref this.canExpand, value);
        }

        private bool isExpanded;
        public bool IsExpanded {
            get => this.isExpanded;
            set => this.RaisePropertyChanged(ref this.isExpanded, value);
        }

        protected string name;
        public string Name {
            get => this.name;
            set => this.RaisePropertyChanged(ref this.name, value);
        }

        public NBTBaseItemViewModel() {

        }

        public NBTBaseItemViewModel(NBTType type) {
            this.NBTType = type;
        }

        public abstract NBTBase ToNBT();

        protected virtual bool CanExpandTreeItem() {
            return false;
        }
    }
}