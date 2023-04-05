using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MCNBTViewer.Core;
using MCNBTViewer.Core.Shortcuts.Managing;
using MCNBTViewer.Shortcuts;

namespace MCNBTViewer.Converters {
    public class ShortcutGestureConverter : BaseViewModel, IValueConverter {
        public static ShortcutGestureConverter Holder { get; } = new ShortcutGestureConverter();

        public string NoSuchShortcutText { get; set; } = null;

        public int Version { get; private set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(parameter is string path)) {
                throw new Exception("Parameter is not a shortcut string: " + parameter);
            }

            ManagedShortcut shortcut = AppShortcutManager.Instance.FindShortcutByPath(path);
            if (shortcut == null) {
                return this.NoSuchShortcutText ?? DependencyProperty.UnsetValue;
            }

            return shortcut.Shortcut.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        public static void BroadcastChange() {
            Holder.Version++;
            Holder.RaisePropertyChanged(nameof(Version));
        }
    }
}