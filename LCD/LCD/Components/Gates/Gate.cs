using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace LCD.Components.Gates
{
    [Serializable]
    
    public abstract class Gate
    {
        public Point Location;
        [Browsable(false)]
        public Size Size { get; set; }
        [Browsable(true), Description("The angle of this gate")]
        public float Angle { get; set; }
        [Browsable(true), Description("Describe what is the purpose of this gate")]
        public String Description { get; set; }
        [Browsable(false)]
        public bool Selected { get; set; }

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

        public Rectangle GetRectangle()
        {
            return new Rectangle(Location, Size);
        }
    }
}
