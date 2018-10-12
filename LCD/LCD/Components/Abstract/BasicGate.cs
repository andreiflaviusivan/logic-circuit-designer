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
using LCD.Properties;
using System.ComponentModel;

namespace LCD.Components.Abstract
{
    [Serializable]
    public class BasicGate:Gate
    {
        protected List<Dot> inputs { get; set; }
        protected Dot output { get; set; }
        protected Boolean DisableInputChanging
        {
            get;
            set;
        }

        [Browsable(true), Description("Change the number of inputs of the basic gates except for the NOT gate.")]
        public int NumberOfInputs
        {
            get
            {
                return inputs.Count;
            }
            set
            {
                if (value < 2 )
                {
                    value = 2;
                }

                if (value > 8)
                {
                    value = 8;
                }

                while (NumberOfInputs < value && 
                    !DisableInputChanging)
                {
                    IncreaseNumberOfInputs();
                }

                while (NumberOfInputs > value &&
                    CanDecreaseNumberOfInputs && 
                    !DisableInputChanging)
                {
                    DecreaseNumberOfInputs();
                }
            }
        }

        private void ComputeGateSize(int numberOfInputs)
        {
            Size = new Size(50, 6 *(2 * numberOfInputs + 1));
        }

        private int GetInputYLocation(int inputIndex)
        {
            int result = 6 * 2 * (inputIndex + 1);

            return result;
        }

        private BasicGate(int ninputs)
        {
            inputs = new List<Dot>(ninputs);

            ComputeGateSize(ninputs);

            for (int i = 0; i < ninputs; i++)
            {
                inputs.Add(new Dot(new Point(0, GetInputYLocation(i) - 3), this));
            }
                       
            output = new Dot(new Point(Width-1,Height/2),this);
        }

        public BasicGate(int ninputs, Point loc)
            : this(ninputs)
        {
            Location = loc;
        }

        

        public override Dot DotOn(Point p)
        {
            for (int i = 1; i <= inputs.Count; i++)
            {
                if (Math.Abs(p.X) <= Settings.Default.DotRadius && Math.Abs(p.Y - 2 * i * 6 + 3) <= Settings.Default.DotRadius)
                {
                    return inputs[i - 1];
                }
            }

            if (Math.Abs(p.X - Size.Width - 1) <= Settings.Default.DotRadius && Math.Abs(p.Y - Size.Height / 2) <= Settings.Default.DotRadius)
            {
                return output;
            }

            return null;
        }

        public override void Reset()
        {
            output.Value = false;
        }

        public override void IncreaseNumberOfInputs()
        {
            if (!DisableInputChanging)
            {
                int numberOfInputs = inputs.Count;

                Dot dot = new Dot(
                    new Point(
                        0,
                        GetInputYLocation(numberOfInputs) - 3)
                        , this);

                inputs.Add(dot);

                ComputeGateSize(numberOfInputs + 1);

                output.Location = new Point(
                    output.Location.X,
                    Height / 2);

                base.IncreaseNumberOfInputs();
            }
        }

        public override void DecreaseNumberOfInputs()
        {
            if (!DisableInputChanging)
            {
                Dot dot = GetDotWithoutConnectedWires();

                if (dot != null)
                {
                    inputs.Remove(dot);

                    for (int i = 0; i < inputs.Count; i++)
                    {
                        inputs[i].Location = new Point(
                            inputs[i].Location.X,
                            GetInputYLocation(i) - 3);
                    }

                    ComputeGateSize(inputs.Count);

                    output.Location = new Point(
                        output.Location.X,
                        Height / 2);
                }

                base.DecreaseNumberOfInputs();
            }
        }

        protected Boolean CanDecreaseNumberOfInputs
        {
            get
            {
                Dot dot = GetDotWithoutConnectedWires();

                if (dot != null)
                {
                    return true;
                }

                return false;
            }
        }

        protected Dot GetDotWithoutConnectedWires()
        {
            foreach (Dot dot in inputs)
            {
                if (dot.connectedWires.Count == 0)
                {
                    return dot;
                }
            }

            return null;
        }

        public override Point[] GetDotPoints()
        {
            List<Point> pointList = new List<Point>();

            Point p;

            foreach (Dot d in inputs)
            {
                p = new Point(
                    d.Location.X + this.Location.X,
                    d.Location.Y + this.Location.Y);

                pointList.Add(p);
            }

            p = new Point(
                output.Location.X + this.Location.X,
                output.Location.Y + this.Location.Y);

            pointList.Add(p);

            return pointList.ToArray();
        }
    }
}
