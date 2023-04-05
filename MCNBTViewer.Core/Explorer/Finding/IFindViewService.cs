namespace MCNBTViewer.Core.Explorer.Finding {
    public interface IFindViewService {
        bool IsOpen { get; }

        void ShowFindView();

        void CloseFindView();
    }
}