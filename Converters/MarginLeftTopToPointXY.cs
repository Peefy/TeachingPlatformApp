using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using TeachingPlatformApp.Utils;
using static TeachingPlatformApp.Converters.ConverterPara;

namespace TeachingPlatformApp.Converters
{
    public class MarginLeftTopToPointXY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = (double)value;
            return Math.Round(-(data - XInit) / XScale, 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = double.Parse(value.ToString());
            return Math.Round(-data * XScale + XInit, 2);
        }

    }

    public static class MarginPointToMapPointConverter
    {
        public static Point To(Point point)
        {
            //去除顶部导航栏的高度
            point.Y -= 30;
            return new Point((point.X - XInit) / XScale, (point.Y - YInit) / YScale);
        }

        public static Point Back(Point point)
        {
            return new Point(point.X * XScale + XInit, point.Y * YScale + YInit + 30);
        }

        public static Point Back(Point point, Thickness drawMargin)
        {
            return new Point((point.X - drawMargin.Left) * XScale + XInit, 
                (point.Y - drawMargin.Top) * YScale + YInit + 30);
        }

        public static Point To(Point point, Thickness drawMargin)
        {
            //去除顶部导航栏的高度
            point.Y -= 30;
            return new Point((point.X - drawMargin.Left - XInit) / XScale, 
                (point.Y - drawMargin.Top - YInit) / YScale);
        }
    }

}
