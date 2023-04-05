using System;
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

        private volatile bool isProcessingDoubleClick;
        private volatile bool isProcessingKeyDown;
        protected override async void OnMouseDoubleClick(MouseButtonEventArgs e) {
            base.OnMouseDoubleClick(e);
            if (e.Handled || this.isProcessingDoubleClick) {
                return;
            }

            this.isProcessingDoubleClick = true;
            try {
                if (this.SelectedItem is BaseNBTViewModel file) {
                    if (this.ItemContainerGenerator.ContainerFromItem(file) is ListBoxItem item) {
                        if (item.IsMouseOver) {
                            await this.Explorer.UseItem(file);
                        }
                    }
                }
            }
            finally {
                this.isProcessingDoubleClick = false;
            }
        }
        protected override async void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (e.Handled || this.isProcessingKeyDown) {
                return;
            }

            this.isProcessingKeyDown = true;
            try {
                if (this.IsFocused && e.Key == Key.Enter) {
                    if (this.SelectedItem is BaseNBTViewModel file) {
                        await this.Explorer.UseItem(file);
                    }
                }
            }
            finally {
                this.isProcessingKeyDown = false;
            }
        }
    }
}