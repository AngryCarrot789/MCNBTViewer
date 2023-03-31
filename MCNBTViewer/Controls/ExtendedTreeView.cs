using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MCNBTViewer.Core.Explorer;
using MCNBTViewer.Core.Explorer.Items;

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

        private static MethodInfo ChangeSelectionMethod;

        static ExtendedTreeView() {
            ChangeSelectionMethod = typeof(TreeView).GetMethod("ChangeSelection", BindingFlags.Instance | BindingFlags.NonPublic);
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

        public static TreeViewItem ContainerFromItemRecursive(ItemContainerGenerator root, object item) {
            if (root == null) {
                return null;
            }

            var treeViewItem = root.ContainerFromItem(item) as TreeViewItem;
            if (treeViewItem != null)
                return treeViewItem;
            foreach (object subItem in root.Items) {
                treeViewItem = root.ContainerFromItem(subItem) as TreeViewItem;
                TreeViewItem search = ContainerFromItemRecursive(treeViewItem?.ItemContainerGenerator, item);
                if (search != null)
                    return search;
            }

            return null;
        }

        public static bool AccumulateItemContainerGeneratorChainForItem(ItemContainerGenerator root, object item, IList<ItemContainerGenerator> generators) {
            if (root.ContainerFromItem(item) is TreeViewItem treeViewItem) {
                generators.Add(root);
                return true;
            }

            foreach (object subItem in root.Items) {
                if (ReferenceEquals(subItem, item) || (subItem != null && subItem.Equals(item))) {
                    generators.Add(root);
                    return true;
                }

                if ((treeViewItem = root.ContainerFromItem(subItem) as TreeViewItem) == null) {
                    continue;
                }

                if (AccumulateItemContainerGeneratorChainForItem(treeViewItem.ItemContainerGenerator, item, generators)) {
                    return true;
                }
            }

            return false;
        }

        public static TreeViewItem GetAndGenerateContainerUntilItemFound(UIElement rootElement, ItemContainerGenerator root, object item) {
            if (root == null) {
                return null;
            }

            if (root.ContainerFromItem(item) is TreeViewItem treeViewItem)
                return treeViewItem;

            foreach (object subItem in root.Items) {
                if ((treeViewItem = root.ContainerFromItem(subItem) as TreeViewItem) == null) {
                    rootElement.UpdateLayout();
                    GenerateChildren(root);
                    rootElement.UpdateLayout();
                    if ((treeViewItem = root.ContainerFromItem(subItem) as TreeViewItem) == null) {
                        continue;
                    }
                }

                if (ReferenceEquals(subItem, item) || (subItem != null && subItem.Equals(item))) {
                    return treeViewItem;
                }

                TreeViewItem search = GetAndGenerateContainerUntilItemFound(treeViewItem, treeViewItem.ItemContainerGenerator, item);
                if (search != null)
                    return search;
            }

            return null;

            // if (root.ContainerFromItem(item) is TreeViewItem treeViewItem)
            //     return treeViewItem;
            // foreach (object subItem in root.Items) {
            //     if ((treeViewItem = root.ContainerFromItem(subItem) as TreeViewItem) != null) {
            //         TreeViewItem search = ContainerFromItemRecursive(treeViewItem.ItemContainerGenerator, item);
            //         if (search != null)
            //             return search;
            //     }
            // }
            // return null;
        }

        public static void GenerateChildren(IItemContainerGenerator generator) {
            using (generator.StartAt(new GeneratorPosition(0, 0), GeneratorDirection.Forward)) {
                while (generator.GenerateNext() is UIElement next) {
                    generator.PrepareItemContainer(next);
                }
            }
        }

        public void SetSelectedFile(object item) {
            if (item is TreeViewItem obj) {
                obj.IsSelected = true;
                obj.BringIntoView();
                // ChangeSelectionMethod.Invoke(this, new object[] {
                //     this.ItemContainerGenerator.ItemFromContainer(obj),
                //     obj,
                //     true
                // });
            }
            else {
                TreeViewItem treeViewItem = ContainerFromItemRecursive(this.ItemContainerGenerator, item);
                if (treeViewItem != null) {
                    // for (ItemsControl parent = ItemsControlFromItemContainer(treeViewItem); parent != null; parent = ItemsControlFromItemContainer(parent)) {
                    //     if (parent is TreeViewItem treeItem) {
                    //         treeItem.IsExpanded = true;
                    //         // treeItem.ExpandSubtree();
                    //     }
                    //     else {
                    //         break;
                    //     }
                    // }

                    treeViewItem.IsSelected = true;
                    treeViewItem.BringIntoView();
                }

                // ChangeSelectionMethod.Invoke(this, new object[] {
                //     item,
                //     this.ItemContainerGenerator.ContainerFromItem(item),
                //     true
                // });
            }
            // if (file is BaseNBTCollectionViewModel folder) {
            //     for (BaseNBTCollectionViewModel parent = folder.Parent; parent != null; parent = parent.Parent) {
            //         if (!parent.IsExpanded && parent.CanExpand) {
            //             parent.IsExpanded = true;
            //         }
            //     }
            //     folder.IsExpanded = true;
            // }
        }

        public object GetSelectedItem() {
            return this.SelectedItem;
        }

        public bool IsItemExpanded(BaseNBTViewModel item) {
            if (item == null) {
                return false;
            }

            DependencyObject container = this.ItemContainerGenerator.ContainerFromItem(item);
            if (container is TreeViewItem treeItem && treeItem.IsExpanded) {
                return true;
            }

            return false;
        }
    }
}