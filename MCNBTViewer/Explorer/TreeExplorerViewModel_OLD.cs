using System.Collections.ObjectModel;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.Explorer {
    public class TreeExplorerViewModel_OLD : BaseViewModel {
        private bool isUpdatingChildren;
        private bool isUpdatingSelection;

        /// <summary>
        /// The children of this explorer
        /// </summary>
        public ObservableCollection<FileItemViewModel> RootFiles { get; }

        /// <summary>
        /// A list of visible files, used for the explorer list
        /// </summary>
        public ObservableCollection<FileItemViewModel> VisibleChildren { get; }

        private FileItemViewModel selectedTreeFile;
        public FileItemViewModel SelectedTreeFile {
            get => this.selectedTreeFile;
            set => this.RaisePropertyChanged(ref this.selectedTreeFile, value);
        }

        public TreeExplorerViewModel_OLD() {
            this.RootFiles = new ObservableCollection<FileItemViewModel>();
            this.VisibleChildren = new ObservableCollection<FileItemViewModel>();
        }

        public void SelectFileFromTree(FileItemViewModel file) {
            if (this.isUpdatingSelection) {
                return;
            }

            try {
                this.isUpdatingSelection = true;
                this.SelectedTreeFile = file;
                this.UpdateChildren();
            }
            finally {
                this.isUpdatingSelection = false;
            }
        }

        private void UpdateChildren() {
            if (this.isUpdatingChildren)
                return;

            this.isUpdatingChildren = true;
            if (this.SelectedTreeFile == null) {
                this.VisibleChildren.Clear();
                foreach (FileItemViewModel file in this.RootFiles) {
                    this.VisibleChildren.Add(file);
                }
            }
            else if (this.SelectedTreeFile is FolderItemViewModel folder) {
                this.VisibleChildren.Clear();
                foreach (FileItemViewModel file in folder.Children) {
                    this.VisibleChildren.Add(file);
                }
            }

            this.isUpdatingChildren = false;
        }
    }
}
