using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.WswPlatform;
using TeachingPlatformApp.Converters;

namespace TeachingPlatformApp.Controls
{
    public class CanvasTrail : Canvas
    {

        private int scale = 10;
        private int init = 20;
        private int pointsUpNum = 10000;

        private Color grayColor = Colors.Gray;

        private List<Point> points;

        public WswAirplane WswAirplane { get; set; }

        public Point Convert(Point point)
        {
            if (point != null)
            {
                var newPoint = new Point();
                newPoint.X = point.X * scale + init;
                newPoint.Y = point.Y * scale + init;
                return newPoint;
            }
            else
            {
                return default(Point);
            }
        }

        public CanvasTrail()
        {
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            points = new List<Point>();
            this.pointsUpNum = JsonFileConfig.ReadFromFile().DataShowConfig.DrawTrailPointNumUp;
        }

        public void DrawAllLines()
        {
            if(points?.Count >= 2)
            {
                var count = points.Count;
                this.Children.Clear();
                for(var i = 0;i < count - 1 ;++i)
                {
                    var path = new Path()
                    {
                        Stroke = new SolidColorBrush(grayColor),
                        StrokeThickness = 2,
                        Data = new LineGeometry()
                        {
                            StartPoint = Convert(points[0]),
                            EndPoint = Convert(points[1])
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
                points.Add(point);
                if (points.Count > pointsUpNum)
                    ClearPoint();
                if (points.Count < 2)
                    return;
                var path = new Path()
                {
                    Stroke = new SolidColorBrush(grayColor),
                    StrokeThickness = 2,
                    Data = new LineGeometry()
                    {
                        StartPoint = Convert(points[points.Count - 2]),
                        EndPoint = Convert(points[points.Count - 1])
                    }
                };
                this.Children.Add(path);
            }
        }

        public void ClearPoint()
        {
            this.points.Clear();
            this.Children.Clear();
        }

    }
}
