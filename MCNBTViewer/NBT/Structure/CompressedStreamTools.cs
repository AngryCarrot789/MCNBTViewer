using System.IO;
using System.IO.Compression;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public static class CompressedStreamTools {
        public static NBTTagCompound ReadCompressed(string filePath) {
            using (FileStream stream = File.OpenRead(filePath)) {
                return ReadCompressed(stream);
            }
        }

        public static NBTTagCompound ReadCompressed(Stream stream) {
            using (BufferedStream buffered = new BufferedStream(new GZipStream(stream, CompressionMode.Decompress))) {
                return ReadTagCompound(new DataInputStream(buffered));
            }
        }

        public static NBTTagCompound ReadTagCompound(DataInputStream input) {
            NBTBase nbtbase = NBTBase.ReadNamedTag(input);
            if (nbtbase is NBTTagCompound compound) {
                return compound;
            } else {
                throw new IOException("Root tag must be a named compound tag");
            }
        }
    }
}