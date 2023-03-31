using System.Threading.Tasks;
using MCNBTViewer.Core.NBT;

namespace MCNBTViewer.Core.Explorer.Dialogs {
    public interface NewTagDialogService {
        Task<(string, NBTTagEnd)> CreateTagEnd(bool canEditName = true);
        Task<(string, NBTTagByte)> CreateTagByte(bool canEditName = true);
        Task<(string, NBTTagShort)> CreateTagShort(bool canEditName = true);
        Task<(string, NBTTagInt)> CreateTagInt(bool canEditName = true);
        Task<(string, NBTTagLong)> CreateTagLong(bool canEditName = true);
        Task<(string, NBTTagFloat)> CreateTagFloat(bool canEditName = true);
        Task<(string, NBTTagDouble)> CreateTagDouble(bool canEditName = true);
        Task<(string, NBTTagByteArray)> CreateTagByteArray(bool canEditName = true);
        Task<(string, NBTTagString)> CreateTagString(bool canEditName = true);
        Task<(string, NBTTagList)> CreateTagList(bool canEditName = true);
        Task<(string, NBTTagCompound)> CreateTagCompound(bool canEditName = true);
        Task<(string, NBTTagIntArray)> CreateTagIntArray(bool canEditName = true);
    }
}