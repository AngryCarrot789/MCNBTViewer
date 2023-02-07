using System;
using System.Collections.Generic;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagCompound : NBTBase {
        public readonly Dictionary<string, NBTBase> tagMap;

        public NBTTagCompound(String var1) : base(var1) {
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
                while ((nbt = ReadNamedTag(input, deep + 1)).GetId() != 0) {
                    this.tagMap[nbt.GetName()] = nbt;
                }
            }
            else {
                throw new Exception("Tried to read NBT tag with too high complexity, depth > 512");
            }
        }

        public override byte GetId() {
            return 10;
        }

        public override NBTBase Copy() {
            NBTTagCompound nbt = new NBTTagCompound(this.GetName());
            foreach (KeyValuePair<string,NBTBase> pair in this.tagMap) {
                nbt.tagMap[pair.Key] = pair.Value.Copy();
            }

            return nbt;
        }
    }
}