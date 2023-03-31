using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

namespace MCNBTViewer.Converters {
    public class NBTArrayNameInlinesConverter : BaseNBTTextRunConverter, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length != 2) {
                throw new Exception("Expected 2 values: [original Name] [array instance]");
            }

            List<Run> runs = new List<Run>();
            string name = values[0] as string;
            if (!string.IsNullOrEmpty(name)) {
                runs.Add(this.CreateNormalRun($"{name} "));
            }

            if (values[1] is int[] intArray) {
                runs.Add(this.CreateExtraRun($"({intArray.Length} integer elements)"));
            }
            else if (values[1] is byte[] byteArray) {
                runs.Add(this.CreateExtraRun($"({byteArray.Length} byte elements)"));
            }
            else {
                runs.Add(this.CreateNormalRun("<invalid data>"));
            }

            return runs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}