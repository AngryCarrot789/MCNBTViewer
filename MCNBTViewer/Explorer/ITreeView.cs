using MCNBTViewer.NBT.Explorer.Items;

namespace MCNBTViewer.Explorer {
    public interface ITreeView {
        void SetSelectedFile(object file);

        object GetSelectedItem();
    }
}