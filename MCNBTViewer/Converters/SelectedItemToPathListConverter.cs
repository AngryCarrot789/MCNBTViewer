using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MCNBTViewer.NBT.Explorer.Items;

namespace MCNBTViewer.Converters {
    public class SelectedItemToPathListConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is BaseNBTViewModel selectedItem) {
                List<string> list = new List<string>();
                foreach (BaseNBTViewModel nextItem in selectedItem.ParentChain) {
                    if (string.IsNullOrEmpty(nextItem.Name)) {
                        if (nextItem.Parent is NBTListViewModel tagList) {
                            list.Add($"[{tagList.Children.IndexOf(selectedItem)}]");
                        }
                        else {
                            list.Add("<unnamed>");
                        }
                    }
                    else {
                        list.Add(nextItem.Name);
                    }
                }

                return list;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}