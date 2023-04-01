using System.Windows;
using System.Windows.Controls;

namespace MCNBTViewer.Utils {
    public static class UIUtils {
        public static T CreateWithStyle<T>(Style style) where T : FrameworkElement, new() {
            return new T() {
                Style = style
            };
        }

        public static T CreateCEWithStyle<T>(Style style) where T : FrameworkContentElement, new() {
            return new T() {
                Style = style
            };
        }
    }
}