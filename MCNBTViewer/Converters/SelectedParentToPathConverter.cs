using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using MCNBTViewer.Core.Explorer.Items;

namespace MCNBTViewer.Converters {
    public class SelectedParentToPathConverter : IValueConverter {
        public bool UseParent { get; set; } = false;
        public string RootPath { get; set; } = "<root>";
        public string PathSeparator { get; set; } = " > ";

        public SelectedParentToPathConverter() {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is BaseNBTViewModel file) {
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

        public string GetPath(BaseNBTViewModel file) {
            List<BaseNBTViewModel> list = new List<BaseNBTViewModel>();
            for (BaseNBTViewModel f = file; f != null; f = f.Parent) {
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

        public static string AsPathElement(BaseNBTViewModel file) {
            return string.IsNullOrEmpty(file.Name) ? "<unnamed>" : file.Name;
        }
    }
}