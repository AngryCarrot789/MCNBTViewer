using System.Collections.ObjectModel;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.Explorer {
    public class ExplorerViewModel : BaseViewModel {
        private bool isUpdatingChildren;
        private bool isUpdatingSelection;

        private FolderItemViewModel root;
        public FolderItemViewModel Root {
            get => this.root;
            set => this.RaisePropertyChanged(ref this.root, value);
        }

        public ExplorerListViewModel ExplorerList { get; }

        public ITreeView TreeView { get; set; }

        public ExplorerViewModel() {
            this.ExplorerList = new ExplorerListViewModel(this);
            this.Root = new FolderItemViewModel();
        }

        /// <summary>
        /// Called when the tree's selected item changes
        /// </summary>
        /// <param name="selectedFile">The newly selected file</param>
        public void SelectFileFromTree(FileItemViewModel selectedFile) {
            if (this.isUpdatingSelection) {
                return;
            }

            try {
                this.isUpdatingSelection = true;
                if (this.isUpdatingChildren)
                    return;

                this.isUpdatingChildren = true;
                // if (selectedFile is FolderItemViewModel folder) {
                //     this.ExplorerList.CurrentFolder = folder;
                //     this.ExplorerList.SelectedFile = null;
                // }
                // else {
                //     if (this.ExplorerList.CurrentFolder == null || this.ExplorerList.CurrentFolder != selectedFile.Parent) {
                //         this.ExplorerList.CurrentFolder = selectedFile.Parent;
                //     }
                //     this.ExplorerList.SelectedFile = selectedFile;
                //     // update UI, e.g data preview
                // }

                if (this.ExplorerList.CurrentFolder == null || this.ExplorerList.CurrentFolder != selectedFile.Parent) {
                    this.ExplorerList.CurrentFolder = selectedFile.Parent;
                }

                this.ExplorerList.SelectedFile = selectedFile;
                this.isUpdatingChildren = false;
            }
            finally {
                this.isUpdatingSelection = false;
            }
        }

        /// <summary>
        /// Called when the list calls for the "use" action against an item (aka navigate by double click or pressing enter)
        /// </summary>
        /// <param name="file">The file (non-null)</param>
        public void NavigateListFileItem(FileItemViewModel file) {
            if (this.isUpdatingSelection) {
                return;
            }

            try {
                this.isUpdatingSelection = true;
                if (this.isUpdatingChildren)
                    return;

                this.isUpdatingChildren = true;
                if (file is FolderItemViewModel folder) {
                    this.ExplorerList.CurrentFolder = folder;
                    this.TreeView.SetSelectedFile(folder);
                }
                else {
                    file.OnUserAction();
                }

                this.isUpdatingChildren = false;
            }
            finally {
                this.isUpdatingSelection = false;
            }

            // if (this.isUpdatingSelection) {
            //     return;
            // }
            // this.isUpdatingSelection = true;
            // this.TreeView?.SetSelectedFile(file);
            // this.isUpdatingSelection = false;
        }
    }
}
