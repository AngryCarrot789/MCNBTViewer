using System;
using MCNBTViewer.Explorer;
using MCNBTViewer.NBT.Explorer.Items;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.NBT.Explorer {
    public class NBTExplorerViewModel : BaseViewModel {
        private NBTCompoundViewModel root;
        private bool isUpdatingSelection;

        public NBTExplorerListViewModel ExplorerList { get; }

        public NBTCompoundViewModel Root {
            get => this.root;
            set => this.RaisePropertyChanged(ref this.root, value);
        }

        public ITreeView TreeView { get; set; }

        private string selectedPath;
        public string SelectedPath {
            get => this.selectedPath;
            set => RaisePropertyChanged(ref this.selectedPath, value);
        }

        public NBTExplorerViewModel() {
            this.ExplorerList = new NBTExplorerListViewModel(this);
        }

        public void OnTreeSelectItem(BaseNBTViewModel item) {
            if (this.isUpdatingSelection) {
                return;
            }

            this.isUpdatingSelection = true;
            try {
                if (this.ExplorerList.CurrentFolder == null || this.ExplorerList.CurrentFolder != item.Parent) {
                    this.ExplorerList.CurrentFolder = item.Parent;
                }

                this.ExplorerList.SelectedFile = item;
            }
            finally {
                this.isUpdatingSelection = false;
            }
        }

        public void UseItem(BaseNBTViewModel file) {

        }
    }
}