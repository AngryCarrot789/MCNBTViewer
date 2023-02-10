using System.Collections.ObjectModel;
using System.Collections.Specialized;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.Explorer {
    /// <summary>
    /// A type of 
    /// </summary>
    public class FileItemViewModel : BaseViewModel {
        private FolderItemViewModel parent;
        public FolderItemViewModel Parent {
            get => this.parent;
            set => this.RaisePropertyChanged(ref this.parent, value);
        }

        private bool canExpand;
        public bool CanExpand {
            get => this.canExpand = this.QueryCanExpand();
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

        private object data;
        public object Data {
            get => this.data;
            set => this.RaisePropertyChanged(ref this.data, value);
        }

        public FileItemViewModel() {

        }

        protected virtual bool QueryCanExpand() {
            return false;
        }

        protected virtual void OnNameChanged(string oldName, string newName) {

        }

        public virtual void OnAddedToFolder() {

        }

        public virtual void OnRemovedFromFolder() {

        }

        /// <summary>
        /// Called when the user "uses"/double clicks this item
        /// </summary>
        public void OnUserAction() {

        }
    }
}
