using System;
using System.Globalization;
using System.Windows.Data;

namespace MCNBTViewer.NBT.Converters.Basic {
    public class NBTArrayNameConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length != 2) {
                throw new Exception("Expected 2 values: [original Name] [array instance]");
            }

            return FormatName(values[0] as string, values[1]);
        }

        public static string FormatName(string name, object value) {
            if (value is int[] intArray) {
                return string.IsNullOrEmpty(name) ? $"{intArray.Length} integer elements" : $"{name}: {intArray.Length} integer elements";
            }
            else if (value is byte[] byteArray) {
                return string.IsNullOrEmpty(name) ? $"{byteArray.Length} byte elements" : $"{name}: {byteArray.Length} byte elements";
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