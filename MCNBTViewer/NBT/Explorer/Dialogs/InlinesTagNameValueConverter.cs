using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using MCNBTViewer.Core.Explorer.Items;
using MCNBTViewer.NBT.Explorer.Dialogs.Inlines;
using TextRange = MCNBTViewer.Core.Explorer.Finding.TextRange;

namespace MCNBTViewer.NBT.Explorer.Dialogs {
    public class InlinesTagNameValueConverter : BaseInlineHighlightConverter, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values == null || values.Length != 4) {
                throw new Exception("Expected 4 values, got " + (values != null ? values.Length.ToString() : "null"));
            }

            BaseNBTViewModel nbt = (BaseNBTViewModel) values[0];
            string primitiveOrArrayFoundValue = (string) values[1];
            List<TextRange> nameMatches = (List<TextRange>) values[2];
            List<TextRange> valueMatches = (List<TextRange>) values[3];

            List<Run> output = new List<Run>();
            if (!string.IsNullOrEmpty(nbt.Name)) {
                output.AddRange(this.CreateString(nbt.Name, nameMatches));
            }

            if (!string.IsNullOrEmpty(primitiveOrArrayFoundValue)) {
                if (output.Count > 0) {
                    output.Add(this.CreateNormalRun(": "));
                }

                output.AddRange(this.CreateString(primitiveOrArrayFoundValue, valueMatches));
            }
            else if (nbt is NBTPrimitiveViewModel primitive) {
                output.Add(this.CreateNormalRun(" (" + primitive.Data + ")"));
            }
            else if (nbt is NBTIntArrayViewModel intArray) {
                output.Add(this.CreateNormalRun(" (" + string.Join(",", intArray.Data) + ")"));
            }
            else if (nbt is NBTByteArrayViewModel byteArray) {
                output.Add(this.CreateNormalRun(" (" + string.Join(",", byteArray.Data) + ")"));
            }

            return output;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}