using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeachingPlatformApp.Converters
{
    public class BoolToVisiblyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)value;
            return result == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
