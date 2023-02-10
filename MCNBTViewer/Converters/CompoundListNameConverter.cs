using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace MCNBTViewer.Converters {
    public class CompoundListNameConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length != 2) {
                throw new Exception("Expected 2 values: [original name] [number of items, or collection instance]");
            }

            return FormatName(values[0] as string, values[1]);
        }

        public static string FormatName(string name, object value) {
            int size;
            if (value is int i) {
                size = i;
            }
            else if (value is ICollection collection) {
                size = collection.Count;
            }
            else {
                return name;
            }

            return string.IsNullOrEmpty(name) ? $"{size} entries" : $"{name}: {size} entries";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}