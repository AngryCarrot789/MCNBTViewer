using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.Explorer;

namespace MCNBTViewer.Controls {
    public class ExtendedTreeView : TreeView, ITreeView {
        public static readonly DependencyProperty ExplorerProperty =
            DependencyProperty.Register(
                "Explorer",
                typeof(ExplorerViewModel),
                typeof(ExtendedTreeView),
                new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ExplorerViewModel) e.NewValue).TreeView = (ExtendedTreeView) d;
        }

        public ExplorerViewModel Explorer {
            get => (ExplorerViewModel) this.GetValue(ExplorerProperty);
            set => this.SetValue(ExplorerProperty, value);
        }

        public ExtendedTreeView() {

        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e) {
            if (e.NewValue is FileItemViewModel file) {
                this.Explorer.SelectFileFromTree(file);
            }

            if (e.Handled) {
                return;
            }

            base.OnSelectedItemChanged(e);
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
