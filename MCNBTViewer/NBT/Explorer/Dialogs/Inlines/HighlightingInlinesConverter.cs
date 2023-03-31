using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using MCNBTViewer.Core.Explorer.Finding;

namespace MCNBTViewer.NBT.Explorer.Dialogs.Inlines {
    public class NameValueInlinesConverter : BaseInlineHighlightConverter, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values == null || values.Length != 4) {
                throw new Exception("Expected 4 values, got " + (values == null ? "null" : values.Length.ToString()));
            }

            string name = values[0] as string;
            string value = values[1] as string;
            List<TextRange> nameRanges = (List<TextRange>) values[2];
            List<TextRange> valueRanges = (List<TextRange>) values[3];


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}