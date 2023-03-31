using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Core.Explorer {
    public interface ITreeView {
        void SetSelectedFile(object item);

        object GetSelectedItem();

        bool IsItemExpanded(BaseNBTViewModel item);
    }
}