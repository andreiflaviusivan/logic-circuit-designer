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
using System.ComponentModel;
using Settings = LCD.Properties.Settings;
using LCD.Components.Abstract;

namespace LCD.Components.Abstract
{
    [Serializable]
    public class Btn : Gate
    {
        private Dot output;

        [Browsable(true),Description("The type of this button")]
        public ButtonTypes ButtonType { get; set; }

        private int ClickDownCount;
        private bool mouseDown;

        public override void Draw(System.Drawing.Graphics g)
        {
            int x, y, w, h;
            w = 30; h = 30;
            x = Location.X; y=Location.Y;
            Size = new Size(w, h);
            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, w, h));
            g.FillEllipse(output.Value == true ? Brushes.LimeGreen : Brushes.Red, new Rectangle(2, 2, w - 4, h - 4));
            g.DrawLine(Pens.Black, new Point(w, h / 2), new Point(w + 10, h / 2));
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
            g.FillEllipse(new SolidBrush(Settings.Default.CircuitBackColor), new Rectangle(2, 2, w - 4, h - 4));
            g.DrawLine(pen, new Point(w, h / 2), new Point(w + 10, h / 2));
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

        public override void MouseDown(MouseEventArgs e)
        {
            if (ButtonType == ButtonTypes.PushButton)
            {
                ClickDownCount = 2;
                output.Value = true;
                mouseDown = true;
            }
            else
            {
                output.Value = !output.Value;
            }
        }

        public override void Simulate()
        {
            if(ButtonType==ButtonTypes.PushButton)
            {
                if (ClickDownCount > 0)
                    ClickDownCount--;
                if (ClickDownCount == 0 && !mouseDown)
                    output.Value = false;
            }
        }

        public override void MouseUp(MouseEventArgs e)
        {
            mouseDown = false;
        }

        public Btn(Point loc)
        {
            Location = loc;
            output = new Dot(new Point(40,15),this);
            ButtonType = ButtonTypes.ToggleButton;
        }

        public override Dot DotOn(Point p)
        {
            if ((Math.Abs(p.X - (Size.Width + 10)) <= Settings.Default.DotRadius) && (Math.Abs(p.Y - (Size.Height / 2)) <= Settings.Default.DotRadius))
                return output;
            return null;
        }

        public void SetValue(bool val)
        {
            output.Value=val;
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

    public enum ButtonTypes
    {
        ToggleButton = 1,
        PushButton = 2
    }
}
