using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeachingPlatformApp.Converters
{
    public class PositionToMarginConverter : IValueConverter
    {

        private static int scale = 10;
        private static int init = 20;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (Point)value;
            var margin = new Thickness(0,0,0,0);
            if (point != null)
            {
                margin = new Thickness(point.X * scale + init, point.Y * scale + init, 0 , 0);
            }
            return margin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }



    }
}
