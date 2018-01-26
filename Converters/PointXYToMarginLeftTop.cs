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
            return Math.Round(data * XScale + XInit, 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = double.Parse(value.ToString());
            return Math.Round((data - XInit) / XScale, 2);
        }

        public double Convert(string value)
        {
            var data = double.Parse(value.ToString());
            return Math.Round(data * XScale + XInit, 2);
        }

    }
}
