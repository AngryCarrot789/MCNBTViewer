using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Converters {
    public class NBTToPathConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value is BaseNBTViewModel nbt ? nbt.Path : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}