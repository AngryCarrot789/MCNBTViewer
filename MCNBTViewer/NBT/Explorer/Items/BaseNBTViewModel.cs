using MCNBTViewer.NBT.Structure;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.NBT.Explorer.Items {
    public abstract class BaseNBTViewModel : BaseViewModel {
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
            set {
                string old = this.name;
                this.RaisePropertyChanged(ref this.name, value);
                this.OnNameChanged(old, value);
            }
        }

        private BaseNBTCollectionViewModel parent;
        public BaseNBTCollectionViewModel Parent {
            get => this.parent;
            set => this.RaisePropertyChanged(ref this.parent, value);
        }

        protected BaseNBTViewModel(NBTType type) {
            this.NBTType = type;
        }

        public abstract NBTBase ToNBT();

        protected virtual bool CanExpandTreeItem() {
            return false;
        }

        protected virtual void OnNameChanged(string oldName, string name) {

        }

        public virtual void OnRemovedFromFolder() {

        }

        public virtual void OnAddedToFolder() {

        }
    }
}