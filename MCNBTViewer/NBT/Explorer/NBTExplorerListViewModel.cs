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

        private BaseNBTCollectionViewModel currentFolder;
        public BaseNBTCollectionViewModel CurrentFolder {
            get => this.currentFolder;
            set => this.RaisePropertyChanged(ref this.currentFolder, value);
        }

        private BaseNBTViewModel selectedFile;
        public BaseNBTViewModel SelectedFile {
            get => this.selectedFile;
            set => this.RaisePropertyChanged(ref this.selectedFile, value);
        }

        public NBTExplorerListViewModel(NBTExplorerViewModel explorer) {
            this.Explorer = explorer;
        }
        // NBTCollectiveItemViewModel

    }
}
