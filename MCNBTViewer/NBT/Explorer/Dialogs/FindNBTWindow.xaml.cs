using System;
using System.Windows.Controls.Primitives;
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

            this.Loaded += (sender, args) => {
                if (this.NameBox.IsFocused || (this.NameBox.Focusable && this.NameBox.Focus())) {
                    this.NameBox.SelectAll();
                }
            };
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);

            ((FindView) IoC.FindView).OnClosedInternal(this);
            if (this.DataContext is FindViewModel findViewModel) {
                findViewModel.Dispose();
            }
        }

        private void ToggleButtonCheckChanged(object sender, System.Windows.RoutedEventArgs e) {
            if (sender is ToggleButton button && button.IsChecked.HasValue) {
                this.Topmost = button.IsChecked.Value;
            }
        }
    }
}
