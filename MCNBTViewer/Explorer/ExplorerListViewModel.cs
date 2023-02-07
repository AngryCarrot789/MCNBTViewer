using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.Explorer {
    public class ExplorerListViewModel : BaseViewModel {
        public ExplorerViewModel Explorer { get; }

        private FolderItemViewModel currentFolder;

        public FolderItemViewModel CurrentFolder {
            get => this.currentFolder;
            set => this.RaisePropertyChanged(ref this.currentFolder, value);
        }

        private FileItemViewModel selectedFile;
        public FileItemViewModel SelectedFile {
            get => this.selectedFile;
            set => this.RaisePropertyChanged(ref this.selectedFile, value);
        }

        public ExplorerListViewModel(ExplorerViewModel explorer) {
            this.Explorer = explorer;
        }
    }
}