using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Converters {
    public class NBTItemNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is BaseNBTViewModel item) {
                if (string.IsNullOrEmpty(item.Name)) {
                    if (item.Parent is NBTListViewModel tagList) {
                        return $"[{tagList.Children.IndexOf(item)}]";
                    }
                    else {
                        return "<unnamed>";
                    }
                }
                else {
                    return item.Name;
                }
            }
            else {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}