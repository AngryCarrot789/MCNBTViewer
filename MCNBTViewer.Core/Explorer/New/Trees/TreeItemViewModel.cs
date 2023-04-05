namespace MCNBTViewer.Core.Explorer.New.Trees {
    public class TreeItemViewModel : BaseViewModel {
        private TreeFolderViewModel parentTreeExpander;
        public virtual TreeFolderViewModel ParentTreeExpander {
            get => this.parentTreeExpander;
            set => this.RaisePropertyChanged(ref this.parentTreeExpander, value);
        }

        public TreeItemViewModel() {

        }
    }
}