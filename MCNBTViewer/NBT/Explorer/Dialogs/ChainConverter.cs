using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MCNBTViewer.NBT.Explorer.Dialogs {
    public class ChainValueConverter : List<IValueConverter>, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return this.Aggregate(value, (x, c) => c.Convert(x, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return this.Aggregate(value, (x, c) => c.ConvertBack(x, targetType, parameter, culture));
        }
    }
}