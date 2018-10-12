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
using System.Windows.Forms;
using System.Drawing;

namespace LCD.Interface
{
    public class NoMousewheelTabPage : TabPage
    {
        private const int WM_MOUSEWHEEL = 0x20a;
        
        private Point startPanPosition = default(Point);
        
        public NoMousewheelTabPage(string title) : base(title)
        {
            
        }

        protected override void WndProc(ref Message m)
        {
            // ignore WM_MOUSEWHEEL events
            if (m.Msg == WM_MOUSEWHEEL)
            {
                return;
            }

            base.WndProc(ref m);
        }

        
        protected  void OnMouseMove(object sender,MouseEventArgs e)
        {
            

            if (startPanPosition != default(Point) && sender is Control)
            {
                int hValue, vValue;

                Control control = (Control)sender;

                Point location = new Point(
                    control.Left + e.X,
                    control.Top + e.Y);

                hValue = HorizontalScroll.Value;
                vValue = VerticalScroll.Value;

                hValue -= location.X - startPanPosition.X;
                vValue -= location.Y - startPanPosition.Y;

                if (hValue >= HorizontalScroll.Minimum && hValue <= HorizontalScroll.Maximum)
                {
                    HorizontalScroll.Value = hValue;
                }

                if (vValue >= VerticalScroll.Minimum && vValue <= VerticalScroll.Maximum)
                {
                    VerticalScroll.Value = vValue;
                }

                startPanPosition = location;
            }

            base.OnMouseMove(e);
        }

        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && sender is Control)
            {
                Control control = (Control)sender;

                startPanPosition = new Point(
                    control.Left + e.X,
                    control.Top + e.Y);

                Cursor = Cursors.NoMove2D;
            }
                        
            base.OnMouseDown(e);
        }

        protected void OnMouseUp(object sender, MouseEventArgs e)
        {
            startPanPosition = default(Point);

            Cursor = Cursors.Default;
            
            base.OnMouseUp(e);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.MouseMove+=new MouseEventHandler(OnMouseMove);
            e.Control.MouseUp+=new MouseEventHandler(OnMouseUp);
            e.Control.MouseDown+=new MouseEventHandler(OnMouseDown);
            
            base.OnControlAdded(e);
        }
    }
}
