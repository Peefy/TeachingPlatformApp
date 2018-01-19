using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.Converters
{
    public class PointToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (Point)value;
            var str = "";
            if (point != null)
            {
                str = $"({point.X},{point.Y})";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pointStr = (string)value;
            if(string.IsNullOrEmpty(pointStr) == false)
            {
                if (pointStr.Contains(",") == true)
                {
                    pointStr = pointStr.Replace(',', '|');
                    pointStr = pointStr.Replace('(', '|');
                    pointStr = pointStr.Replace(')', '|');
                    var numStrs = pointStr.Split('|');
                    for (int i = 0; i < numStrs.Length - 1; ++i)
                    {
                        if (numStrs[i] == "")
                            continue;
                        if (int.TryParse(numStrs[i], out int x) == true &&
                            int.TryParse(numStrs[i + 1], out int y) == true)
                        {
                            return new Point(x, y);
                        }
                    }
                }
            }
            return new Point(0, 0);
        }
    }
}
