using System;
using System.Windows;
using System.Windows.Data;

namespace RamanSoftwareV2.Resources.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, System.Globalization.CultureInfo culture)
            => (bool)value ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type type, object parameter, System.Globalization.CultureInfo culture)
            => (Visibility)value == Visibility.Visible;
    }
}
