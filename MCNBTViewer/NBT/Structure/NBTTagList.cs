using System;
using System.Collections.Generic;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagList : NBTBase {
        public readonly List<NBTBase> tagList;
        public byte field_74746_b;

        public NBTTagList(String var1) : base(var1) {
            this.tagList = new List<NBTBase>();
        }

        public override void Write(DataOutputStream output) {
            this.field_74746_b = this.tagList.Count > 0 ? this.tagList[0].GetId() : (byte) 1;
            output.WriteByte(this.field_74746_b);
            output.WriteInt(this.tagList.Count);
            foreach (NBTBase t in this.tagList) {
                t.Write(output);
            }
        }

        public override void Read(DataInputStream input, int deep) {
            if (deep <= 512) {
                this.field_74746_b = input.ReadByte();
                int count = input.ReadInt();
                this.tagList.Clear();
                for (int k = 0; k < count; ++k) {
                    NBTBase nbtbase = CreateTag(this.field_74746_b, null);
                    nbtbase.Read(input, deep + 1);
                    this.tagList.Add(nbtbase);
                }
            }
            else {
                throw new Exception("Tried to read NBT tag with too high complexity, depth > 512");
            }
        }

        public override byte GetId() {
            return 9;
        }

        public override NBTBase Copy() {
            NBTTagList copy = new NBTTagList(this.GetName()) {field_74746_b = this.field_74746_b};
            foreach (NBTBase nbtBase in this.tagList) {
                copy.tagList.Add(nbtBase.Copy());
            }

            return copy;
        }

        public override bool Equals(object var1) {
            if (base.Equals(var1) && var1 is NBTTagList list) {
                if (this.field_74746_b == list.field_74746_b) {
                    return this.tagList.Equals(list.tagList);
                }
            }

            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.tagList.GetHashCode();
        }
    }
}