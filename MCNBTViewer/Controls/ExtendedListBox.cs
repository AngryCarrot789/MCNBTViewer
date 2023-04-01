using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MCNBTViewer.Core.Explorer;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Controls {
    public class ExtendedListBox : ListBox {
        public static readonly DependencyProperty ExplorerProperty =
            DependencyProperty.Register(
                "Explorer",
                typeof(NBTExplorerViewModel),
                typeof(ExtendedListBox),
                new PropertyMetadata(null));

        public NBTExplorerViewModel Explorer {
            get => (NBTExplorerViewModel) this.GetValue(ExplorerProperty);
            set => this.SetValue(ExplorerProperty, value);
        }

        private ScrollViewer PART_ScrollViewer;

        public ExtendedListBox() {
            this.PreviewMouseWheel += this.ExtendedListBox_PreviewMouseWheel;
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
            this.PART_ScrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
        }

        private void ExtendedListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && this.PART_ScrollViewer != null) {
                if (e.Delta < 0) {
                    // scroll right
                    this.PART_ScrollViewer.LineRight();
                    this.PART_ScrollViewer.LineRight();
                    this.PART_ScrollViewer.LineRight();
                }
                else if (e.Delta > 0) {
                    this.PART_ScrollViewer.LineLeft();
                    this.PART_ScrollViewer.LineLeft();
                    this.PART_ScrollViewer.LineLeft();
                }
                else {
                    return;
                }

                e.Handled = true;
                return;
            }
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e) {
            base.OnMouseDoubleClick(e);
            if (e.Handled) {
                return;
            }

            if (this.SelectedItem is BaseNBTViewModel file) {
                if (this.ItemContainerGenerator.ContainerFromItem(file) is ListBoxItem item) {
                    if (item.IsMouseOver) {
                        this.Explorer.UseItem(file);
                    }
                }
            }
        }
        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (e.Handled) {
                return;
            }

            if (this.IsFocused && e.Key == Key.Enter) {
                if (this.SelectedItem is BaseNBTViewModel file) {
                    this.Explorer.UseItem(file);
                }
            }
        }
    }
}