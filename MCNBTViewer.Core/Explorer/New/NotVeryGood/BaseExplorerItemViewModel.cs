namespace MCNBTViewer.Core.Explorer.New.NotVeryGood {
    public abstract class BaseExplorerItemViewModel : BaseViewModel {
        private ExplorerFolderViewModel parent;
        public ExplorerFolderViewModel Parent {
            get => this.parent;
            set => this.RaisePropertyChanged(ref this.parent, value);
        }

        private ExplorerViewModel explorer;
        public ExplorerViewModel Explorer {
            get => this.explorer;
            set => this.RaisePropertyChanged(ref this.explorer, value);
        }

        protected BaseExplorerItemViewModel() {

        }
    }
}