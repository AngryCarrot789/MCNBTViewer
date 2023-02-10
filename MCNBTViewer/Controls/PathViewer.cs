using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MCNBTViewer.NBT.Explorer.Items;

namespace MCNBTViewer.Controls {
    public class PathViewer : Control {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(BaseNBTViewModel), typeof(PathViewer), new FrameworkPropertyMetadata(null, PropertyChangedCallback));

        public static readonly DependencyProperty PathElementsProperty = DependencyProperty.Register("PathElements", typeof(ICollection), typeof(PathViewer), new PropertyMetadata(null));

        public ICollection PathElements {
            get => (ICollection) this.GetValue(PathElementsProperty);
            set => this.SetValue(PathElementsProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (e.NewValue == e.OldValue) {
                return;
            }

            ((PathViewer) d).OnSelectedItemChanged((BaseNBTViewModel) e.NewValue);
        }

        public BaseNBTViewModel SelectedItem {
            get => (BaseNBTViewModel) this.GetValue(SelectedItemProperty);
            set => this.SetValue(SelectedItemProperty, value);
        }

        private void OnSelectedItemChanged(BaseNBTViewModel nbt) {
            using (List<BaseNBTViewModel>.Enumerator chain = nbt.ParentChain.GetEnumerator()) {
                List<object> list = new List<object>();
                if (chain.MoveNext()) {
                    list.Add(chain.Current);
                    while (chain.MoveNext()) {
                        list.Add(" > ");
                        list.Add(chain.Current);
                    }
                }

                this.PathElements = list;
            }
        }

        private static string GetItemName(BaseNBTViewModel nbt) {
            if (string.IsNullOrEmpty(nbt.Name)) {
                if (nbt.Parent is NBTListViewModel tagList) {
                    return $"[{tagList.Children.IndexOf(nbt)}]";
                }
                else {
                    return "<unnamed>";
                }
            }
            else {
                return nbt.Name;
            }
        }
    }
}