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
            using (BufferedStream buffered = new BufferedStream(new GZipStream(stream, CompressionMode.Decompress, true))) {
                if (NBTBase.ReadTag(CreateInput(buffered), 0, out tagName, out NBTBase nbt)) {
                    if (nbt is NBTTagCompound compound) {
                        return compound;
                    }
                    else {
                        throw new Exception("Expected to read NBTTagCompound. Got " + nbt.Type + " instead");
                    }
                }
                else {
                    throw new Exception("Failed to read NBTTagCompound from stream");
                }
            }
        }

        public static void WriteCompressed(NBTBase nbt, string filePath) {
            using (FileStream stream = File.OpenWrite(filePath)) {
                WriteCompressed(nbt, stream);
            }
        }

        public static void WriteCompressed(NBTBase nbt, Stream stream) {
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress, true)) {
                NBTBase.WriteTag(CreateOutput(gzip), null, nbt);
            }
        }

        public static IDataInput CreateInput(Stream stream) {
            IDataInput output;
            if (IoC.IsBigEndian) {
                output = new DataInputStream(stream);
            }
            else {
                output = new DataInputStreamLE(stream);
            }
            return output;
        }

        public static IDataOutput CreateOutput(Stream stream) {
            IDataOutput output;
            if (IoC.IsBigEndian) {
                output = new DataOutputStream(stream);
            }
            else {
                output = new DataOutputStreamLE(stream);
            }
            return output;
        }
    }
}