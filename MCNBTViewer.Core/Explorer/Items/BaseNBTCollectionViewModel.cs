using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MCNBTViewer.Core.NBT.Explorer.Items {
    public abstract class BaseNBTCollectionViewModel : BaseNBTViewModel {
        public ObservableCollection<BaseNBTViewModel> Children { get; }

        private bool isEmpty;
        public bool IsEmpty {
            get => this.isEmpty;
            set => this.RaisePropertyChanged(ref this.isEmpty, value);
        }

        protected BaseNBTCollectionViewModel(string name, NBTType type) : base(name, type) {
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
        }

        protected override bool CanExpandTreeItem() {
            return this.Children.Count > 0;
        }
    }
}