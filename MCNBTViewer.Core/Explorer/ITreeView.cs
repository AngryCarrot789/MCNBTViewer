using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Core.Explorer {
    public interface ITreeView {
        void SetSelectedFile(object file);

        object GetSelectedItem();

        bool IsItemExpanded(BaseNBTViewModel item);
    }
}