/*This file is part of Logic Circuit Designer.

    Logic Circuit Designer is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Logic Circuit Designer is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Logic Circuit Designer.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LCD.Interface.Additional
{
    public abstract class LinePointUtils
    {
        

        //Distance between 2 points
        public static double GetDistanceBetween(Point p1, Point p2)
        {
            double sum1 = Math.Pow(p2.X - p1.X, 2);
            double sum2 = Math.Pow(p2.Y - p1.Y, 2);

            return Math.Sqrt(sum1 + sum2);
        }

        //Get the slope of the line
        public static double GetSlope(Point p1, Point p2)
        {
            if (p2.X - p1.X == 0)
            {
                return double.PositiveInfinity;
            }

            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

        public static Point[] GetMiddlePoints(Point p1, Point p2, int distanceSeek)
        {
            return GetMiddlePoints(p1, p2, GetDistanceBetween(p1, p2), distanceSeek);
        }

        private static Point[] GetMiddlePoints(Point p1, Point p2, double distance, int distanceSeek)
        {
            if (distance > 0)
            {
                List<Point> pointList = new List<Point>();

                Point middlePoint = GetMiddlePoint(p1, p2);

                pointList.Add(middlePoint);

                Point[] firstVector = GetMiddlePoints(p1, middlePoint, distance - distanceSeek,distanceSeek);
                Point[] secondVector = GetMiddlePoints(middlePoint, p2, distance - distanceSeek,distanceSeek);

                if (firstVector != null && firstVector.Length != 0)
                {
                    pointList.AddRange(firstVector);
                }

                if (secondVector != null && secondVector.Length != 0)
                {
                    pointList.AddRange(secondVector);
                }

                return pointList.ToArray();
            }

            return null;
        }

        public static Rectangle[] GetMiddlePointsVicinities(Point p1, Point p2, int distanceSeek, int rectSize)
        {
            return GetMiddlePointsVicinities(p1, p2, GetDistanceBetween(p1, p2), distanceSeek, rectSize);
        }

        private static Rectangle[] GetMiddlePointsVicinities(Point p1, Point p2, double distance, int distanceSeek,int rectSize)
        {
            if (distance > 0)
            {
                List<Point> pointList = new List<Point>();

                Point middlePoint = GetMiddlePoint(p1, p2);

                pointList.Add(middlePoint);

                Point[] firstVector = GetMiddlePoints(p1, middlePoint, distance - distanceSeek, distanceSeek);
                Point[] secondVector = GetMiddlePoints(middlePoint, p2, distance - distanceSeek, distanceSeek);

                if (firstVector != null && firstVector.Length != 0)
                {
                    pointList.AddRange(firstVector);
                }

                if (secondVector != null && secondVector.Length != 0)
                {
                    pointList.AddRange(secondVector);
                }

                return GetRectanglesFromEnumerable(pointList, rectSize);
            }

            return null;
        }

        private static Rectangle[] GetRectanglesFromEnumerable(IEnumerable<Point> list,int rectSize)
        {
            List<Rectangle> rectList = new List<Rectangle>();

            foreach (Point p in list)
            {
                Rectangle r = new Rectangle(
                    p.X - rectSize / 2,
                    p.Y - rectSize / 2,
                    rectSize,
                    rectSize);

                rectList.Add(r);
            }

            return rectList.ToArray();
        }

        public static Point GetMiddlePoint(Point p1, Point p2)
        {
            return new Point(
                (p1.X + p2.X) / 2,
                (p1.Y + p2.Y) / 2);
        }

        public static Rectangle GetRectangleWithCorners(Point p1, Point p2)
        {
            int width, height;

            width = p2.X - p1.X;
            height = p2.Y - p1.Y;

            Size size = new Size(
                    Math.Abs(width),
                    Math.Abs(height));

            Point startPoint = p1;

            if (width > 0 && height < 0)
            {
                startPoint = new Point(
                    p1.X,
                    p2.Y);
            }

            if (width < 0 && height > 0)
            {
                startPoint = new Point(
                    p2.X,
                    p1.Y);
            }

            if (width < 0 && height < 0)
            {
                startPoint = p2;
            }

            Rectangle rect = new Rectangle(
                startPoint,
                size);

            return rect;
        }

        public static Point RotatePoint(Point input, Point reference, double angle)
        {
            Point ret = new Point();
            double cos, sin;
            cos = Math.Cos(angle * Math.PI / 180);
            sin = Math.Sin(angle * Math.PI / 180);
            ret.X = (int)(cos * (input.X - reference.X) - sin * (input.Y - reference.Y) + reference.X);
            ret.Y = (int)(sin * (input.X - reference.X) + cos * (input.Y - reference.Y) + reference.Y);
            return ret;
        }
    }
}
