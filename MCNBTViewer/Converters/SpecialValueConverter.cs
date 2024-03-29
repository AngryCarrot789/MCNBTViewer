using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MCNBTViewer.Converters {
    public abstract class SpecialValueConverter<T> : MarkupExtension, IValueConverter where T : class, new() {
        private static T instance;

        public static T Instance { get => instance ?? (instance = new T()); }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return Instance;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}