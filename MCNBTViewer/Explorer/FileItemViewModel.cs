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

        public virtual FileItemType FileType => FileItemType.File;

        protected string name;
        public virtual string Name {
            get => this.name;
            set => this.RaisePropertyChanged(ref this.name, value);
        }

        public FileItemViewModel() {

        }

        protected virtual bool QueryCanExpand() {
            return false;
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
