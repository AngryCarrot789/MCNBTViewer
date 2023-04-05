using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MCNBTViewer.Core.Explorer;

namespace MCNBTViewer.NBT.Explorer {
    public class ListFallbackSourceConverter : IMultiValueConverter {
        public NBTExplorerViewModel Explorer { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            foreach (object value in values) {
                if (value != null && value != DependencyProperty.UnsetValue) {
                    return value;
                }
            }

            if (this.Explorer != null) {
                return this.Explorer.RootFiles;
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}