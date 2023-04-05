using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Dialogs {
    public interface INBTDialogService {
        (string, NBTTagByte)? CreateTagByte(bool canEditName = true, string defaultName = null, NBTTagByte defaultTag = null);
        (string, NBTTagShort)? CreateTagShort(bool canEditName = true, string defaultName = null, NBTTagShort defaultTag = null);
        (string, NBTTagInt)? CreateTagInt(bool canEditName = true, string defaultName = null, NBTTagInt defaultTag = null);
        (string, NBTTagLong)? CreateTagLong(bool canEditName = true, string defaultName = null, NBTTagLong defaultTag = null);
        (string, NBTTagFloat)? CreateTagFloat(bool canEditName = true, string defaultName = null, NBTTagFloat defaultTag = null);
        (string, NBTTagDouble)? CreateTagDouble(bool canEditName = true, string defaultName = null, NBTTagDouble defaultTag = null);
        (string, NBTTagString)? CreateTagString(bool canEditName = true, string defaultName = null, NBTTagString defaultTag = null);
        (string, NBTTagByteArray)? CreateTagByteArray(bool canEditName = true, string defaultName = null, NBTTagByteArray defaultTag = null);
        (string, NBTTagIntArray)? CreateTagIntArray(bool canEditName = true, string defaultName = null, NBTTagIntArray defaultTag = null);
        (string, NBTTagLongArray)? CreateTagLongArray(bool canEditName = true, string defaultName = null, NBTTagLongArray defaultTag = null);
        (string, NBTTagList)? CreateTagList(bool canEditName = true, string defaultName = null, NBTTagList defaultTag = null);
        (string, NBTTagCompound)? CreateTagCompound(bool canEditName = true, string defaultName = null, NBTTagCompound defaultTag = null);
    }
}