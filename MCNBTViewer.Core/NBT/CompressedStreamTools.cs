using System;
using System.IO;
using System.IO.Compression;
using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public static class CompressedStreamTools {
        public static NBTTagCompound ReadCompressed(string filePath, out string tagName) {
            using (FileStream stream = File.OpenRead(filePath)) {
                return ReadCompressed(stream, out tagName);
            }
        }

        public static NBTTagCompound ReadCompressed(Stream stream, out string tagName) {
            using (BufferedStream buffered = new BufferedStream(new GZipStream(stream, CompressionMode.Decompress))) {
                if (NBTBase.ReadTag(new DataInputStream(buffered), 0, out tagName, out NBTBase nbt)) {
                    if (nbt is NBTTagCompound compound) {
                        return compound;
                    }
                    else {
                        throw new Exception("Expected to read NBTTagCompound. Got " + nbt.Type + " instead");
                    }
                }
                else {
                    throw new Exception("Failed to read NBT from stream");
                }
            }
        }
    }
}