using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using static TeachingPlatformApp.Converters.ConverterPara;

namespace TeachingPlatformApp.Converters
{
    public class SetPointToRealMarginPointConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (Point)value;
            if (point != null)
            {
                var newPoint = new Point
                {
                    X = point.X * XScale + XInit,
                    Y = point.Y * YScale + YInit
                };
                return newPoint;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public Point ConvertBackToPoint(Thickness margin)
        {
            return new Point((margin.Left - XInit) / XScale, (margin.Top - YInit) / YScale);
        }

    }

}
