using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

namespace MCNBTViewer.Converters {
    public class NbtCollectiveNameInlinesConverter : BaseNBTTextRunConverter, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length != 3) {
                throw new Exception("Expected 3 values: [original Name] [number of items, or collection instance] [dummy count]");
            }

            List<Run> runs = new List<Run>();
            string name = values[0] as string;
            int size;
            if (values[1] is int count) {
                size = count;
            }
            else if (values[1] is ICollection collection) {
                size = collection.Count;
            }
            else {
                runs.Add(string.IsNullOrEmpty(name) ? new Run("<weird unnamed>") : new Run(name));
                return runs;
            }

            if (!string.IsNullOrEmpty(name)) {
                runs.Add(this.CreateNormalRun($"{name} "));
            }

            runs.Add(this.CreateExtraRun($"({size} entries)"));
            return runs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}