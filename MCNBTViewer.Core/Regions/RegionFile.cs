using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using REghZy.Streams;

namespace MCNBTViewer.Core.Regions {
    public class RegionFile : IDisposable {
        private static readonly byte[] EMPTY_SECTOR = new byte[4096];

        private FileStream file;
        private IDataInput dataInput;
        private IDataOutput dataOutput;
        private int sizeDelta;
        private List<bool> sectorFree;
        private readonly int[] offsets;
        private readonly int[] timestamps; // chunk timestamps

        public string FilePath { get; }

        public DateTime LastModifiedTime { get; private set; }

        public RegionFile(string filePath) {
            this.FilePath = filePath;
            this.offsets = new int[1024];
            this.timestamps = new int[1024];
        }

        public void ReadFile() {
            try {
                this.LastModifiedTime = File.GetLastWriteTime(this.FilePath);
            }
            catch {
                this.LastModifiedTime = default;
            }

            this.OpenFileReadWrite();
            if (this.file.Length < 4096) {
                this.file.Write(EMPTY_SECTOR, 0, 4096);
                this.file.Write(EMPTY_SECTOR, 0, 4096);
                this.file.Flush();
                this.sizeDelta += 8192;
            }

            if ((this.file.Length % 4095) != 0L) {
                this.file.Seek(0, SeekOrigin.End);
                for (int i = 0; i < (this.file.Length & 4095); i++)
                    this.dataOutput.WriteByte(0);
                this.file.Flush();
            }

            int capacity = (int) (this.file.Length / 4096L);
            this.sectorFree = new List<bool>(capacity);
            for (int i = 0; i < capacity; i++) {
                this.sectorFree.Add(true);
            }

            this.sectorFree[0] = false;
            this.sectorFree[1] = false;
            this.file.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < 1024; i++) {
                int x = this.dataInput.ReadInt();
                this.offsets[i] = x;
                if (x != 0 && (x >> 8) + (x & 255) <= this.sectorFree.Count) {
                    for (int j = 0; j < (x & 255); ++j) {
                        this.sectorFree[(x >> 8) + j] = false;
                    }
                }
            }

            for (int i = 0; i < 1024; ++i) {
                int x = this.dataInput.ReadInt();
                this.timestamps[i] = x;
            }
        }

        public bool ChunkExists(int x, int z) {
            if (IsChunkOutOfBounds(x, z)) {
                return false;
            }

            int offset = this.GetOffset(x, z);
            if (offset == 0) {
                return false;
            }

            int a = offset >> 8;
            int b = offset & 255;
            if (a + b > this.sectorFree.Count) {
                return false;
            }
            else {
                this.file.Seek(a * 4096, SeekOrigin.Begin);
                int c = this.dataInput.ReadInt();
                if (c <= 4096 * b && c > 0) {
                    byte d = this.dataInput.ReadByte();
                    return d == 1 || d == 2;
                }
                else {
                    return false;
                }
            }
        }

        public bool HasChunkEntry(int x, int z) {
            return this.GetOffset(x, z) != 0;
        }

        public Stream GetChunkReadStreamCompressed(int x, int z) {
            if (IsChunkOutOfBounds(x, z)) {
                return null;
            }

            int offset = this.GetOffset(x, z);
            if (offset == 0) {
                return null;
            }

            int a = offset >> 8;
            int b = offset & 255;
            if (a + b > this.sectorFree.Count) {
                return null;
            }

            this.file.Seek(a * 4096, SeekOrigin.Begin);
            int c = this.dataInput.ReadInt();
            if (c > (4096 * b) || c <= 0) {
                return null;
            }

            byte d = this.dataInput.ReadByte();
            if (d != 1 && d != 2) {
                return null;
            }

            byte[] buffer = new byte[c - 1];
            this.file.Read(buffer, 0, buffer.Length);
            if (d == 1) {
                return new GZipStream(new MemoryStream(buffer), CompressionMode.Decompress);
            }
            else {
                return new DeflateStream(new MemoryStream(buffer), CompressionMode.Decompress);
            }
        }

        public void Write(int x, int z, byte[] data, int length) {
            this.Write(x, z, data, length, UnixTime.GetUnixTime());
        }

        public void Write(int x, int z, byte[] data, int length, int timestamp) {
            int offset = this.GetOffset(x, z);
            int a = offset >> 8;
            int b = offset & byte.MaxValue;
            int c = (length + 5) / 4096 + 1;
            if (c >= 256)
                return;
            if (a != 0 && b == c) {
                this.Write(a, data, length);
            }
            else {
                for (int i = 0; i < b; ++i) {
                    this.sectorFree[a + i] = true;
                }

                int d = this.sectorFree.IndexOf(true);
                int e = 0;
                if (d != -1) {
                    for (int index = d; index < this.sectorFree.Count; ++index) {
                        if (e != 0) {
                            if (this.sectorFree[index]) {
                                e++;
                            }
                            else {
                                e = 0;
                            }
                        }
                        else if (this.sectorFree[index]) {
                            d = index;
                            e = 1;
                        }

                        if (e >= c) {
                            break;
                        }
                    }
                }

                if (e >= c) {
                    int sectorNumber2 = d;
                    this.SetOffset(x, z, sectorNumber2 << 8 | c);
                    for (int index = 0; index < c; ++index)
                        this.sectorFree[sectorNumber2 + index] = false;
                    this.Write(sectorNumber2, data, length);
                }
                else {
                    this.file.Seek(0L, SeekOrigin.End);
                    int count = this.sectorFree.Count;
                    for (int index = 0; index < c; ++index) {
                        this.file.Write(EMPTY_SECTOR, 0, EMPTY_SECTOR.Length);
                        this.sectorFree.Add(false);
                    }

                    this.sizeDelta += 4096 * c;
                    this.Write(count, data, length);
                    this.SetOffset(x, z, count << 8 | c);
                }
            }

            this.SetTimestamp(x, z, timestamp);
        }

        private void Write(int sectorNumber, byte[] data, int length) {
            this.file.Seek(sectorNumber * 4096, SeekOrigin.Begin);
            this.dataOutput.WriteInt(length + 1);
            this.file.WriteByte(2);
            this.file.Write(data, 0, length);
        }

        public void DeleteChunk(int x, int z) {
            int offset = this.GetOffset(x, z);
            int a = offset >> 8;
            int b = offset & byte.MaxValue;
            this.file.Seek(a * 4096, SeekOrigin.Begin);
            for (int i = 0; i < b; ++i)
                this.file.Write(EMPTY_SECTOR, 0, 4096);
            this.SetOffset(x, z, 0);
            this.SetTimestamp(x, z, 0);
        }

        public int GetTimestamp(int x, int z) => this.timestamps[x + z * 32];

        public void SetTimestamp(int x, int z, int value) {
            this.timestamps[x + z * 32] = value;
            this.file.Seek(4096 + (x + z * 32) * 4, SeekOrigin.Begin);
            this.dataOutput.WriteInt(value);
        }

        public int GetOffset(int x, int z) {
            return this.offsets[x + (z * 32)];
        }

        private void SetOffset(int x, int z, int offset) {
            this.offsets[x + z * 32] = offset;
            this.file.Seek((x + z * 32) * 4, SeekOrigin.Begin);
            this.dataOutput.WriteInt(offset);
        }

        public static bool IsChunkOutOfBounds(int x, int z) {
            return x < 0 || x >= 32 || z < 0 || z >= 32;
        }

        private void OpenFileReadWrite() {
            if (this.file != null) {
                throw new Exception("File stream already open");
            }

            this.file = File.Open(this.FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (IoC.IsBigEndian) {
                this.dataInput = new DataInputStream(this.file);
                this.dataOutput = new DataOutputStream(this.file);
            }
            else {
                this.dataInput = new DataInputStreamLE(this.file);
                this.dataOutput = new DataOutputStreamLE(this.file);
            }
        }

        public void Dispose() {
            if (this.file != null) {
                this.file.Close();
                this.file = null;
                this.dataInput = null;
                this.dataOutput = null;
            }
        }
    }
}