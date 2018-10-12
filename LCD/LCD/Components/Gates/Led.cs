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
using Settings = LCD.Properties.Settings;
using LCD.Components.Abstract;

namespace LCD.Components.Abstract
{
    [Serializable]
    public class Led:Gate
    {
        private Dot input;

        public Led()
        {
            Size = new Size(30, 30);

            input = new Dot(new Point(-10, 15),this);
        }

        public override void Simulate()
        {
            /*
            if (input.w != null)
                input.Value = input.w.GetOtherDot(input).Value;
             */

            SetDotValue(input);
        }

        public Led(Point location) : this()
        {
            Location = location;
        }

        public override void Draw(Graphics g)
        {
            int x, y, w, h;
            x = Location.X; y=Location.Y;
            w = Size.Width; h = Size.Height;
           
            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            g.DrawEllipse(
                Selected ? Pens.Blue : Pens.Black,
                new Rectangle(0, 0, w, h));

            g.FillEllipse(input.Value ? Brushes.Yellow : Brushes.Black,
                new Rectangle(2, 2, w - 4, h - 4));
            g.DrawLine(Pens.Black,
                new Point(- 10, 15), new Point(0, 15));
            input.Draw(g);

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
        }

        public override void Clear(Graphics g)
        {
            int x, y, w, h;
            x = Location.X; y = Location.Y;
            w = Size.Width; h = Size.Height;

            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            Pen pen = new Pen(Settings.Default.CircuitBackColor);

            g.DrawEllipse(
                pen,
                new Rectangle(0, 0, w, h));

            g.FillEllipse(new SolidBrush(Settings.Default.CircuitBackColor),
                new Rectangle(2, 2, w - 4, h - 4));
            g.DrawLine(pen,
                new Point(-10, 15), new Point(0, 15));
            input.Clear(g);

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
            
            base.Clear(g);
        }

        public bool GetValue()
        {
            return input.Value;
        }

        public override Dot DotOn(Point p)
        {
            if (Math.Abs(p.X - input.Location.X) <= Settings.Default.DotRadius && Math.Abs(p.Y - input.Location.Y) <= Settings.Default.DotRadius)
                return input;
            return null;
        }

        public override string ToString()
        {
            return "Led (" + Location.X + ',' + Location.Y + ")";
        }

        public override Point[] GetDotPoints()
        {
            Point p = new Point(
                input.Location.X + this.Location.X,
                input.Location.Y + this.Location.Y);

            Point[] array = new Point[1];
            array[0] = p;

            return array;
        }
    }
}
