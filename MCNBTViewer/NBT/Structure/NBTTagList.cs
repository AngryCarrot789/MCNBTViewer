using System;
using System.Collections.Generic;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public class NBTTagList : NBTBase {
        public readonly List<NBTBase> list;
        public byte heldItemType;

        public NBTTagList() {
            this.list = new List<NBTBase>();
        }

        public NBTTagList(string name) : base(name) {
            this.list = new List<NBTBase>();
        }

        public override void Write(DataOutputStream output) {
            this.heldItemType = this.list.Count > 0 ? this.list[0].Id : (byte) 1;
            output.WriteByte(this.heldItemType);
            output.WriteInt(this.list.Count);
            foreach (NBTBase t in this.list) {
                t.Write(output);
            }
        }

        public override void Read(DataInputStream input, int deep) {
            if (deep <= 512) {
                this.heldItemType = input.ReadByte();
                int count = input.ReadInt();
                this.list.Clear();
                for (int k = 0; k < count; ++k) {
                    NBTBase nbtbase = CreateTag(this.heldItemType, null);
                    nbtbase.Read(input, deep + 1);
                    this.list.Add(nbtbase);
                }
            }
            else {
                throw new Exception("Tried to read NBT tag with too high complexity, depth > 512");
            }
        }

        public override byte Id => 9;

        public override NBTBase CloneTag() {
            NBTTagList copy = new NBTTagList(this.Name) {heldItemType = this.heldItemType};
            foreach (NBTBase nbtBase in this.list) {
                copy.list.Add(nbtBase.CloneTag());
            }

            return copy;
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj) && obj is NBTTagList list) {
                if (this.heldItemType == list.heldItemType) {
                    return this.list.Equals(list.list);
                }
            }

            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode() ^ this.list.GetHashCode();
        }
    }
}