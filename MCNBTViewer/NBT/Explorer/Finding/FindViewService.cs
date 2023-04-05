using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer.Finding;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.NBT.Explorer.Dialogs;

namespace MCNBTViewer.NBT.Explorer.Finding {
    public class FindViewService : IFindViewService {
        private FindNBTWindow window;

        public FindViewModel ViewModel => this.window?.DataContext as FindViewModel;

        public bool IsOpen => this.window != null && this.window.IsLoaded;

        public void ShowFindView() {
            if (this.IsOpen) {
                return;
            }

            this.window = new FindNBTWindow();
            this.window.Show();
        }

        public void CloseFindView() {
            if (this.IsOpen) {
                this.window.Close();
                this.window = null;
            }
        }

        internal void OnClosedInternal(FindNBTWindow window) {
            this.window = null;
            (window.DataContext as FindViewModel)?.Dispose();
        }
    }
}