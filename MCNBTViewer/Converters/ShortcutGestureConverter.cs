using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using MCNBTViewer.Core.Shortcuts.Managing;
using MCNBTViewer.Shortcuts;

namespace MCNBTViewer.Converters {
    public class ShortcutGestureConverter : SpecialValueConverter<ShortcutGestureConverter>, IValueConverter, INotifyPropertyChanged {
        public string NoSuchShortcutText { get; set; } = null;

        public int Version { get; private set; }

        public static void BroadcastChange() {
            Instance.Version++;
            Instance.OnPropertyChanged(nameof(Version));
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(parameter is string path)) {
                throw new Exception("Parameter is not a shortcut string: " + parameter);
            }

            ManagedShortcut shortcut = AppShortcutManager.Instance.FindShortcutByPath(path);
            if (shortcut == null) {
                return this.NoSuchShortcutText ?? DependencyProperty.UnsetValue;
            }

            return shortcut.Shortcut.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}