using System.IO;

namespace MCNBTViewer.Core.Regions {
    public class ChunkBuffer : MemoryStream {
        private readonly RegionFile region;

        public int X { get; }
        public int Z { get; }

        public int timestamp;

        public ChunkBuffer(RegionFile region, int x, int z) : base(8096) {
            this.region = region;
            this.X = x;
            this.Z = z;
        }

        public ChunkBuffer(RegionFile region, int x, int z, int timestamp) : this(region, x, z) {
            this.timestamp = timestamp;
        }

        public override void Close() {
            if (this.timestamp >= 0) {
                this.region.Write(this.X, this.Z, this.GetBuffer(), (int) this.Length, this.timestamp);
            }
            else {
                this.region.Write(this.X, this.Z, this.GetBuffer(), (int) this.Length);
            }
        }
    }
}