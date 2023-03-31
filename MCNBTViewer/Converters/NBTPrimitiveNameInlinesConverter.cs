using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

namespace MCNBTViewer.Converters {
    public class NBTPrimitiveNameInlinesConverter : BaseNBTTextRunConverter, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length != 2) {
                throw new Exception("Expected 2 values: [original Name] [string data]");
            }

            List<Run> runs = new List<Run>();
            string name = values[0] as string ?? "<unnamed>";
            if (values[1] is object value) {
                runs.Add(this.CreateNormalRun(name + " "));
                runs.Add(this.CreateExtraRun("(" + value.ToString() + ")"));
            }
            else {
                runs.Add(this.CreateNormalRun(name));
            }

            return runs;
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