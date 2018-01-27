using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.WswPlatform;
using TeachingPlatformApp.Converters;

using static TeachingPlatformApp.Converters.ConverterPara;

namespace TeachingPlatformApp.Controls
{
    public class CanvasTrail : Canvas
    {

        private int _pointsUpNum = 5000;

        private Color _grayColor = Colors.Gray;

        private List<Point> _points;

        public WswModelKind WswModelKind { get; set; }

        public CanvasTrail()
        {
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            _points = new List<Point>();
            this._pointsUpNum = JsonFileConfig.ReadFromFile().DataShowConfig.DrawTrailPointNumUp;
        }

        public Point Convert(Point point)
        {
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
                return default;
            }
        }

        public void DrawAllLines()
        {
            if(_points?.Count >= 2)
            {
                var count = _points.Count;
                this.Children.Clear();
                for(var i = 0;i < count - 1 ;++i)
                {
                    var path = new Path()
                    {
                        Stroke = new SolidColorBrush(_grayColor),
                        StrokeThickness = 2,
                        Data = new LineGeometry()
                        {
                            StartPoint = Convert(_points[0]),
                            EndPoint = Convert(_points[1])
                        }
                    };
                    this.Children.Add(path);
                }
            }
        }

        public void AddPoint(Point point)
        {
            if(point != null)
            {
                _points.Add(point);
                if (_points.Count > _pointsUpNum)
                    ClearPoint();
                if (_points.Count < 2)
                    return;
                var path = new Path()
                {
                    Stroke = new SolidColorBrush(_grayColor),
                    StrokeThickness = 2,
                    Data = new LineGeometry()
                    {
                        StartPoint = Convert(_points[_points.Count - 2]),
                        EndPoint = Convert(_points[_points.Count - 1])
                    }
                };
                this.Children.Add(path);
            }
        }

        public void ClearPoint()
        {
            this._points.Clear();
            this.Children.Clear();
        }

    }
}
