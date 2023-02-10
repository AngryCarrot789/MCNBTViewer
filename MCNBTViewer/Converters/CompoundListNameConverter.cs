using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace MCNBTViewer.Converters {
    public class CollectionItemNameConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length != 2) {
                throw new Exception("Expected 2 values: [original name] [number of items, or collection instance]");
            }

            if (values[0] == null) {
                return GetCollectionCount(values[1]);
            }
            else if (values[1] is string name) {
                return name.Length == 0 ? GetCollectionCount(values[1]) : $"{name}: {GetCollectionCount(values[1])}";
            }
            else {
                return values[0];
            }
        }

        public static string GetCollectionCount(object value) {
            int size;
            if (value is int i) {
                size = i;
            }
            else if (value is ICollection collection) {
                size = collection.Count;
            }
            else {
                throw new Exception("Expected size or collection, not: " + value);
            }

            return $"{size} entries";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}