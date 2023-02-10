using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCNBTViewer.Explorer;
using MCNBTViewer.NBT.Explorer.Items;
using REghZy.MVVM.ViewModels;

namespace MCNBTViewer.NBT.Explorer {
    public class NBTExplorerListViewModel : BaseViewModel {
        public NBTExplorerViewModel Explorer { get; }

        private NBTCollectiveItemViewModelBase currentFolder;
        public NBTCollectiveItemViewModelBase CurrentFolder {
            get => this.currentFolder;
            set => this.RaisePropertyChanged(ref this.currentFolder, value);
        }

        private NBTItemViewModelBase selectedFile;
        public NBTItemViewModelBase SelectedFile {
            get => this.selectedFile;
            set => this.RaisePropertyChanged(ref this.selectedFile, value);
        }

        public NBTExplorerListViewModel(NBTExplorerViewModel explorer) {
            this.Explorer = explorer;
        }
        // NBTCollectiveItemViewModel

    }
}
