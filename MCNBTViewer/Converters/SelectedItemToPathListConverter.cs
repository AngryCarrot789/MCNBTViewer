using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Converters {
    public class SelectedItemToPathListConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is BaseNBTViewModel selectedItem) {
                return selectedItem.PathChain;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}