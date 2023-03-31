using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Core.Explorer {
    public class NBTExplorerListViewModel : BaseViewModel {
        public NBTExplorerViewModel Explorer { get; }

        private BaseNBTCollectionViewModel currentFolder;
        public BaseNBTCollectionViewModel CurrentFolder {
            get => this.currentFolder;
            set => this.RaisePropertyChanged(ref this.currentFolder, value);
        }

        private BaseNBTViewModel selectedItem;
        public BaseNBTViewModel SelectedItem {
            get => this.selectedItem;
            set => this.RaisePropertyChanged(ref this.selectedItem, value);
        }

        public NBTExplorerListViewModel(NBTExplorerViewModel explorer) {
            this.Explorer = explorer;
        }
    }
}
