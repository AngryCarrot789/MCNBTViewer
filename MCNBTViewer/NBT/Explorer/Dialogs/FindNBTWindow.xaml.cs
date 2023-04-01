using System;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Explorer.Finding;
using MCNBTViewer.NBT.Explorer.Finding;
using MCNBTViewer.Views;

namespace MCNBTViewer.NBT.Explorer.Dialogs {
    /// <summary>
    /// Interaction logic for FindNBTWindow.xaml
    /// </summary>
    public partial class FindNBTWindow : BaseWindow {
        public FindNBTWindow() {
            this.InitializeComponent();
            this.DataContext = new FindViewModel() {
                Window = this
            };
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);

            ((FindView) IoC.FindView).OnClosedInternal(this);
            if (this.DataContext is FindViewModel findViewModel) {
                findViewModel.Dispose();
            }
        }
    }
}
