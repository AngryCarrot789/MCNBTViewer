using System;
using MCNBTViewer.NBT.Explorer;
using MCNBTViewer.NBT.Explorer.Items;

namespace MCNBTViewer.NBT.Structure {
    public enum NBTType {
        End       = 0,
        Byte      = 1,
        Short     = 2,
        Int       = 3,
        Long      = 4,
        Float     = 5,
        Double    = 6,
        String    = 8,
        ByteArray = 7,
        IntArray  = 11,
        List      = 9,
        Compound  = 10
    }

    public static class NBTypeExtensions {
        public static NBTBase ToBaseNBT(this NBTType type, string name) {
            switch (type) {
                case NBTType.End: return new NBTTagEnd();
                case NBTType.Byte: return new NBTTagByte(name);
                case NBTType.Short: return new NBTTagShort(name);
                case NBTType.Int: return new NBTTagInt(name);
                case NBTType.Long: return new NBTTagLong(name);
                case NBTType.Float: return new NBTTagFloat(name);
                case NBTType.Double: return new NBTTagDouble(name);
                case NBTType.String: return new NBTTagString(name);
                case NBTType.ByteArray: return new NBTTagByteArray(name);
                case NBTType.IntArray: return new NBTTagIntArray(name);
                case NBTType.List: return new NBTTagList(name);
                case NBTType.Compound: return new NBTTagCompound(name);
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static BaseNBTViewModel ToViewModel(this NBTType type, string name) {
            switch (type) {
                case NBTType.End:
                case NBTType.Byte:
                case NBTType.Short:
                case NBTType.Int:
                case NBTType.Long:
                case NBTType.Float:
                case NBTType.Double:
                case NBTType.String:
                    return new NBTPrimitiveViewModel(type) {Name = name};
                case NBTType.ByteArray: return new NBTByteArrayViewModel() {Name = name};
                case NBTType.IntArray: return new NBTIntArrayViewModel() {Name = name};
                case NBTType.List: return new NBTListViewModel() {Name = name};
                case NBTType.Compound: return new NBTCompoundViewModel() {Name = name};
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}