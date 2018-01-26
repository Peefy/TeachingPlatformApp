using System;
using System.Globalization;
using System.Windows.Data;

using static TeachingPlatformApp.Converters.ConverterPara;

namespace TeachingPlatformApp.Converters
{
    public class PointXYToMarginLeftTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = double.Parse(value.ToString());
            return data * XScale + XInit;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = double.Parse(value.ToString());
            return (data - XInit) / XScale;
        }

        public double Convert(string value)
        {
            var data = double.Parse(value.ToString());
            return data * XScale + XInit;
        }

    }
}
