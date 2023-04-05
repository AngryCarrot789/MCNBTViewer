namespace MCNBTViewer.Core.Explorer.New.Trees {
    public class BaseTreeItemViewModel : BaseViewModel {
        private TreeFolderViewModel parentTreeExpander;
        public virtual TreeFolderViewModel ParentTreeExpander {
            get => this.parentTreeExpander;
            set => this.RaisePropertyChanged(ref this.parentTreeExpander, value);
        }

        protected BaseTreeItemViewModel() {

        }
    }
}