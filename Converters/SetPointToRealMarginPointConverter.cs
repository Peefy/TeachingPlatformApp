using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeachingPlatformApp.Converters
{
    public class SetPointToRealMarginPointConverter : IValueConverter
    {

        private static int scale = 10;
        private static int init = 20;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (Point)value;
            if (point != null)
            {
                var newPoint = new Point();
                newPoint.X = point.X * scale + init;
                newPoint.Y = point.Y * scale + init;
                return newPoint;
            }
            else
            {
                return null;
            }
            //return newPoint;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
