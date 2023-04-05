namespace MCNBTViewer.Core.Explorer.New {
    public class ExplorerViewModel : BaseViewModel {
        private ExplorerFolderViewModel rootFolder;
        public ExplorerFolderViewModel RootFolder {
            get => this.rootFolder;
            protected set {
                if (ReferenceEquals(value, this.rootFolder)) {
                    this.RaisePropertyChanged();
                }
                else {
                    if (this.rootFolder != null) {
                        this.rootFolder.Explorer = null;
                    }

                    this.RaisePropertyChanged(ref this.rootFolder, value);
                    if (value != null) {
                        value.Explorer = this;
                    }
                }
            }
        }
    }
}