using System.IO;
using MCNBTViewer.Core.NBT;
using MCNBTViewer.Core.Regions;

namespace MCNBTViewer.Core.Explorer.Items.Regions {
    public class RegionChunkViewModel : BaseViewModel {
        public RegionItemViewModel Region { get; }

        public int X { get; }

        public int Z { get; }

        public NBTCompoundViewModel ChunkData { get; private set; }

        public RegionChunkViewModel(RegionItemViewModel region, int x, int z) {
            this.Region = region;
            this.X = x;
            this.Z = z;
        }

        public void LoadData(RegionFile file) {
            using (Stream stream = file.GetChunkReadStreamCompressed(this.X, this.Z)) {
                NBTTagCompound nbt = CompressedStreamTools.Read(stream, out string name, false, IoC.IsBigEndian);
                this.ChunkData = BaseNBTViewModel.CreateFrom(name, nbt);
            }
        }
    }
}