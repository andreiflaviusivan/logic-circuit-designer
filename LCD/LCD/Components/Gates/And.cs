﻿/*This file is part of Logic Circuit Designer.

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
    public class And : BasicGate
    {
        

        public override void Simulate()
        {
            
            foreach (Dot d in inputs)
            {
                /*
                if (d.w != null)
                {
                    d.Value = d.w.GetOtherDot(d).Value;
                }
                */

                SetDotValue(d);
            }
            
            
            bool o=true;

            foreach (Dot d in inputs)
            {
                o = o & d.Value;
            }

            output.Value = o;
        }

        public And(int numberOfInputs, Point location):base(numberOfInputs,location)
        {

        }

        public override void Draw(Graphics g)
        {
            int w, h, x, y;
            int n = inputs.Count;
            x = Location.X; y = Location.Y;
            w = Width; h = Height;
            
            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            for (int i = 1; i <= n; i++)
                g.DrawLine(Pens.Black, new Point(0, 2 * i * 6 - 3), new Point(10, 2 * i * 6 - 3));

            g.DrawLine(Pens.Black, new Point(w - 21, h / 2 - 0), new Point(w - 1, h / 2 - 0));

            g.DrawLine(Pens.Black, new Point(10, 0), new Point(10, h - 1));
            g.DrawArc(Pens.Black, new Rectangle(-7, 0, w - 15, h - 1), -90, 180);

            foreach (Dot d in inputs)
                d.Draw(g);
            output.Draw(g);
            if (Selected)
                g.DrawRectangle(Pens.Blue, new Rectangle(0,0,w,h));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
        }

        public override void Clear(Graphics g)
        {
            int w, h, x, y;
            int n = inputs.Count;
            x = Location.X; y = Location.Y;
            w = Width; h = Height;

            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            Pen pen = new Pen(Settings.Default.CircuitBackColor);

            for (int i = 1; i <= n; i++)
                g.DrawLine(pen, new Point(0, 2 * i * 6 - 3), new Point(10, 2 * i * 6 - 3));

            g.DrawLine(pen, new Point(w - 21, h / 2 - 0), new Point(w - 1, h / 2 - 0));

            g.DrawLine(pen, new Point(10, 0), new Point(10, h - 1));
            g.DrawArc(pen, new Rectangle(-7, 0, w - 15, h - 1), -90, 180);

            foreach (Dot d in inputs)
                d.Clear(g);
            output.Clear(g);
            if (Selected)
                g.DrawRectangle(pen, new Rectangle(0, 0, w, h));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
            
            base.Clear(g);
        }
                
        public override string ToString()
        {
            return "And (" + Location.X + "," + Location.Y + ")";
        }

    }
}
