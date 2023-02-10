using System;
using System.Collections.Generic;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagCompound : NBTBase {
        public readonly Dictionary<string, NBTBase> tagMap;

        public NBTTagCompound() {
            this.tagMap = new Dictionary<string, NBTBase>();
        }

        public NBTTagCompound(string name) : base(name) {
            this.tagMap = new Dictionary<string, NBTBase>();
        }

        public override void Write(DataOutputStream output) {
            foreach (NBTBase nbt in this.tagMap.Values) {
                WriteNamedTag(nbt, output);
            }

            output.WriteByte(0);
        }

        public override void Read(DataInputStream input, int deep) {
            if (deep <= 512) {
                this.tagMap.Clear();
                NBTBase nbt;
                while ((nbt = ReadNamedTag(input, deep + 1)).Id != 0) {
                    this.tagMap[nbt.Name] = nbt;
                }
            }
            else {
                throw new Exception("Tried to read NBT tag with too high complexity, depth > 512");
            }
        }

        public void Put(string key, NBTBase nbt) {
            nbt.Name = key;
            this.tagMap[key] = nbt;
        }

        public override byte Id => 10;

        public override NBTBase CloneTag() {
            NBTTagCompound nbt = new NBTTagCompound(this.Name);
            foreach (KeyValuePair<string, NBTBase> pair in this.tagMap) {
                nbt.tagMap[pair.Key] = pair.Value.CloneTag();
            }

            return nbt;
        }
    }
}