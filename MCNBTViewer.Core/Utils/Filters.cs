using MCNBTViewer.Core.Views.Dialogs.FilePicking;

namespace MCNBTViewer.Core.Utils {
    public static class Filters {
        public static string NBTDatFilter = Filter.Of().AddFilter("DAT/NBT", "dat").ToString();
        public static string NBTDatAndAllFilesFilter = Filter.Of().AddFilter("DAT/NBT", "dat").AddAllFiles().ToString();
    }
}