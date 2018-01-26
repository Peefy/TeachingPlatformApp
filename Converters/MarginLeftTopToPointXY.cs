using System;
using System.Globalization;
using System.Windows.Data;

using static TeachingPlatformApp.Converters.ConverterPara;

namespace TeachingPlatformApp.Converters
{
    public class MarginLeftTopToPointXY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = (double)value;
            return -(data - XInit) / XScale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = double.Parse(value.ToString());
            return -data * XScale + XInit;
        }
    }

}
