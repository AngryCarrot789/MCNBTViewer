using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Dialogs {
    public interface INewTagDialogService {
        (string, NBTTagByte)? CreateTagByte(bool canEditName = true);
        (string, NBTTagShort)? CreateTagShort(bool canEditName = true);
        (string, NBTTagInt)? CreateTagInt(bool canEditName = true);
        (string, NBTTagLong)? CreateTagLong(bool canEditName = true);
        (string, NBTTagFloat)? CreateTagFloat(bool canEditName = true);
        (string, NBTTagDouble)? CreateTagDouble(bool canEditName = true);
        (string, NBTTagByteArray)? CreateTagByteArray(bool canEditName = true);
        (string, NBTTagString)? CreateTagString(bool canEditName = true);
        (string, NBTTagList)? CreateTagList(bool canEditName = true);
        (string, NBTTagCompound)? CreateTagCompound(bool canEditName = true);
        (string, NBTTagIntArray)? CreateTagIntArray(bool canEditName = true);
    }
}