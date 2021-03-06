﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using static TeachingPlatformApp.Converters.ConverterPara;

namespace TeachingPlatformApp.Converters
{
    public class PositionToMarginConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (Point)value;
            var margin = new Thickness(0,0,0,0);
            if (point != null)
            {
                margin = new Thickness(point.X * XScale + XInit, point.Y * YScale + YInit, 0 , 0);
            }
            return margin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }



    }
}
