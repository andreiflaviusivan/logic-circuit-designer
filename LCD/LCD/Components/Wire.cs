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
using System.Windows.Forms;
using LCD.Components.Abstract;
using LCD.Properties;
using LCD.Interface.Additional;

namespace LCD.Components
{
    [Serializable]
    public class Wire
    {
        public Dot src { get; set; }
        public Dot dst { get; set; }
        public bool Selected { get; set; }
        public List<WirePoint> Points { get; set; }

                
        public Wire()
        {
            Points = new List<WirePoint>();
        }

        public Dot GetOtherDot(Dot d)
        {
            if (d == src)
                return dst;
            if (d == dst)
                return src;
            return null;
        }

        private Point RotatePoint(Point input, Point reference, double angle)
        {
            Point ret = new Point();
            double cos, sin;
            cos = Math.Cos(angle * Math.PI / 180);
            sin = Math.Sin(angle * Math.PI / 180);
            ret.X = (int)(cos * (input.X - reference.X) - sin * (input.Y - reference.Y) + reference.X);
            ret.Y = (int)(sin * (input.X - reference.X) + cos * (input.Y - reference.Y) + reference.Y);
            return ret;
        }

        public void Draw(Graphics g)
        {
            Point a, b;
            a = new Point(src.Location.X + src.Parent.Location.X, src.Location.Y + src.Parent.Location.Y);
            b = new Point(dst.Location.X + dst.Parent.Location.X, dst.Location.Y + dst.Parent.Location.Y);
            a = RotatePoint(a, src.Parent.Location, src.Parent.Angle);
            b = RotatePoint(b, dst.Parent.Location, dst.Parent.Angle);

            Pen pen;
            if (dst.Value || src.Value)
            {
                pen = new Pen(Settings.Default.WireOnColor);
            }
            else
            {
                pen = new Pen(Settings.Default.WireOffColor);
            }

            pen.Width = this.Selected ? 2 : 1;

            Point lastPoint = a;

            foreach (WirePoint currentPoint in Points)
            {
                g.DrawLine(pen, lastPoint, currentPoint.Location);
                Pen penWirePoint = new Pen(Settings.Default.WirePointColor);

                g.DrawEllipse(
                    penWirePoint,
                    currentPoint.X - Settings.Default.WirePointRadius / 2,
                    currentPoint.Y - Settings.Default.WirePointRadius / 2,
                    Settings.Default.WirePointRadius,
                    Settings.Default.WirePointRadius);

                SolidBrush brushWirePoint = new SolidBrush(Settings.Default.WirePointColor);

                g.FillEllipse(
                    brushWirePoint,
                    currentPoint.X - Settings.Default.WirePointRadius / 2,
                    currentPoint.Y - Settings.Default.WirePointRadius / 2,
                    Settings.Default.WirePointRadius,
                    Settings.Default.WirePointRadius);

                lastPoint = currentPoint.Location;
            }
            g.DrawLine(pen, lastPoint, b);
        }

        private bool move = true;

        public void MoveWirePoints(int xOffset, int yOffset,Graphics g)
        {
            //This is intended to block the move if both Gates are selected
            //because otherwise the WirePoints will move twice whenever the gates are moved
            if (src.Parent.Selected && dst.Parent.Selected)
            {
                if (!move)
                {
                    move = true;

                    return;
                }

                move = false;
            }
            else
            {
                move = true;
            }

            Clear(g);

            foreach (WirePoint wirePoint in Points)
            {
                int xLocation = wirePoint.Location.X + xOffset;
                int yLocation = wirePoint.Location.Y + yOffset;

                if (xLocation <= 0 && yLocation > 0)
                {
                    wirePoint.Location = new Point(
                        wirePoint.Location.X,
                        yLocation);
                }

                if (yLocation <= 0 && xLocation > 0)
                {
                    wirePoint.Location = new Point(
                        xLocation,
                        wirePoint.Location.Y);
                }

                if (xLocation > 0 && yLocation > 0)
                {
                    wirePoint.Location = new Point(
                        xLocation,
                        yLocation);
                }
            }

            Draw(g);
        }

        public Point[] GetWirePointsLocation()
        {
            List<Point> pointList = new List<Point>();

            foreach (WirePoint w in Points)
            {
                pointList.Add(w.Location);
            }

            return pointList.ToArray();
        }

        public void AddWirePoint(int index, Point location,Graphics g)
        {
            Clear(g);

            Points.Insert(index,new WirePoint(location,this));

            Draw(g);
        }

        public void UpdateWirePointsParent()
        {
            foreach (WirePoint wp in Points)
            {
                wp.Parent = this;
            }
        }

        public virtual void Clear(Graphics g)
        {
            Point a, b;
            a = new Point(src.Location.X + src.Parent.Location.X, src.Location.Y + src.Parent.Location.Y);
            b = new Point(dst.Location.X + dst.Parent.Location.X, dst.Location.Y + dst.Parent.Location.Y);
            a = RotatePoint(a, src.Parent.Location, src.Parent.Angle);
            b = RotatePoint(b, dst.Parent.Location, dst.Parent.Angle);

            Pen pen=new Pen(Settings.Default.CircuitBackColor);

            pen.Width = this.Selected ? 2 : 1;
            
            Point lastPoint = a;

            foreach (WirePoint currentPoint in Points)
            {
                g.DrawLine(pen, lastPoint, currentPoint.Location);
                
                g.DrawEllipse(
                    pen,
                    currentPoint.X - Settings.Default.WirePointRadius / 2,
                    currentPoint.Y - Settings.Default.WirePointRadius / 2,
                    Settings.Default.WirePointRadius,
                    Settings.Default.WirePointRadius);

                SolidBrush brushWirePoint = new SolidBrush(Settings.Default.CircuitBackColor);

                g.FillEllipse(
                    brushWirePoint,
                    currentPoint.X - Settings.Default.WirePointRadius / 2,
                    currentPoint.Y - Settings.Default.WirePointRadius / 2,
                    Settings.Default.WirePointRadius,
                    Settings.Default.WirePointRadius);

                lastPoint = currentPoint.Location;
            }
            g.DrawLine(pen, lastPoint, b);
        }

        public void Select(Graphics g)
        {
            Clear(g);

            Selected = true;

            Draw(g);
        }

        public void DeSelect(Graphics g)
        {
            Clear(g);

            Selected = false;

            Draw(g);
        }

        public void InvertSelect(Graphics g)
        {
            Clear(g);

            Selected = !Selected;

            Draw(g);
        }

        #region Line Points Methods

        private const int DISTANCE_SEEK = 300;
        private const int RECT_SIZE = 40;

        //Gets a certain amount of points from the line
        //according to the distance between the points
        public Point[] GetLinePoints()
        {
            List<Point> pointList = new List<Point>();

            Point firstPoint = new Point(
                src.Location.X + src.Parent.Location.X,
                src.Location.Y + src.Parent.Location.Y);

            firstPoint = LinePointUtils.RotatePoint(
                firstPoint,
                src.Parent.Location,
                -src.Parent.Angle);

            Point currentPoint=firstPoint;
            
            foreach (WirePoint wirePoint in Points)
            {
                currentPoint = wirePoint.Location;

                Point[] aux = LinePointUtils.GetMiddlePoints(firstPoint, currentPoint, DISTANCE_SEEK);
                
                if (aux != null)
                {
                    pointList.AddRange(aux);
                }
                pointList.Add(firstPoint);

                firstPoint = currentPoint;
            }

            Point lastPoint = new Point(
                dst.Location.X + dst.Parent.Location.X,
                dst.Location.Y + dst.Parent.Location.Y);

            lastPoint = LinePointUtils.RotatePoint(
                lastPoint,
                dst.Parent.Location,
                -dst.Parent.Angle);


            Point[] array = LinePointUtils.GetMiddlePoints(firstPoint, lastPoint, DISTANCE_SEEK);

            if (array != null)
            {
                pointList.AddRange(array);
            }

            return pointList.ToArray();
        }

        //Avoid using this. Consumes lots of resources!!
        public Rectangle[] GetLinePointsVicinities()
        {
            List<Rectangle> rectList = new List<Rectangle>();

            Point firstPoint = new Point(
                src.Location.X + src.Parent.Location.X,
                src.Location.Y + src.Parent.Location.Y);

            Point currentPoint = firstPoint;
            
            foreach (WirePoint wirePoint in Points)
            {
                currentPoint = wirePoint.Location;

                Rectangle[] aux = LinePointUtils.GetMiddlePointsVicinities(firstPoint, currentPoint, DISTANCE_SEEK,RECT_SIZE);

                if (aux != null)
                {
                    rectList.AddRange(aux);
                }

                firstPoint = currentPoint;
            }

            Point lastPoint = new Point(
                dst.Location.X + dst.Parent.Location.X,
                dst.Location.Y + dst.Parent.Location.Y);

            Rectangle[] array = LinePointUtils.GetMiddlePointsVicinities(firstPoint, lastPoint, DISTANCE_SEEK,RECT_SIZE);

            if (array != null)
            {
                rectList.AddRange(array);
            }

            return rectList.ToArray();
        }

        //This was optimized. It returns no more than 2 Rectangles
        //representing the surface that needs to be redrawn 
        //when the respective wirePoint is moved
        public Rectangle[] GetLinePointsVicinities(WirePoint wirePoint)
        {
            List<Rectangle> rectList = new List<Rectangle>();

            Point firstPoint = new Point(
                       src.Location.X + src.Parent.Location.X,
                       src.Location.Y + src.Parent.Location.Y);
            Point lastPoint = new Point(
                        dst.Location.X + dst.Parent.Location.X,
                        dst.Location.Y + dst.Parent.Location.Y); 

            int index = Points.IndexOf(wirePoint);

            if (index != -1)
            {
                Rectangle rect1;
                Rectangle rect2;

                if (index > 0 && index < Points.Count - 1)
                {
                    WirePoint point1 = Points[index - 1];
                    WirePoint point2 = Points[index + 1];

                    rect1 = LinePointUtils.GetRectangleWithCorners(
                        point1.Location,
                        wirePoint.Location);

                    rect2 = LinePointUtils.GetRectangleWithCorners(
                        point2.Location,
                        wirePoint.Location);
                }
                else
                {
                    rect1 = LinePointUtils.GetRectangleWithCorners(
                        firstPoint,
                        wirePoint.Location);

                    rect2 = LinePointUtils.GetRectangleWithCorners(
                        lastPoint,
                        wirePoint.Location);
                }

                if (rect1 != null)
                {
                    rectList.Add(rect1);
                }

                if (rect2 != null)
                {
                    rectList.Add(rect2);
                }
            }

            return rectList.ToArray();
        }

        //This was optimized. It returns only a rectangle who's corners
        //are the gate location and the nearest Wire Point in the wire
        //If no wire points exist the opposite gate's location is used
        public Rectangle[] GetLinePointsVicinities(Gate gate)
        {
            List<Rectangle> rectList = new List<Rectangle>();

            Point firstPoint;
            Point lastPoint;

            if (gate == src.Parent)
            {
                lastPoint = new Point(
                        src.Location.X + src.Parent.Location.X,
                        src.Location.Y + src.Parent.Location.Y);

                if (Points.Count != 0)
                {
                    firstPoint = Points[0].Location;
                    
                }
                else
                {
                    firstPoint = new Point(
                        dst.Location.X + dst.Parent.Location.X,
                        dst.Location.Y + dst.Parent.Location.Y);
                    
                }

                Rectangle rect = LinePointUtils.GetRectangleWithCorners(
                        lastPoint,
                        firstPoint);

                if (rect != null)
                {
                    rectList.Add(rect);
                }   

            }

            if (gate == dst.Parent)
            {
                lastPoint = new Point(
                        dst.Location.X + dst.Parent.Location.X,
                        dst.Location.Y + dst.Parent.Location.Y);

                if (Points.Count != 0)
                {
                    firstPoint = Points[Points.Count-1].Location;

                }
                else
                {
                    firstPoint = new Point(
                        src.Location.X + src.Parent.Location.X,
                        src.Location.Y + src.Parent.Location.Y);

                }

                Rectangle rect = LinePointUtils.GetRectangleWithCorners(
                        lastPoint,
                        firstPoint);

                if (rect != null)
                {
                    rectList.Add(rect);
                }   
            }

            return rectList.ToArray();
        }

        #endregion
    }
}
