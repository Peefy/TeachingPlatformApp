using System;
using System.Collections.Generic;
using System.Windows;

namespace TeachingPlatformApp.Utils
{
    public static class VectorPointHelper
    {

        /// <summary>
        /// 将平面内一个点平移x和y
        /// </summary>
        /// <param name="point"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point PointOffset(Point point , double x, double y)
        {
            return new Point(point.X + x, point.Y + y);
        }

        /// <summary>
        /// 获得两个向量顶端坐标的距离
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static double VectorDistance(Vector vector1, Vector vector2)
        {
            var subX = vector1.X - vector2.X;
            var subY = vector1.Y - vector2.Y;
            return Math.Sqrt(subX * subX + subY * subY);
        }
        /// <summary>
        /// 平面内到点到线段所在直线距离
        /// </summary>
        /// <param name="singlePoint"></param>
        /// <param name="linePoint1"></param>
        /// <param name="linePoint2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 平面内两个坐标的距离
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double GetTwoPointDistance(Point point1, Point point2)
        {
            var subX = point2.X - point1.X;
            var subY = point2.Y - point1.Y;
            return Math.Sqrt(subX * subX + subY * subY);
        }

        /// <summary>
        /// 获得当前坐标到所有航路点连线之间的距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 平面两点所连线段与X轴夹角
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double GetTwoPointLineAngle(Point point1, Point point2)
        {
            var vector1 = new Vector(point1.X, point1.Y);
            var vector2 = new Vector(point2.X, point2.Y);
            return Vector.AngleBetween(new Vector(1, 0), Vector.Subtract(vector2, vector1));
        }

        /// <summary>
        /// 获得三个点中其中一个点与另外两个点连线夹角
        /// </summary>
        /// <param name="singlePoint"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double GetThreePointsTwoLineAngle(Point singlePoint, Point point1, Point point2)
        {
            var vectorSinglePoint = new Vector(singlePoint.X, singlePoint.Y);
            var vectorPoint1 = new Vector(point1.X, point1.Y);
            var vectorPoint2 = new Vector(point2.X, point2.Y);
            var vector1 = Vector.Subtract(vectorPoint1, vectorSinglePoint);
            var vector2 = Vector.Subtract(vectorPoint2, vectorSinglePoint);
            return Vector.AngleBetween(vector1, vector2);
        }

        /// <summary>
        /// 获得一系列点两两连成的线段与X轴夹角的集合
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<double> GetPointsLineVectorAngle(IList<Point> points)
        {
            var angles = new List<double>();
            if (points?.Count > 1)
            {
                var count = points.Count;
                for (var i = 0; i < count - 1; ++i)
                {
                    angles.Add(GetTwoPointLineAngle(points[i], points[i + 1]));
                }
                angles.Add(GetTwoPointLineAngle(points[count - 1], points[0]));
            }
            return angles;
        }

        /// <summary>
        /// 获得当前坐标到所有航路点的距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 判断点是否在多边形内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
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
                    oddNodes ^= (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * 
                        (polyX[j] - polyX[i]) < x);
                }
                j = i;
            }
            return oddNodes;
        }

    }
}
