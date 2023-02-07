using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MCNBTViewer.Explorer;

namespace MCNBTViewer.Controls {
    public class ExtendedListBox : ListBox {
        public static readonly DependencyProperty ExplorerProperty =
            DependencyProperty.Register(
                "Explorer",
                typeof(ExplorerViewModel),
                typeof(ExtendedListBox),
                new PropertyMetadata(null));

        public ExplorerViewModel Explorer {
            get => (ExplorerViewModel) this.GetValue(ExplorerProperty);
            set => this.SetValue(ExplorerProperty, value);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e) {
            base.OnMouseDoubleClick(e);
            if (e.Handled) {
                return;
            }

            if (this.SelectedItem is FileItemViewModel folder) {
                if (this.ItemContainerGenerator.ContainerFromItem(folder) is ListBoxItem item) {
                    if (item.IsMouseOver) {
                        this.Explorer.NavigateListFileItem(folder);
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (e.Handled) {
                return;
            }

            if (e.Key == Key.Enter) {
                if (this.SelectedItem is FileItemViewModel folder) {
                    this.Explorer.NavigateListFileItem(folder);
                }
            }
        }
    }
}