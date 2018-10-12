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
    public class Not : BasicGate
    {
        
        public override void Simulate()
        {
            /*
            if (inputs[0].w != null)
                inputs[0].Value = inputs[0].w.GetOtherDot(inputs[0]).Value;
             */

            SetDotValue(inputs[0]);

            output.Value = !inputs[0].Value;
        }

        public override void Draw(Graphics g)
        {
            int w, h, x, y;
            x = Location.X; y = Location.Y;
            w = Width; h = Height;

            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            g.DrawLine(Pens.Black, new Point(w-13, h / 2 - 0), new Point(w, h / 2 - 0));
            g.DrawLine(Pens.Black, new Point(0, h / 2), new Point(10, h / 2));

            Point[] vp = new Point[3];
            vp[0].X = 10; vp[0].Y = 0;
            vp[1].X = 10; vp[1].Y = h;
            vp[2].X = w - 20; vp[2].Y = h / 2;
            g.DrawPolygon(Pens.Black, vp);

            g.DrawEllipse(Pens.Black, new Rectangle(w - 17 - 3, h / 2 - 3, 6, 6));
            inputs[0].Draw(g);
            output.Draw(g);

            if (Selected)
                g.DrawRectangle(Pens.Blue, new Rectangle(0, 0, w, h));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
        }

        public override void Clear(Graphics g)
        {
            Pen pen = new Pen(Settings.Default.CircuitBackColor);

            int w, h, x, y;
            x = Location.X; y = Location.Y;
            w = Width; h = Height;

            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            g.DrawLine(pen, new Point(w - 13, h / 2 - 0), new Point(w, h / 2 - 0));
            g.DrawLine(pen, new Point(0, h / 2), new Point(10, h / 2));

            Point[] vp = new Point[3];
            vp[0].X = 10; vp[0].Y = 0;
            vp[1].X = 10; vp[1].Y = h;
            vp[2].X = w - 20; vp[2].Y = h / 2;
            g.DrawPolygon(pen, vp);

            g.DrawEllipse(pen, new Rectangle(w - 17 - 3, h / 2 - 3, 6, 6));
            inputs[0].Clear(g);
            output.Clear(g);

            if (Selected)
                g.DrawRectangle(pen, new Rectangle(0, 0, w, h));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
            
            base.Clear(g);
        }

        public Not(Point location):base(1,location)
        {
            DisableInputChanging = true;
        }

        public override string ToString()
        {
            return "Not (" + Location.X + "," + Location.Y + ")";
        }
    }
}
