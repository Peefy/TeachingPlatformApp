using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using TeachingPlatformApp.Models;
using static TeachingPlatformApp.Converters.ConverterPara;

namespace TeachingPlatformApp.Converters
{
    public class SetPointsToPathFigureConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var points = (ObservableRangeCollection<Point>)value;
            var str = "";
            if (points?.Count > 0)
            {
                var startPoint = SetPointToRealMarginPoint(points.FirstOrDefault());
                str += $"M {startPoint.X},{startPoint.Y} L ";
                for (var i = 1; i < points.Count; ++i)
                {
                    var point = SetPointToRealMarginPoint(points[i]);
                    str += $"{point.X},{point.Y} ";
                }
                str += $"{startPoint.X},{startPoint.Y}";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public static Point SetPointToRealMarginPoint(Point point)
        {
            return new Point(point.X * XScale + XInit, point.Y * YScale + YInit);
        }
    }

}
