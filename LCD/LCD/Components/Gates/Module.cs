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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Runtime.Serialization;
//using LCD.Properties.
using Settings = LCD.Properties.Settings;
using LCD.Components.Abstract;
using System.ComponentModel;

namespace LCD.Components.Abstract
{
    enum Alignment
    {
        Vertical,
        Horizontal
    }

    [Serializable]
    class Module : Gate
    {
        private List<Dot> inputs { get; set; }
        private List<Dot> outputs { get; set; }
        private string fileName { get; set; }
        private Circuit circuit { get; set; }
        
        [Browsable(true), Description("Replaces every space with a newline character")]
        public Boolean WrapWords { get; set; }

        public override void Simulate()
        {
            int idx = 0;

            foreach (Dot d in inputs)
            {
                /*
                if (d.w != null)
                {
                    d.Value = d.w.GetOtherDot(d).Value;
                    Btn b = circuit.GetButton(idx);
                    b.SetValue(d.Value);
                }
                */

                SetDotValue(d);

                Btn b = circuit.GetButton(idx);
                b.SetValue(d.Value);

                idx++;
            }
            circuit.Simulate();
            idx = 0;
            foreach (Dot d in outputs)
            {
                Led l = circuit.GetLed(idx);
                d.Value=l.GetValue();
                idx++;
            }
        }

        private Font textFont = new Font("Arial", 8, FontStyle.Bold);

        private String WrappedFileName
        {
            get
            {
                if (fileName != null)
                {
                    int ind = fileName.LastIndexOf('\\');
                    String temp = fileName.Remove(0, ind + 1);
                    ind = temp.LastIndexOf('.');
                    temp = temp.Remove(ind);

                    StringBuilder builder = new StringBuilder(temp);

                    if (WrapWords)
                    {
                        for (int i = 0; i < builder.Length; i++)
                        {

                            if (builder[i].CompareTo(' ') == 0)
                            {
                                builder[i] = '\n';
                            }
                        }
                    }

                    return builder.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        private void ComputeGateSize(int numberOfInputs, int numberOfOutputs)
        {
            int n = numberOfInputs > numberOfOutputs ? numberOfInputs : numberOfOutputs;

            int width=50, height;

            height = 6 * (2 * n + 1);

            Size = new Size(width, height);
        }

        private const int offsetFromMargin = 5;

        private void ChangeSizeAccordingToText(Graphics g)
        {
            int width, height=Size.Height;

            Size textSize = SizeOfText(g);

            width = textSize.Width+2*offsetFromMargin;

            int textHeight = textSize.Height;

            if (height < textHeight + 2 * offsetFromMargin)
            {
                height = textHeight + 2 * offsetFromMargin;
            }

            Size = new Size(width, height);

            ChangeOutputsLocation();
        }

        private void ChangeOutputsLocation()
        {
            for (int i = 0; i < outputs.Capacity; i++)
            {
                outputs[i].Location=new Point(Width, 2 * (i + 1) * 6 - 3);
            }
        }

        protected Size SizeOfText(Graphics g)
        {
            if (textFont == null)
            {
                textFont = new Font("Arial", 8, FontStyle.Bold);
            }

            SizeF textSize = g.MeasureString(WrappedFileName, textFont);

            return textSize.ToSize();
        }

        public override void Draw(Graphics g)
        {
            int w, h, x, y;
            x = Location.X; y = Location.Y;

            ComputeGateSize(inputs.Capacity, outputs.Capacity);

            ChangeSizeAccordingToText(g);

            w = Size.Width; ; h = Size.Height;
            
            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, w, h));
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, w, h));

            if (WrappedFileName != "")
            {
                               
                Size textSize=SizeOfText(g);

                g.DrawString(WrappedFileName, textFont, new SolidBrush(Color.White)
                        , new PointF(offsetFromMargin, h / 2 - textSize.Height / 2));
                
                                
            }

            /*for (int i = 1; i <= n; i++)
                g.DrawLine(Pens.Black, new Point(0, 2 * i * 6 - 3), new Point(10, 2 * i * 6 - 3));

            g.DrawLine(Pens.Black, new Point(w - 21, h / 2 - 0), new Point(w - 1, h / 2 - 0));

            g.DrawLine(Pens.Black, new Point(10, 0), new Point(10, h - 1));
            g.DrawArc(Pens.Black, new Rectangle(-7, 0, w - 15, h - 1), -90, 180);*/

            foreach (Dot d in inputs)
                d.Draw(g);
            foreach (Dot d in outputs)
                d.Draw(g);
            if (Selected)
                g.DrawRectangle(Pens.Blue, new Rectangle(0,0,w,h));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
        }

        public override void Clear(Graphics g)
        {
            int w, h, x, y;
            x = Location.X; y = Location.Y;

            ComputeGateSize(inputs.Capacity, outputs.Capacity);

            ChangeSizeAccordingToText(g);

            w = Size.Width; ; h = Size.Height;

            g.TranslateTransform(x, y);
            g.RotateTransform(Angle);

            Pen pen = new Pen(Settings.Default.CircuitBackColor);
            SolidBrush brush = new SolidBrush(Settings.Default.CircuitBackColor);

            g.FillRectangle(brush, new Rectangle(0, 0, w, h));
            g.DrawRectangle(pen, new Rectangle(0, 0, w, h));

            if (WrappedFileName != "")
            {

                Size textSize = SizeOfText(g);

                g.DrawString(WrappedFileName, textFont, brush
                        , new PointF(offsetFromMargin, h / 2 - textSize.Height / 2));


            }

            /*for (int i = 1; i <= n; i++)
                g.DrawLine(Pens.Black, new Point(0, 2 * i * 6 - 3), new Point(10, 2 * i * 6 - 3));

            g.DrawLine(Pens.Black, new Point(w - 21, h / 2 - 0), new Point(w - 1, h / 2 - 0));

            g.DrawLine(Pens.Black, new Point(10, 0), new Point(10, h - 1));
            g.DrawArc(Pens.Black, new Rectangle(-7, 0, w - 15, h - 1), -90, 180);*/

            foreach (Dot d in inputs)
                d.Clear(g);
            foreach (Dot d in outputs)
                d.Clear(g);
            if (Selected)
                g.DrawRectangle(pen, new Rectangle(0, 0, w, h));

            g.RotateTransform(-Angle);
            g.TranslateTransform(-x, -y);
            
            base.Clear(g);
        }

        private Module(string filename)
        {
            this.fileName = filename;

            LoadModule();

            int numberOfInputs, numberOfOutputs;

            numberOfInputs = circuit.CountButtons();
            numberOfOutputs = circuit.CountLeds();

            ComputeGateSize(numberOfInputs, numberOfOutputs);

            inputs = new List<Dot>(circuit.CountButtons());
            outputs = new List<Dot>(circuit.CountLeds());
            for (int i = 0; i < inputs.Capacity; i++)
            {
                inputs.Add(new Dot(new Point(0, 2 * (i + 1) * 6 - 3),this));

                inputs[i].Description=circuit.GetButton(i).Description;
            }
            for (int i = 0; i < outputs.Capacity; i++)
            {
                outputs.Add(new Dot(new Point(Width, 2 * (i + 1) * 6 - 3), this));

                outputs[i].Description = circuit.GetLed(i).Description;
            }
        }

        private void LoadModule()
        {
            if (String.IsNullOrEmpty(fileName)) return;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            circuit = (Circuit)bf.Deserialize(fs);
            fs.Close();
            fs.Dispose();
        }

        public Module(string filename, Point loc)
            : this(filename)
        {
            Location = loc;
        }

        public override Dot DotOn(Point p)
        {
            for (int i = 1; i <= inputs.Capacity; i++)
                if (Math.Abs(p.X) <= Settings.Default.DotRadius && Math.Abs(p.Y - 2 * i * 6 + 3) <= Settings.Default.DotRadius)
                    return inputs[i - 1];
            for (int i = 1; i <= outputs.Capacity; i++)
                if (Math.Abs(Width - p.X) <= Settings.Default.DotRadius && Math.Abs(p.Y - 2 * i * 6 + 3) <= Settings.Default.DotRadius)
                    return outputs[i - 1];
            
            return null;
        }

        public override string ToString()
        {
            return "Module (" + Location.X + "," + Location.Y + ")";
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

            foreach (Dot d in outputs)
            {
                p = new Point(
                    d.Location.X + this.Location.X,
                    d.Location.Y + this.Location.Y);

                pointList.Add(p);
            }

            return pointList.ToArray();
        }
    }
}
