using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeachingPlatformApp.Converters
{
    public class NullToVisiblyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (Point)value;
            if (result != null)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
