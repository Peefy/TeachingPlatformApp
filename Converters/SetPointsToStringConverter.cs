using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.Converters
{
    public class SetPointsToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var points = (ObservableRangeCollection<Point>)value;
            var str = "";
            if(points?.Count > 0)
            {
                foreach(var point in points)
                {
                    str += $"({point.X},{point.Y}),";
                }
                str = str.Remove(str.Length - 1, 1);
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pointsStr = (string)value;
            var points = new ObservableRangeCollection<Point>();
            try
            {
                if (string.IsNullOrEmpty(pointsStr) == false)
                {
                    if (pointsStr.Contains(",") == true)
                    {
                        pointsStr = pointsStr.Replace(',', '|');
                        pointsStr = pointsStr.Replace('(', '|');
                        pointsStr = pointsStr.Replace(')', '|');
                        var numStrs = pointsStr.Split('|');
                        for (int i = 0; i < numStrs.Length - 1; ++i)
                        {
                            if (numStrs[i] == "")
                                continue;
                            if (int.TryParse(numStrs[i], out int x) == true &&
                                int.TryParse(numStrs[i + 1], out int y) == true)
                            {
                                points.Add(new Point(x, y));
                                i++;
                            }
                        }
                    }
                }
            }
            catch
            {
                return points;
            }
            return points;
        }
    
    }
}
