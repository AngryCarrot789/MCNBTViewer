using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using MCNBTViewer.Explorer;

namespace MCNBTViewer.Converters {
    public class SelectedParentToPathConverter : IValueConverter {
        public bool UseParent { get; set; } = true;
        public string RootPath { get; set; } = "<Root>";
        public string PathSeparator { get; set; } = ">";

        public SelectedParentToPathConverter() {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is FileItemViewModel file) {
                return this.UseParent ? (file.Parent != null ? GetPath(file.Parent) : this.RootPath) : GetPath(file);
            }
            else if (value == null) {
                return this.RootPath;
            }
            else {
                return $"<ERROR TYPE {value}>";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        public string GetPath(FileItemViewModel file) {
            List<FileItemViewModel> list = new List<FileItemViewModel>();
            for (FileItemViewModel f = file; file != null; file = file.Parent) {
                list.Add(f);
            }

            StringBuilder sb = new StringBuilder();
            if (list.Count > 0) {
                sb.Append(AsPathElement(list[list.Count - 1]));
            }

            for (int i = list.Count - 2; i >= 0; i--) {
                sb.Append(this.PathSeparator).Append(AsPathElement(list[i]));
            }

            return sb.ToString();
        }

        public static string AsPathElement(FileItemViewModel file) {
            return file.Name ?? "<unnamed file>";
        }
    }
}