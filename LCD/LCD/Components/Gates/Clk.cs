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
using System.ComponentModel;
using System.Drawing;
using Settings = LCD.Properties.Settings;
using LCD.Components.Abstract;

namespace LCD.Components.Abstract
{
    [Serializable]
    public class Clk : Gate
    {
        private Dot output;

        [Browsable(true)]
        public int HighTime { get; set; }
        [Browsable(true)]
        public int LowTime { get; set; }

        private DateTime lastTime;

        public override void Draw(System.Drawing.Graphics g)
        {
            int x, y, w, h;
            w = 30; h = 30;
            x = Location.X; y=Location.Y;
            Size = new Size(w, h);
            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            //g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, w, h));
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, w, h));
            g.DrawLine(Pens.Black, new Point(w, h / 2), new Point(w + 10, h / 2));

            g.DrawLine(Pens.Black, w / 2, 4, w / 2, h / 2 - 3);
            g.DrawLine(Pens.Black, w / 2, h/2+3, w / 2, h - 4);
            g.DrawLine(Pens.Black, 4, h / 2, w / 2 - 3, h / 2);
            g.DrawLine(Pens.Black, w / 2 + 3, h / 2, w - 4, h / 2);

            g.DrawLine(Pens.Gray, 8, 8, w / 2 - 3, h / 2 - 3);
            g.DrawLine(Pens.Gray, w - 8, h - 8, w / 2 + 3, h / 2 + 3);
            g.DrawLine(Pens.Gray, 8, h-8, w / 2 - 3, h / 2 + 3);
            g.DrawLine(Pens.Gray, w - 8, 8, w / 2 + 3, h / 2 - 3);

            g.DrawEllipse(Pens.Black, new Rectangle(2, 2, w-4, h-4));

            output.Draw(g);
            if (Selected)
                g.DrawRectangle(Pens.Blue, new Rectangle(0,0, Size.Width,Size.Height));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
        }

        public override void Clear(Graphics g)
        {
            int x, y, w, h;
            w = 30; h = 30;
            x = Location.X; y = Location.Y;
            Size = new Size(w, h);
            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);


            Pen pen = new Pen(Settings.Default.CircuitBackColor);

            g.DrawRectangle(pen, new Rectangle(0, 0, w, h));
            g.DrawLine(pen, new Point(w, h / 2), new Point(w + 10, h / 2));

            g.DrawLine(pen, w / 2, 4, w / 2, h / 2 - 3);
            g.DrawLine(pen, w / 2, h / 2 + 3, w / 2, h - 4);
            g.DrawLine(pen, 4, h / 2, w / 2 - 3, h / 2);
            g.DrawLine(pen, w / 2 + 3, h / 2, w - 4, h / 2);

            g.DrawLine(pen, 8, 8, w / 2 - 3, h / 2 - 3);
            g.DrawLine(pen, w - 8, h - 8, w / 2 + 3, h / 2 + 3);
            g.DrawLine(pen, 8, h - 8, w / 2 - 3, h / 2 + 3);
            g.DrawLine(pen, w - 8, 8, w / 2 + 3, h / 2 - 3);

            g.DrawEllipse(pen, new Rectangle(2, 2, w - 4, h - 4));

            output.Clear(g);
            if (Selected)
                g.DrawRectangle(pen, new Rectangle(0, 0, Size.Width, Size.Height));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
            
            base.Clear(g);
        }

        public override void Reset()
        {
            output.Value = false;
        }

        public override void Simulate()
        {
            if (lastTime == null)
                lastTime = DateTime.Now;
            if(output.Value==false && DateTime.Now.Subtract(lastTime).TotalMilliseconds > LowTime)
            {
                output.Value = true;
                lastTime = DateTime.Now;
            }
            if (output.Value == true && DateTime.Now.Subtract(lastTime).TotalMilliseconds > HighTime)
            {
                output.Value = false;
                lastTime = DateTime.Now;
            }
        }

        public Clk(Point loc)
        {
            Location = loc;
            output = new Dot(new Point(40,15),this);
            HighTime = LowTime = 1000;
        }

        public override Dot DotOn(Point p)
        {
            if ((Math.Abs(p.X - (Size.Width + 10)) <= Settings.Default.DotRadius) && (Math.Abs(p.Y - (Size.Height / 2)) <= Settings.Default.DotRadius))
                return output;
            return null;
        }

        public override string ToString()
        {
            return "Btn (" + Location.X + "," + Location.Y + ")";
        }

        public override Point[] GetDotPoints()
        {
            Point p = new Point(
                output.Location.X + this.Location.X,
                output.Location.Y + this.Location.Y);

            Point[] array = new Point[1];
            array[0] = p;

            return array;
        }
    }
}
