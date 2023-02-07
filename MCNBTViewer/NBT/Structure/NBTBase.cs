using System;
using REghZy.Streams;

namespace MCNBTViewer.NBT.Structure {
    public abstract class NBTBase {
        public static readonly string[] NBT_TYPES = new string[] {"END", "BYTE", "SHORT", "INT", "LONG", "FLOAT", "DOUBLE", "BYTE[]", "STRING", "LIST", "COMPOUND", "INT[]"};

        public String tagName;

        public NBTBase() {
        }

        public NBTBase(string name) {
            this.tagName = name ?? "";
        }

        public abstract void Write(DataOutputStream output);

        public abstract void Read(DataInputStream input, int deep);

        public abstract byte GetId();

        public NBTBase setName(String var1) {
            this.tagName = var1 ?? "";
            return this;
        }

        public String GetName() {
            return this.tagName ?? "";
        }

        public static NBTBase ReadNamedTag(DataInputStream input) {
            return ReadNamedTag(input, 0);
        }

        public static NBTBase ReadNamedTag(DataInputStream input, int deep) {
            byte id = input.ReadByte();
            if (id == 0) {
                return new NBTTagEnd();
            }
            else {
                String name = input.ReadStringUTF8Labelled();
                NBTBase tag = CreateTag(id, name);
                tag.Read(input, deep);
                return tag;
            }
        }

        public static void WriteNamedTag(NBTBase tag, DataOutputStream output) {
            output.WriteByte(tag.GetId());
            if (tag.GetId() != 0) {
                output.WriteStringLabelledUTF8(tag.GetName());
                tag.Write(output);
            }
        }

        public static NBTBase CreateTag(byte id, String name) {
            switch (id) {
                case 0: return new NBTTagEnd();
                case 1: return new NBTTagByte(name);
                case 2: return new NBTTagShort(name);
                case 3: return new NBTTagInt(name);
                case 4: return new NBTTagLong(name);
                case 5: return new NBTTagFloat(name);
                case 6: return new NBTTagDouble(name);
                case 7: return new NBTTagByteArray(name);
                case 8: return new NBTTagString(name);
                case 9: return new NBTTagList(name);
                case 10: return new NBTTagCompound(name);
                case 11: return new NBTTagIntArray(name);
                default: return null;
            }
        }

        public static String GetTagName(byte tagId) {
            switch (tagId) {
                case 0: return "TAG_End";
                case 1: return "TAG_Byte";
                case 2: return "TAG_Short";
                case 3: return "TAG_Int";
                case 4: return "TAG_Long";
                case 5: return "TAG_Float";
                case 6: return "TAG_Double";
                case 7: return "TAG_Byte_Array";
                case 8: return "TAG_String";
                case 9: return "TAG_List";
                case 10: return "TAG_Compound";
                case 11: return "TAG_Int_Array";
                default: return "UNKNOWN";
            }
        }

        public abstract NBTBase Copy();

        public override bool Equals(object var1) {
            if (var1 is NBTBase nbt) {
                if (this.GetId() != nbt.GetId()) {
                    return false;
                }
                else if (this.tagName == null && nbt.tagName != null || this.tagName != null && nbt.tagName == null) {
                    return false;
                }
                else {
                    return this.tagName == null || this.tagName.Equals(nbt.tagName);
                }
            }
            else {
                return false;
            }
        }

        public override int GetHashCode() {
            return this.tagName.GetHashCode() ^ this.GetId();
        }
    }
}