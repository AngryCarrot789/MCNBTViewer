using System;
using System.Globalization;
using System.Windows.Data;

namespace MCNBTViewer.Converters {
    public class NBTPrimitiveNameConverter  : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length != 2) {
                throw new Exception("Expected 2 values: [original Name] [string data]");
            }

            return FormatName(values[0] as string, values[1] as string);
        }

        public static string FormatName(string name, string data) {
            if (string.IsNullOrEmpty(name)) {
                return data ?? "<invalid data>";
            }
            else if (data != null) {
                return $"{name}: {data}";
            }
            else {
                return name;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}