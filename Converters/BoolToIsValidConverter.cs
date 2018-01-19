using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeachingPlatformApp.Converters
{
    public class BoolToIsValidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)value;
            return result == true ? "成功" : "失败";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
