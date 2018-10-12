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

namespace LCD.Components
{
    [Serializable]
    public class Dot
    {
        public bool Value { get; set; }
        public Point Location { get; set; }
        public LCD.Components.Abstract.Gate Parent { get; set; }
        
        public Wire w { get; set; }
        public List<Wire> connectedWires = new List<Wire>();

        public String Description
        {
            get;
            set;
        }

        public Dot()
        {
            Parent = null;
        }

        public Dot(bool value):this()
        {
            this.Value = value;
        }

        public Dot(Point location,LCD.Components.Abstract.Gate parent):this(false)
        {
            Location = location;
            Parent = parent;
        }

        public void Draw(Graphics g)
        {
            Pen onPen = new Pen(Settings.Default.DotOnColor);
            Pen offPen = new Pen(Settings.Default.DotOffColor);

            g.DrawEllipse(
                Value == true ? onPen : offPen,
                new Rectangle(Location.X - Settings.Default.DotRadius / 2,
                    Location.Y - Settings.Default.DotRadius / 2,
                    Settings.Default.DotRadius,
                    Settings.Default.DotRadius));

            SolidBrush offBrush = new SolidBrush(Settings.Default.DotOffColor);
            SolidBrush onBrush = new SolidBrush(Settings.Default.DotOnColor);

            g.FillEllipse(Value == true ? onBrush : offBrush,
                new Rectangle(Location.X - Settings.Default.DotRadius / 2,
                    Location.Y - Settings.Default.DotRadius / 2,
                    Settings.Default.DotRadius,
                    Settings.Default.DotRadius));
        }

        public void Clear(Graphics g)
        {
            Pen pen = new Pen(Settings.Default.CircuitBackColor);

            g.DrawEllipse(
                pen,
                new Rectangle(Location.X - Settings.Default.DotRadius / 2,
                    Location.Y - Settings.Default.DotRadius / 2,
                    Settings.Default.DotRadius,
                    Settings.Default.DotRadius));

            g.FillEllipse(new SolidBrush(Settings.Default.CircuitBackColor),
                new Rectangle(Location.X - Settings.Default.DotRadius / 2,
                    Location.Y - Settings.Default.DotRadius / 2,
                    Settings.Default.DotRadius,
                    Settings.Default.DotRadius));
        }
    }
}
