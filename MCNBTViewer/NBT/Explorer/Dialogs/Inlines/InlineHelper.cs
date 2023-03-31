using System;
using System.Collections.Generic;
using System.Windows.Documents;
using MCNBTViewer.Core.Utils;
using TextRange = MCNBTViewer.Core.Explorer.Finding.TextRange;

namespace MCNBTViewer.NBT.Explorer.Dialogs.Inlines {
    public static class InlineHelper {
        public static IEnumerable<Run> CreateString(string text, IEnumerable<TextRange> ranges, Func<string, Run> normalRunProvider, Func<string, Run> highlightedRunProvider) {
            int lastIndex = 0;
            foreach (TextRange range in ranges) {
                if ((range.Index - lastIndex) > 0) {
                    yield return normalRunProvider(text.JSubstring(lastIndex, range.Index));
                }

                yield return highlightedRunProvider(range.GetString(text));
                lastIndex = range.EndIndex;
            }

            if (lastIndex < text.Length) {
                yield return normalRunProvider(text.Substring(lastIndex));
            }
        }
    }
}