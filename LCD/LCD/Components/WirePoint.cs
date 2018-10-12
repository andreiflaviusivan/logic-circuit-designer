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

namespace LCD.Components
{
    [Serializable]
    public class WirePoint
    {
        private Point _location;
        public int X
        {
            get
            {
                return Location.X;
            }
            
        }

        public int Y
        {
            get
            {
                return Location.Y;
            }
            
        }

        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        private Wire parent;

        public WirePoint(Wire parent)
        {
            this.parent = parent;
        }
        public WirePoint(int x,int y,Wire parent):this(parent)
        {
            Location = new Point(x, y);
        }
        public WirePoint(Point p, Wire parent)
            : this(parent)
        {
            Location = p;
        }

        public bool PointOn(Point p)
        {
            if (Math.Abs(p.X - Location.X) <= Properties.Settings.Default.WirePointRadius && Math.Abs(p.Y - Location.Y) <= Properties.Settings.Default.WirePointRadius)
                return true;
            return false;
        }

        public void Move(int xOffset, int yOffset, Graphics g)
        {
            Point p = new Point(
                Location.X + xOffset,
                Location.Y + yOffset);

            parent.Clear(g);

            Location = p;

            parent.Draw(g);
        }

        public void Move(Point location, Graphics g)
        {
            if (parent != null)
            {
                parent.Clear(g);
            }

            Location = location;

            if (parent != null)
            {
                parent.Draw(g);
            }
        }

        public override string ToString()
        {
            return Location.ToString();
        }

        public Wire Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        private const int vicinitySize = 40;

        private Rectangle vicinity = default(Rectangle);

        private void UpdateVicinity()
        {
            vicinity.X = X - vicinitySize / 2;
            vicinity.Y = Y - vicinitySize / 2;
            vicinity.Width = vicinitySize;
            vicinity.Height = vicinitySize;
        }

        public Rectangle GetVicinity()
        {
            if (vicinity == default(Rectangle))
            {
                UpdateVicinity();
            }

            return vicinity;
        }
    }
}
