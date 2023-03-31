using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Core.Explorer.Finding {
    public interface IFindView {
        bool IsOpen { get; }

        void ShowFindView();

        void CloseFindView();

        void NavigateTo(BaseNBTViewModel item);
    }
}