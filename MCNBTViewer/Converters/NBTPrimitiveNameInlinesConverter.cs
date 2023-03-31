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
            string name = values[0] as string ?? "";
            if (values[1] is object value) {
                if (!string.IsNullOrEmpty(name))
                    runs.Add(this.CreateNormalRun(name + " "));
                runs.Add(this.CreateExtraRun("(" + value + ")"));
            }
            else {
                runs.Add(this.CreateNormalRun(name));
            }

            return runs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}