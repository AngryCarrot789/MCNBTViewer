using System.Collections.Generic;
using System.Collections.ObjectModel;
using MCNBTViewer.Core.Regions;
using MCNBTViewer.Core.Utils;

namespace MCNBTViewer.Core.Explorer.Items.Regions {
    public class RegionItemViewModel : BaseViewModel {
        protected readonly EfficientObservableCollection<RegionChunkViewModel> children;
        public ReadOnlyObservableCollection<RegionChunkViewModel> Children { get; }

        private string filePath;
        public string FilePath {
            get => this.filePath;
            set => this.RaisePropertyChanged(ref this.filePath, value);
        }

        public RegionItemViewModel() {

        }

        public void ClearAndLoadChunks(RegionFile file) {
            this.FilePath = file.FilePath;
            List<RegionChunkViewModel> chunks = new List<RegionChunkViewModel>();
            for (int x = 0; x < 32; x++) {
                for (int z = 0; z < 32; z++) {
                    if (file.HasChunkEntry(x, z)) {
                        RegionChunkViewModel chunk = new RegionChunkViewModel(this, x, z);
                        chunk.LoadData(file);
                        chunks.Add(chunk);
                    }
                }
            }

            this.children.ClearAndAddRange(chunks);
        }
    }
}