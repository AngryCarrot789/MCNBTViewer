using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.Explorer;
using MCNBTViewer.NBT.Explorer;
using MCNBTViewer.NBT.Explorer.Items;

namespace MCNBTViewer.Controls {
    public class ExtendedTreeView : TreeView, ITreeView {
        public static readonly DependencyProperty ExplorerProperty =
            DependencyProperty.Register(
                "Explorer",
                typeof(NBTExplorerViewModel),
                typeof(ExtendedTreeView),
                new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((NBTExplorerViewModel) e.NewValue).TreeView = (ExtendedTreeView) d;
        }

        public NBTExplorerViewModel Explorer {
            get => (NBTExplorerViewModel) this.GetValue(ExplorerProperty);
            set => this.SetValue(ExplorerProperty, value);
        }

        public ExtendedTreeView() {

        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e) {
            base.OnSelectedItemChanged(e);
            if (e.Handled) {
                return;
            }

            if (e.NewValue is BaseNBTViewModel file) {
                this.Explorer.OnTreeSelectItem(file);
            }
        }

        public void SetSelectedFile(FileItemViewModel file) {
            if (file is FolderItemViewModel folder) {
                for (FolderItemViewModel parent = folder.Parent; parent != null; parent = parent.Parent) {
                    if (!parent.IsExpanded && parent.CanExpand) {
                        parent.IsExpanded = true;
                    }
                }

                folder.IsExpanded = true;
            }
        }
    }
}
