using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MCNBTViewer.NBT.Explorer.ExplorerList {
    public class FallbackValueConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            foreach (object value in values) {
                if (value != null) {
                    return value;
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}