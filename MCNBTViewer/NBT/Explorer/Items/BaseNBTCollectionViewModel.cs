using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MCNBTViewer.NBT.Structure;

namespace MCNBTViewer.NBT.Explorer.Items {
    public abstract class BaseNBTCollectionViewModel : BaseNBTViewModel {
        public ObservableCollection<BaseNBTViewModel> Children { get; }

        private string folderNameFinal;
        public string FolderNameFinal {
            get => this.folderNameFinal;
            set => this.RaisePropertyChanged(ref this.folderNameFinal, value);
        }

        private bool isEmpty;
        public bool IsEmpty {
            get => this.isEmpty;
            set => this.RaisePropertyChanged(ref this.isEmpty, value);
        }

        protected BaseNBTCollectionViewModel(NBTType type) : base(type) {
            this.Children = new ObservableCollection<BaseNBTViewModel>();
            this.Children.CollectionChanged += this.OnChildrenChanged;
        }

        protected virtual void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.OldItems != null) {
                foreach (BaseNBTViewModel item in e.OldItems) {
                    item.OnRemovedFromFolder();
                    item.Parent = null;
                }
            }

            if (e.NewItems != null) {
                foreach (BaseNBTViewModel item in e.NewItems) {
                    item.Parent = this;
                    item.OnAddedToFolder();
                }
            }

            this.IsEmpty = this.Children.Count == 0;
            this.UpdateFolderName();
        }

        protected override bool CanExpandTreeItem() {
            return this.Children.Count > 0;
        }

        protected override void OnNameChanged(string oldName, string name) {
            this.UpdateFolderName();
        }

        protected void UpdateFolderName() {
            if (string.IsNullOrEmpty(this.Name)) {
                this.FolderNameFinal = $"{this.Children.Count} entries";
            }
            else {
                this.FolderNameFinal = $"{this.Name}: {this.Children.Count} entries";
            }
        }
    }
}