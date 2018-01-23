using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TeachingPlatformApp.Utils
{
    public static class VectorPointHelper
    {
        public static double GetPointToLineDistance(Point singlePoint, Point linePoint1, Point linePoint2)
        {
            if (linePoint1.X == linePoint2.X)
                return Math.Abs(singlePoint.X - linePoint1.X);
            if (linePoint1.Y == linePoint2.Y)
                return Math.Abs(singlePoint.Y - linePoint1.Y);
            var a = 1 / (linePoint2.X - linePoint1.X);
            var b = 1 / (linePoint1.Y - linePoint2.Y);
            var c = linePoint1.Y / (linePoint2.Y - linePoint1.Y) +
                linePoint1.X / (linePoint1.X - linePoint2.X);
            var x = singlePoint.X;
            var y = singlePoint.Y;
            return Math.Abs((a * x + b * y + c)) / Math.Sqrt(a * a + b * b);
        }

        public static double GetTwoPointDistance(Point point1, Point point2)
        {
            var subX = point2.X - point1.X;
            var subY = point2.Y - point1.Y;
            return Math.Sqrt(subX * subX + subY * subY);
        }

        public static List<double> GetPointToAllSetPointsLineDistance(Point point, IList<Point> points)
        {
            var distances = new List<double>();
            if (points?.Count > 1 && point != null)
            {
                var count = points.Count;
                for (var i = 0; i < count - 1; ++i)
                {
                    distances.Add(GetPointToLineDistance(point, points[i], points[i + 1]));
                }
                distances.Add(GetPointToLineDistance(point, points[0], points[count - 1]));
            }
            return distances;
        }

        public static List<double> GetPointToAllSetPointsDistance(Point point, IList<Point> points)
        {
            var distances = new List<double>();
            if (points?.Count > 1 && point != null)
            {
                var count = points.Count;
                for (var i = 0; i < count; ++i)
                {
                    distances.Add(GetTwoPointDistance(point, points[i]));
                }
            }
            return distances;
        }

        public static bool JudgePointInParallelogram(Point point, Point point1, Point point2, double margin = 5)
        {
            bool oddNodes = false;
            var maxX = Math.Max(point1.X, point2.X);
            var minX = Math.Min(point1.X, point2.X);
            var maxY  = Math.Max(point1.Y, point2.Y);
            var minY = Math.Min(point1.Y, point2.Y);
            var x = point.X;
            var y = point.Y;
            var polySides = 4;
            int i, j = polySides - 1;
            double[] polyX = {minX - margin,  minX - margin, maxX + margin, maxX + margin};
            double[] polyY = {maxY + margin,  minY - margin, minY - margin, maxY + margin};
            for (i = 0; i < polySides; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y
                || polyY[j] < y && polyY[i] >= y)
                && (polyX[i] <= x || polyX[j] <= x))
                {
                    oddNodes ^= (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x);
                }
                j = i;
            }
            return oddNodes;
        }


    }
}
