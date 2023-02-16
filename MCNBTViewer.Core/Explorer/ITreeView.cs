namespace MCNBTViewer.Core.Explorer {
    public interface ITreeView {
        void SetSelectedFile(object file);

        object GetSelectedItem();
    }
}