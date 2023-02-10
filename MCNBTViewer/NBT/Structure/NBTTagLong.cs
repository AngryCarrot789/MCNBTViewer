using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagLong : NBTBase {
        public override byte Id => 4;

        public long data;

        public NBTTagLong() {
        }

        public NBTTagLong(long data) {
            this.data = data;
        }

        public override void Write(DataOutputStream output) {
            output.WriteLong(this.data);
        }

        public override void Read(DataInputStream input, int deep) {
            this.data = input.ReadLong();
        }


        public override string ToString() {
            return this.data.ToString();
        }

        public override NBTBase CloneTag() {
            return new NBTTagLong(this.data);
        }

        public override bool Equals(object obj) {
            return base.Equals(obj) && obj is NBTTagLong tag && this.data == tag.data;
        }

        public override int GetHashCode() {
            ulong x = (ulong) this.data;
            return base.GetHashCode() ^ (int) (x ^ x >> 32);
        }
    }
}