using System.IO;
using System.Threading;
using REghZy.Streams;

namespace MCNBTViewer.Core.NBT {
    public abstract class NBTBase {
        public static bool IgnoreStackDepth = false;

        public abstract byte Id { get; }

        public NBTType Type => (NBTType) this.Id;

        protected NBTBase() {

        }

        /// <summary>
        /// Read the data for this tag
        /// </summary>
        /// <param name="input"></param>
        /// <param name="deep">How deep the read procedure is currently</param>
        public abstract void Read(IDataInput input, int deep);

        /// <summary>
        /// Write this tag's data to the output
        /// </summary>
        public abstract void Write(IDataOutput output);

        /// <summary>
        /// Creates a deep copy of this tag
        /// </summary>
        public abstract NBTBase CloneTag();

        public static bool ReadTag(IDataInput input, int deep, out string name, out NBTBase nbt) {
            byte id = input.ReadByte();
            if (id == 0) {
                name = null;
                nbt = null;
                return false;
            }
            else {
                name = input.ReadStringUTF8Labelled();
                if (string.IsNullOrEmpty(name)) {
                    name = null;
                }

                if ((nbt = CreateTag(id)) == null) {
                    return false;
                }

                nbt.Read(input, deep);
                return true;
            }
        }

        public static void WriteTag(IDataOutput output, string name, NBTBase tag) {
            output.WriteByte(tag.Id);
            if (tag.Id != 0) {
                output.WriteStringLabelledUTF8(name ?? "");
                tag.Write(output);
            }
        }

        public static NBTBase CreateTag(byte id) {
            switch (id) {
                case 0: return new NBTTagEnd();
                case 1: return new NBTTagByte();
                case 2: return new NBTTagShort();
                case 3: return new NBTTagInt();
                case 4: return new NBTTagLong();
                case 5: return new NBTTagFloat();
                case 6: return new NBTTagDouble();
                case 7: return new NBTTagByteArray();
                case 8: return new NBTTagString();
                case 9: return new NBTTagList();
                case 10: return new NBTTagCompound();
                case 11: return new NBTTagIntArray();
                case 12: return new NBTTagLongArray();
                default: return null;
            }
        }

        public override bool Equals(object obj) {
            return obj is NBTBase nbt && this.Id == nbt.Id;
        }

        public override int GetHashCode() {
            return this.Id.ToString().GetHashCode();
        }

        protected static NBTBase Clone(NBTBase nbt) {
            return nbt != null ? nbt.CloneTag() : null;
        }
    }
}