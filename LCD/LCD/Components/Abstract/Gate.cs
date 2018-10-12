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
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using LCD.Properties;

namespace LCD.Components.Abstract
{
    [Serializable]
    
    public abstract class Gate
    {
        private Point _location;
        private float _angle;
        [Browsable(false)]
        public Size Size { get; set; }
        [Browsable(true), Description("The angle of this gate")]
        public float Angle 
        {
            get
            {
                
                return _angle;
            }
            set
            {

                _angle = value % 360;

                if (_angle < 0)
                {
                    _angle = 360 + _angle;
                }
            }
        }
        [Browsable(true), Description("Describe what is the purpose of this gate")]
        public String Description { get; set; }


        [Browsable(false)]
        public bool Selected { get; set; }

        [Browsable(false)]
        public int Width
        {
            get
            {
                return Size.Width;
            }
        }

        [Browsable(false)]
        public int Height
        {
            get
            {
                return Size.Height;
            }
        }

        protected Point LastLocation { get; set; }

        [Browsable(false)]
        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                LastLocation = _location;

                _location = value;
            }
        }

        [Browsable(false)]
        public int X
        {
            get
            {
                return Location.X;
            }
            set
            {
                Location = new Point(value, Location.Y);
            }
        }

        [Browsable(false)]
        public int Y
        {
            get
            {
                return Location.Y;
            }
            set
            {
                Location = new Point(Location.X, value);
            }
        }

        public virtual void Simulate() { }
        public virtual void Draw(Graphics g) { }
        public virtual void Reset() { }
//         public virtual void MouseDown() { }
//         public virtual void MouseUp() { }
        public virtual void MouseDown(MouseEventArgs e) { }
        public virtual void MouseUp(MouseEventArgs e) { }

        public virtual Dot DotOn(Point p)
        {
            return null;
        }
        public override string ToString()
        {
            return "Gate (" + Location.X + "," + Location.Y + ")";
        }

        private Rectangle rectangle=default(Rectangle), vicinity=default(Rectangle);

        private void UpdateRectangles()
        {
            UpdateGateRectangle();
            UpdateVicinity();
        }

        private void UpdateGateRectangle()
        {
            //Update the Gate rectangle

            if (Angle > 0 && Angle <= 90)
            {
                rectangle.X = Location.X - Height;
                rectangle.Y = Location.Y;
                rectangle.Width = Size.Height;
                rectangle.Height = Size.Width;
            }

            if (Angle > 90 && Angle <= 180)
            {
                rectangle.X = Location.X - Width;
                rectangle.Y = Location.Y - Height;
                rectangle.Width = Size.Width;
                rectangle.Height = Size.Height;
            }

            if (Angle > 180 && Angle <= 270)
            {
                rectangle.X = Location.X;
                rectangle.Y = Location.Y - Width;
                rectangle.Width = Size.Height;
                rectangle.Height = Size.Width;
            }

            if (Angle > 270 && Angle <= 360 || Angle == 0)
            {
                rectangle.X = Location.X;
                rectangle.Y = Location.Y;
                rectangle.Width = Size.Width;
                rectangle.Height = Size.Height;
            }
        }

        private void UpdateVicinity()
        {
            //Update the Gate vicinity

            if (Width > Height)
            {
                vicinity.X = X - Width;
                vicinity.Y = Y - Width;
                vicinity.Width = 2 * Width;
                vicinity.Height = 2 * Width;
            }
            else
            {
                vicinity.X = X - Height;
                vicinity.Y = Y - Height;
                vicinity.Width = 2 * Height;
                vicinity.Height = 2 * Height;
            }
        }


        public Rectangle GetRectangle()
        {
            if(rectangle==default(Rectangle))
            {
                UpdateGateRectangle();
            }

            return rectangle;
        }

        public virtual Rectangle GateVicinity()
        {
            if (vicinity == default(Rectangle))
            {
                UpdateVicinity();
            }

            return vicinity;
        }

        public virtual void IncreaseNumberOfInputs()
        {

        }

        public virtual void DecreaseNumberOfInputs()
        {

        }

        public virtual Point[] GetDotPoints()
        {
            return new Point[0];
        }

        public virtual void Clear(Graphics g)
        {

        }

        public virtual void Move(int xOffset, int yOffset, Graphics g)
        {
            Point p = new Point(
                Location.X + xOffset,
                Location.Y + yOffset);

            Clear(g);

            Location = p;

            Draw(g);

            UpdateRectangles();
        }

        public virtual void Move(Point location, Graphics g)
        {
            Clear(g);

            Location = location;

            Draw(g);

            UpdateRectangles();
        }

        public virtual Point GetPointIfMoving(int xOffset, int yOffset)
        {
            Point p = new Point(
                Location.X + xOffset,
                Location.Y + yOffset);

            return p;
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

        protected virtual void SetDotValue(Dot d)
        {
            d.Value = false;

            foreach (Wire w in d.connectedWires)
            {
                if (w.GetOtherDot(d).Value)
                {
                    d.Value = true;
                }
                
            }
        }
    }
}
