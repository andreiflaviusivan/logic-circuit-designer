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
using System.Windows.Forms;
using LCD.Components;
using System.Drawing;
using LCD.Components.Abstract;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Printing;
using Settings = LCD.Properties.Settings;
using LCD.Interface.Additional;

namespace LCD.Interface
{
    public class CircuitView : PictureBox
    {
        #region Fields

        public Circuit circuit { get; set; }
        //private bool isMouseDownGate = false;
        private bool isMouseDownDot = false;
        public bool Simulating { get; set; }
        private Dot selectedDot = null;
        private WirePoint selectedWP = null;
        private bool anythingMoved = false;
        private Point MouseDownPosition;
        private bool saved;
        private PrintDocument printDoc=new PrintDocument();
        private Point firstMouseHoldPoint=default(Point);
        private Rectangle selectionRectangle = default(Rectangle);

        private Gate floatingGate = null;

        public bool Saved
        {
            get
            {
                return saved;
            }
            protected set
            {
                saved = value;
                if (!saved)
                {
                    circuit.SaveState();
                }
            }
        }

        public string FileName { get; set; }
        private ToolTip toolTip = new ToolTip();
        private Gate lastToolTippedGate = null;
        private Dot lastToolTippedDot = null;

        #endregion

        public event GateSelectedEvent OnGateSelected;

        public delegate void GateSelectedEvent(Gate[] gateArray);

        protected Graphics GraphicsObject
        {
            get
            {
                Graphics g = Graphics.FromImage(Image);
                
                return g;
                
            }
        }

        protected void DrawingFinished()
        {
            Image = Image;
        }

        public CircuitView()
        {
            circuit = new Circuit();
            Initialize();
            Saved = true;

            circuit.SaveState();
        }

        public CircuitView(string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            circuit = (Circuit)bf.Deserialize(fs);
            fs.Close();
            fs.Dispose();
            this.FileName = FileName;

            Initialize();
            Saved = true;
            ResizeAtLoad();

            circuit.SaveState();

            UpdateWirePointsParents();
        }

        public void Reset()
        {
            foreach (Gate g in circuit.Gates)
            {
                g.Reset();
            }
        }

        private void Initialize()
        {
            Image = new Bitmap(Size.Width, Size.Height);
            SizeChanged += new EventHandler(CircuitView_SizeChanged);
            MouseDown += new MouseEventHandler(CircuitView_MouseDown);
            MouseUp += new MouseEventHandler(CircuitView_MouseUp);
            MouseMove += new MouseEventHandler(CircuitView_MouseMove);
            MouseWheel += new MouseEventHandler(CircuitView_MouseWheel);
            MouseEnter += new EventHandler(CircuitView_MouseEnter);
            KeyDown += new KeyEventHandler(CircuitView_KeyDown);
            printDoc.PrintPage+= new PrintPageEventHandler(printDoc_PrintPage);

            UpdateSnapSettings();
        }

        //Facuta doar pt k nu mai mergeau unele circuite
        //dupa niste modificari
        private void UpdateWirePointsParents()
        {
            foreach (Wire w in circuit.Wires)
            {
                w.UpdateWirePointsParent();
            }
        }

        public void UpdateSnapSettings()
        {
            SnapMode = Settings.Default.SnapEnabled;
            SnapTolerance = Settings.Default.SnapTolerance;
        }

        private void ClearAllGates()
        {
            foreach (Gate gate in circuit.Gates)
            {
                gate.Clear(GraphicsObject);
            }

            DrawingFinished();
        }

        private void ClearAllWires()
        {
            foreach (Wire wire in circuit.Wires)
            {
                wire.Clear(GraphicsObject);
            }

            DrawingFinished();
        }

        public void ClearAll()
        {
            ClearAllGates();
            ClearAllWires();
        }

        public void ClearBackground()
        {
            GraphicsObject.Clear(Settings.Default.CircuitBackColor);

            DrawingFinished();
        }


        private void DrawWiresConnectedTo(Gate gate, Graphics g)
        {
            foreach (Wire w in circuit.GetWiresConnectedTo(gate))
            {
                w.Draw(g);
            }

            DrawingFinished();
        }

        private void ClearWiresConnectedTo(Gate gate, Graphics g)
        {
            foreach (Wire w in circuit.GetWiresConnectedTo(gate))
            {
                w.Clear(g);
            }

            DrawingFinished();
        }

        public void RedrawGates()
        {
            RedrawGates(null);
        }

        private void RedrawGates(Graphics graph)
        {
            Graphics gr;
            if (graph == null)
            {
                try
                {
                    gr = GraphicsObject;
                }
                catch
                {
                    return;
                }
            }
            else
            {
                gr = graph;
            }
            //gr.Clear(Settings.Default.CircuitBackColor);
            //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //Draw the floating gate if it exists

            if (floatingGate != null)
            {
                floatingGate.Draw(gr);
            }
            
            foreach (Gate g in circuit.Gates)
            {
                g.Draw(gr);
            }


            foreach (Wire w in circuit.Wires)
            {
                w.Draw(gr);

                
            }

            if (selectionRectangle != default(Rectangle))
            {
                gr.DrawRectangle(new Pen(Color.Black), selectionRectangle);
            }

            if (graph == null)
            {
                DrawingFinished();
            }
        }

        //Draw only the gates which intersect with the Rectangle
        private void RedrawGates(Graphics graph, Rectangle intersetionRect)
        {
            Graphics gr;
            if (graph == null)
            {
                try
                {
                    gr = GraphicsObject;
                }
                catch
                {
                    return;
                }
            }
            else
            {
                gr = graph;
            }
            //gr.Clear(Settings.Default.CircuitBackColor);
            //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //Draw the floating gate if it exists

            foreach (Gate g in circuit.Gates)
            {
                if (g.GetRectangle().IntersectsWith(intersetionRect))
                {
                    g.Draw(gr);
                }
            }


            foreach (Wire wire in circuit.Wires)
            {
                Point[] pointArray = wire.GetLinePoints();

                bool ok = false;

                foreach (Point point in pointArray)
                {
                    if (point.X >= intersetionRect.X &&
                        point.X <= intersetionRect.Left + intersetionRect.Width &&
                        point.Y >= intersetionRect.Y &&
                        point.Y <= intersetionRect.Top + intersetionRect.Height)
                    {
                        ok = true;
                    }
                }

                if (ok)
                {
                    wire.Draw(gr);
                }


            }

            if (graph == null)
            {
                DrawingFinished();
            }
        }

        //Redraw the gates which are contained in at least one rectangle from the list
        private void RedrawGates(Graphics graph, IEnumerable<Rectangle> rectList)
        {
            foreach (Rectangle r in rectList)
            {
                RedrawGates(graph, r);
            }
        }

        private void tabControl_SizeChanged(object sender, EventArgs e)
        {
            ResizeAtLoad();
        }

        //Resizes the component after loading a circuit from file
        //Some gates may not be shown if this function is not called
        public void ResizeAtLoad()
        {
            if (this.Parent != null)
            {
                this.Size = this.Parent.Size;
            }

            foreach (Gate gate in circuit.Gates)
            {
                AcceptMoveAndResize(gate.Location, gate);
            }

            foreach (Wire wire in circuit.Wires)
            {
                foreach (WirePoint wirePoint in wire.Points)
                {
                    AcceptMoveAndResize(
                    wirePoint.Location,
                    new Size(
                        Settings.Default.WirePointRadius,
                        Settings.Default.WirePointRadius));
                }
            }
        }

        private void ShowToolTip(String text, int seconds, Point location)
        {
            if (seconds > 0)
            {
                toolTip.Show(text, this, location.X, location.Y, seconds*1000);
            }
            
        }

        public void DumpToDisk(string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, circuit);
            fs.Close();
            fs.Dispose();

            Saved = true;
        }

        private Boolean IsMouseOverAnyComponent(Point location)
        {
            Gate gateOn = GateOn(location);
            Wire wireOn = WireOn(location);
            Dot dotOn = DotOn(location);
            WirePoint wirePointOn = WirePointOn(location);

            if (gateOn == null && wireOn == null && dotOn == null && wirePointOn == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Add Delete Move Resize

        public void AddGate(Gate g)
        {
            circuit.Gates.Add(g);

            g.Draw(GraphicsObject);
            
            Saved = false;

            DrawingFinished();
        }

        public void AddFloatingGate(Gate g)
        {
            DeleteFloatingGate();

            if (!Simulating)
            {
                floatingGate = g;

                floatingGate.Draw(GraphicsObject);
            }
                        
        }

        public void DeleteFloatingGate()
        {
            if (floatingGate != null)
            {
                floatingGate.Clear(GraphicsObject);

                RedrawGates(GraphicsObject, floatingGate.GetRectangle());

                floatingGate = null;
            }
        }

        private void AddWire(Dot start, Dot dest)
        {
            Wire w = new Wire();

            w.src = dest;
            w.dst = start;

            dest.w = w;
            start.w = w;

            dest.connectedWires.Add(w);
            start.connectedWires.Add(w);

            circuit.Wires.Add(w);

            w.Draw(GraphicsObject);

            Saved = false;
        }
        
        private bool AcceptMoveAndResize(Point newLocation, Gate gate)
        {
            if (newLocation.X < 0 || newLocation.Y < 0)
            {
                return false;
            }

            //Resize the width 
            if (newLocation.X + gate.Size.Width > this.Width)
            {
                this.Width = newLocation.X + 2 * gate.Size.Width;

            }

            //Resize the height
            if (newLocation.Y + gate.Size.Height > this.Height)
            {
                this.Height = newLocation.Y + 2 * gate.Size.Height;

            }

            return true;
        }

        private bool AcceptMoveAndResize(Point newLocation, Size size)
        {
            if (newLocation.X < 0 || newLocation.Y < 0)
            {
                return false;
            }

            //Resize the width 
            if (newLocation.X + size.Width > this.Width)
            {
                this.Width = newLocation.X + 2 * size.Width;

            }

            //Resize the height
            if (newLocation.Y + size.Height > this.Height)
            {
                this.Height = newLocation.Y + 2 * size.Height;

            }

            return true;
        }

        #endregion
        #region Keyboard events

        void CircuitView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                DeleteSelection();

            if (e.KeyCode == Keys.Escape)
            {
                DeleteFloatingGate();
            }
        }

        void CircuitView_SizeChanged(object sender, EventArgs e)
        {
            if (Size.Height == 0)
            {
                this.Height = 1;
            }

            if (Size.Width == 0)
            {
                this.Width = 1;
            }

            Image = new Bitmap(Size.Width, Size.Height);

            RedrawGates();
        }

        #endregion
        #region Mouse events

        private void CircuitView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Simulating) return;
            int WHEEL_DELTA = 120;

            foreach (Gate g in circuit.Gates)
            {
                if (g.Selected)
                {
                    g.Clear(GraphicsObject);

                    ClearWiresConnectedTo(g, GraphicsObject);

                    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        g.Angle -= Math.Sign(e.Delta);
                    }
                    else
                    {
                        g.Angle -= (4 * e.Delta / WHEEL_DELTA);
                    }

                    g.Draw(GraphicsObject);

                    DrawWiresConnectedTo(g, GraphicsObject);

                    DrawingFinished();
                }
            }

        }

        public void MoveSelectedGatesUp()
        {
            foreach (Gate gate in circuit.SelectedGates)
            {
                Point location = new Point(
                    gate.Location.X,
                    gate.Location.Y - 1);

                MoveGate(gate, 0,-1);
            }

            DrawingFinished();
        }

        public void MoveSelectedGatesDown()
        {
            foreach (Gate gate in circuit.SelectedGates)
            {
                Point location = new Point(
                    gate.Location.X,
                    gate.Location.Y + 1);

                MoveGate(gate, 0,1);
            }

            DrawingFinished();
        }

        public void MoveSelectedGatesRight()
        {
            foreach (Gate gate in circuit.SelectedGates)
            {
                Point location = new Point(
                    gate.Location.X + 1,
                    gate.Location.Y);

                MoveGate(gate, 1,0);
            }

            DrawingFinished();
        }

        public void MoveSelectedGatesLeft()
        {
            foreach (Gate gate in circuit.SelectedGates)
            {
                Point location=new Point(
                    gate.Location.X-1,
                    gate.Location.Y);

                MoveGate(gate, -1,0);
            }

            DrawingFinished();
        }

        private void MoveGate(Gate gate, int xOffset,int yOffset)
        {
            Point newLocation = gate.GetPointIfMoving(xOffset, yOffset);

            if (AcceptMoveAndResize(newLocation, gate))
            {
                MoveGatesWirePoints(gate, xOffset, yOffset);

                ClearWiresConnectedTo(gate, GraphicsObject);

                gate.Move(xOffset, yOffset, GraphicsObject);

                DrawWiresConnectedTo(gate, GraphicsObject);

                anythingMoved = true;

                RedrawGates(GraphicsObject,gate.GateVicinity());

                foreach (Wire w in circuit.GetWiresConnectedTo(gate))
                {
                    RedrawGates(GraphicsObject, w.GetLinePointsVicinities(gate));
                }
            }
        }

        private void MoveSelectedGates(MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left && firstMouseHoldPoint==default(Point))
            {
                foreach (Gate gate in circuit.SelectedGates)
                {
                    int xOffset=e.Location.X - MouseDownPosition.X;
                    int yOffset=e.Location.Y - MouseDownPosition.Y;

                    MoveGate(gate, xOffset,yOffset);
                }

                MouseDownPosition = e.Location;
            }
        }

        private void MoveGatesWirePoints(Gate g, int xOffset, int yOffset)
        {
            foreach (Wire wire in circuit.GetSelectedWiresConnectedTo(g))
            {
                

                wire.MoveWirePoints(xOffset, yOffset,GraphicsObject);

                anythingMoved = true;

                RedrawGates();
            }
        }

        private void MoveFloatingGate(MouseEventArgs e)
        {
            if (floatingGate!=null && e.Button == MouseButtons.None)
            {
                Point location = new Point(
                        e.Location.X - floatingGate.Width / 2,
                        e.Location.Y - floatingGate.Height / 2);

                if (AcceptMoveAndResize(location, floatingGate))
                {
                    floatingGate.Move(location, GraphicsObject);

                    RedrawGates(GraphicsObject, floatingGate.GateVicinity());
                }
                
            }
        }

        private void MoveSelectedWirePoints(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //selectedWP = WirePointOn(e.Location);

                if (selectedWP != null)
                {
                    if (AcceptMoveAndResize(
                        e.Location,
                        new Size(
                            Settings.Default.WirePointRadius,
                            Settings.Default.WirePointRadius)))
                    {
                        selectedWP.Move(e.Location,GraphicsObject);
                        
                        anythingMoved = true;

                        RedrawGates(GraphicsObject,selectedWP.Parent.GetLinePointsVicinities(selectedWP));

                        RedrawGates(GraphicsObject, selectedWP.GetVicinity());
                    }
                }
            }
        }

        private Object tempClipboardData = null;
        private const String clipboardGateFormat = "LCD Clipboard Gate Format";

        private void CopyFloatingGate()
        {
            if (floatingGate != null)
            {
                if (Clipboard.ContainsData(clipboardLCDFormat))
                {
                    tempClipboardData = Clipboard.GetData(clipboardLCDFormat);
                }

                Clipboard.SetData(clipboardGateFormat, floatingGate);
            }
        }

        private void PasteFloatingGate()
        {
            if (Clipboard.ContainsData(clipboardGateFormat))
            {
                floatingGate = (Gate)Clipboard.GetData(clipboardGateFormat);

                if (tempClipboardData != null)
                {
                    Clipboard.SetData(clipboardLCDFormat, tempClipboardData);

                    tempClipboardData = null;
                }
            }
        }

        private void FloatingGateOperations(MouseEventArgs e)
        {
            if (floatingGate != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    AddGate(floatingGate);

                    if (!((Control.ModifierKeys & Keys.Control) == Keys.Control))
                    {
                        floatingGate = null;
                    }
                    else
                    {
                        CopyFloatingGate();
                        PasteFloatingGate();
                    }
                }

                if (e.Button == MouseButtons.Right)
                {
                    DeleteFloatingGate();
                }
            }
        }

        private void ToolTipSelectedGate(MouseEventArgs e)
        {
            Gate gateMouseOver = GateOn(e.Location);

            if (e.Button == MouseButtons.None &&
                gateMouseOver != null &&
                lastToolTippedGate != gateMouseOver)
            {
                String gateDescription = gateMouseOver.Description;

                if (gateDescription != null && gateDescription.Length != 0)
                {
                    ShowToolTip(
                        gateDescription,
                        Settings.Default.DescriptionToolTipTimeOut,
                        new Point(
                            gateMouseOver.Location.X + 5,
                            gateMouseOver.Location.Y - 15));

                    lastToolTippedGate = gateMouseOver;
                }
            }
            else
            {
                if (gateMouseOver == null)
                {
                    lastToolTippedGate = null;
                }
            }
        }

        private void ToolTipSelectedDot(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                Dot dotOver = DotOn(e.Location);

                if (dotOver != null)
                {
                    if (dotOver.Parent is Module &&
                        dotOver.Description != null &&
                        dotOver.Description.Length != 0)
                    {
                        Gate gateParent = dotOver.Parent;

                        if (e.Button == MouseButtons.None &&
                            dotOver != lastToolTippedDot)
                        {

                            ShowToolTip(
                                dotOver.Description,
                                Settings.Default.DescriptionToolTipTimeOut,
                                new Point(
                                    gateParent.Location.X + 5,
                                    gateParent.Location.Y - 15));

                            lastToolTippedDot = dotOver;
                        }
                    }
                }
                else
                {
                    lastToolTippedDot = null;
                }
            }
        }

        private void ChangeCursorIfMouseOverDot(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                Dot dotOver = DotOn(e.Location);

                if (dotOver != null)
                {
                    this.Cursor = Cursors.Cross;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void DrawSelectionRectangle(MouseEventArgs e)
        {
            DeleteSelectionRectangle();
            
            if (e.Button == MouseButtons.Left && firstMouseHoldPoint != default(Point))
            {
                
                selectionRectangle = LinePointUtils.GetRectangleWithCorners(
                    firstMouseHoldPoint, e.Location);

                Graphics gr = GraphicsObject;

                gr.DrawRectangle(new Pen(Color.Black), selectionRectangle);

                SelectComponentsInSelectionRectangle();
                /*
                int selectionVicinity = 40;
                
                Rectangle auxRectangle=new Rectangle(
                    selectionRectangle.X - selectionVicinity,
                    selectionRectangle.Y - selectionVicinity,
                    selectionRectangle.Width + selectionVicinity,
                    selectionRectangle.Height + selectionVicinity);

                //Redraw any gate in the selection's rectangle vicinity
                //RedrawGates(GraphicsObject, auxRectangle);
                */

                DrawingFinished();
            }
            
            //DrawingFinished();
        }

        private void DeleteSelectionRectangle()
        {
            Graphics gr = GraphicsObject;

            gr.DrawRectangle(new Pen(Settings.Default.CircuitBackColor),
                selectionRectangle);

            DrawingFinished();

            RedrawGates(GraphicsObject,selectionRectangle);

            selectionRectangle = default(Rectangle);
        }

        private List<Gate> gatesInSelectionRectangle = new List<Gate>();
        private List<Wire> wiresInSelectionRectangle = new List<Wire>();

        private void SelectComponentsInSelectionRectangle()
        {
            
            
            foreach (Gate gate in circuit.Gates)
            {
                Rectangle gateRectangle=gate.GetRectangle();

                if (gateRectangle.IntersectsWith(selectionRectangle))
                {
                    if (!gatesInSelectionRectangle.Contains(gate))
                    {
                        gate.InvertSelect(GraphicsObject);

                        gatesInSelectionRectangle.Add(gate);

                        //gate.Draw(GraphicsObject);
                    }
                }
                else
                {
                    if (gatesInSelectionRectangle.Contains(gate))
                    {
                        gate.InvertSelect(GraphicsObject);

                        gatesInSelectionRectangle.Remove(gate);

                        //gate.Draw(GraphicsObject);
                    }
                }
                
            }
           
            foreach (Wire wire in circuit.Wires)
            {
                Point[] pointArray = wire.GetLinePoints();

                bool ok = false;

                foreach (Point point in pointArray)
                {
                    if (point.X >= selectionRectangle.X &&
                        point.X <= selectionRectangle.Left + selectionRectangle.Width &&
                        point.Y >= selectionRectangle.Y &&
                        point.Y <= selectionRectangle.Top + selectionRectangle.Height)
                    {
                        ok = true;
                    }
                }

                if (ok)
                {
                    if (!wiresInSelectionRectangle.Contains(wire))
                    {
                        wire.InvertSelect(GraphicsObject);

                        wiresInSelectionRectangle.Add(wire);
                    }
                }
                else
                {
                    if (wiresInSelectionRectangle.Contains(wire))
                    {
                        wire.InvertSelect(GraphicsObject);

                        wiresInSelectionRectangle.Remove(wire);
                    }
                }
            }

            //DrawingFinished();
        }

        private Point lastWirePreviewPoint = default(Point);

        private void WirePreview(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isMouseDownDot && selectedDot != null)
                {
                    Graphics gr = GraphicsObject;

                    Point startPoint = new Point(
                        selectedDot.Location.X + selectedDot.Parent.Location.X,
                        selectedDot.Location.Y + selectedDot.Parent.Location.Y);

                    startPoint = LinePointUtils.RotatePoint(
                        startPoint,
                        selectedDot.Parent.Location,
                        selectedDot.Parent.Angle);

                    if (lastWirePreviewPoint != default(Point))
                    {
                        gr.DrawLine(new Pen(Color.White),
                            startPoint,
                            lastWirePreviewPoint);

                        
                    }

                    lastWirePreviewPoint = e.Location;

                    gr.DrawLine(new Pen(Color.Black),
                        startPoint,
                        e.Location);


                    RedrawGates();
                    DrawingFinished();
                }
            }
        }

        private void ClearLastWirePreviewPoint(MouseEventArgs e)
        {
            if (lastWirePreviewPoint != default(Point))
            {
                Point startPoint = new Point(
                        selectedDot.Location.X + selectedDot.Parent.Location.X,
                        selectedDot.Location.Y + selectedDot.Parent.Location.Y);

                startPoint = LinePointUtils.RotatePoint(
                    startPoint,
                    selectedDot.Parent.Location,
                    selectedDot.Parent.Angle);

                GraphicsObject.DrawLine(new Pen(Settings.Default.CircuitBackColor),
                    startPoint,
                    e.Location);
            }
            
            DrawingFinished();
        }

        private void CircuitView_MouseMove(object sender, MouseEventArgs e)
        {
            if (Simulating) return;

            MoveSelectedGates(e);
            MoveFloatingGate(e);
            
            Point closestPoint = SnapPoint(e.Location);

            MouseEventArgs tempArgs = new MouseEventArgs
                (e.Button,
                e.Clicks,
                closestPoint.X,
                closestPoint.Y,
                e.Delta);
            
            MoveSelectedWirePoints(tempArgs);
            
            ToolTipSelectedGate(e);
            ToolTipSelectedDot(e);
            ChangeCursorIfMouseOverDot(e);
            
            DrawSelectionRectangle(e);

            WirePreview(e);
        }

        private void CircuitView_MouseUp(object sender, MouseEventArgs e)
        {
            if (Simulating)
            {
                Gate g = GateOn(e.Location);
                if (g != null)
                {
                    Point rotatedPoint = LinePointUtils.RotatePoint(e.Location, g.Location, -g.Angle);
                    MouseEventArgs eventArgs = new MouseEventArgs(
                        e.Button,
                        e.Clicks,
                        rotatedPoint.X,
                        rotatedPoint.Y,
                        e.Delta);
                    g.MouseUp(eventArgs);
                }

                return;
            }

            //isMouseDownGate = false;

            firstMouseHoldPoint = default(Point);

            gatesInSelectionRectangle.Clear();
            wiresInSelectionRectangle.Clear();

            if (OnGateSelected != null)
            {
                OnGateSelected(circuit.SelectedGates);
            }

            ClearLastWirePreviewPoint(e);

            //Undo redo when gates have been moved
            //is not working!
            if (anythingMoved)
            {
                //Saved = false;

                anythingMoved = false;
            }

            if (isMouseDownDot)
            {
                Dot d = DotOn(e.Location);

                if (d != null && selectedDot != d)
                {
                    //Don't let 2 dots of the same gate connected
                    //or if they are already connected
                    if (d.Parent != selectedDot.Parent && 
                        !circuit.AlreadyConnected(d,selectedDot))
                    {
                        
                        
                        AddWire(d, selectedDot);
                        //RedrawGates();

                        //d = null;
                        //selectedDot = null;
                        
                        
                        DrawingFinished();
                    }

                    

                }

                isMouseDownDot = false;
            }

            if (e.Button == MouseButtons.Right)
            {
                Wire wire = WireOn(e.Location);

                if (wire != null)
                {
                    if (selectedWP != null)
                    {
                        wire.Clear(GraphicsObject);

                        wire.Points.Remove(selectedWP);

                        wire.Draw(GraphicsObject);
                    }
                    else
                    {
                        int idx = BeforeWirePoint(e.Location, wire);
                        wire.AddWirePoint(idx, e.Location,GraphicsObject);
                    }

                    DrawingFinished();
                }
            }

            selectedWP = null;

            RedrawGates();

            //GC.Collect();
        }

        private void CircuitView_MouseDown(object sender, MouseEventArgs e)
        {
            Gate g = GateOn(e.Location);
            Dot d = DotOn(e.Location);
            WirePoint wp = WirePointOn(e.Location);
            Wire wire = WireOn(e.Location);

            if (Simulating)
            {
                if (g != null)
                {
                    Point rotatedPoint = LinePointUtils.RotatePoint(e.Location, g.Location, -g.Angle);
                    MouseEventArgs eventArgs = new MouseEventArgs(
                        e.Button,
                        e.Clicks,
                        rotatedPoint.X,
                        rotatedPoint.Y,
                        e.Delta);
                    g.MouseDown(eventArgs);
                }

                return;
            }

            FloatingGateOperations(e);

            if (!IsMouseOverAnyComponent(e.Location) &&
                e.Button!=MouseButtons.Middle)
            {
                if (! ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                {
                    SelectNone();
                }
                
                firstMouseHoldPoint = e.Location;

                RedrawGates();
            }
            else
            {
                firstMouseHoldPoint = default(Point);
            }

            if (e.Button != MouseButtons.Middle)
            {
                if (d != null)
                    Dot_MouseDown(d, e.Location);
                else
                {
                    if (g != null)
                    {
                        Gate_MouseDown(g, e.Location);
                    }
                    else
                    {
                        if (wp != null)
                        {
                            selectedWP = wp;

                            SelectNone();
                        }
                        else
                        {
                            if (wire != null)
                            {
                                Wire_MouseDown(wire);
                            }
                        }
                    }
                }
            }

        }

        void CircuitView_MouseEnter(object sender, EventArgs e)
        {
            Focus();
        }

        private void Wire_MouseDown(Wire wire)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                wire.InvertSelect(GraphicsObject);
            }
            else
            {
                if (wire.Selected == false)
                {
                    SelectNone();
                    wire.Select(GraphicsObject);
                }
            }

            DrawingFinished();
        }

        private void Dot_MouseDown(Dot d, Point location)
        {
            isMouseDownDot = true;
            selectedDot = d;
            SelectNone();
        }

        void Gate_MouseDown(Gate g, Point location)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                g.InvertSelect(GraphicsObject);
            }
            else
            {
                if (g.Selected == false)
                {
                    SelectNone();
                    g.Select(GraphicsObject);
                    
                }
            }

            MouseDownPosition = location;
            //isMouseDownGate = true;
            DrawingFinished();
        }

        
        
        #endregion

        #region Hit tests

        private Gate GateOn(Point location)
        {
            foreach (Gate g in circuit.Gates)
            {
                Point p = LinePointUtils.RotatePoint(location, g.Location, -g.Angle);
                Rectangle r = new Rectangle(g.Location, g.Size);
                if (r.Contains(p))
                    return g;
            }
            return null;
        }

        private Dot DotOn(Point location)
        {
            Dot d;
            foreach (Gate g in circuit.Gates)
            {
                Point p = LinePointUtils.RotatePoint(location, g.Location, -g.Angle);
                d = g.DotOn(new Point(p.X - g.Location.X, p.Y - g.Location.Y));
                if (d != null)
                    return d;
            }
            return null;
        }

        private WirePoint WirePointOn(Point location)
        {
            foreach (Wire w in circuit.Wires)
            {
                foreach (WirePoint p in w.Points)
                {
                    if (p.PointOn(location))
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        private Wire WireOn(Point point)
        {
            foreach (Wire w in circuit.Wires)
            {
                Point a, b;
                a = new Point(w.src.Location.X + w.src.Parent.Location.X, w.src.Location.Y + w.src.Parent.Location.Y);
                b = new Point(w.dst.Location.X + w.dst.Parent.Location.X, w.dst.Location.Y + w.dst.Parent.Location.Y);
                a = LinePointUtils.RotatePoint(a, w.src.Parent.Location, w.src.Parent.Angle);
                b = LinePointUtils.RotatePoint(b, w.dst.Parent.Location, w.dst.Parent.Angle);
                Point lastPoint = a;
                foreach (WirePoint currentPoint in w.Points)
                {
                    if (PointOnLine(lastPoint, currentPoint.Location, point))
                        return w;
                    lastPoint = currentPoint.Location;
                }
                if (PointOnLine(lastPoint, b, point))
                    return w;
            }
            return null;
        }

        private int BeforeWirePoint(Point point, Wire w)
        {
            int x = 0;
            Point a, b;
            a = new Point(w.src.Location.X + w.src.Parent.Location.X, w.src.Location.Y + w.src.Parent.Location.Y);
            b = new Point(w.dst.Location.X + w.dst.Parent.Location.X, w.dst.Location.Y + w.dst.Parent.Location.Y);
            a = LinePointUtils.RotatePoint(a, w.src.Parent.Location, w.src.Parent.Angle);
            b = LinePointUtils.RotatePoint(b, w.dst.Parent.Location, w.dst.Parent.Angle);
            Point lastPoint = a;
            foreach (WirePoint currentPoint in w.Points)
            {
                if (PointOnLine(lastPoint, currentPoint.Location, point))
                    return x;
                x++;
                lastPoint = currentPoint.Location;
            }
            if (PointOnLine(lastPoint, b, point))
                return x;
            return 0;
        }

        private bool PointOnLine(Point ls, Point le, Point p)
        {
            double numarator1, numitor1;
            double numarator2, numitor2;
            numarator1 = p.X - ls.X;
            numitor1 = le.X - ls.X;
            numarator2 = p.Y - ls.Y;
            numitor2 = le.Y - ls.Y;
            double fr1, fr2;
            if (numitor1 == 0)
                fr1 = 0;
            else
                fr1 = numarator1 / numitor1;
            if (numitor2 == 0)
                fr2 = 0;
            else
                fr2 = numarator2 / numitor2;

            int minx, maxx, miny, maxy;
            minx = ls.X < le.X ? ls.X : le.X;
            maxx = ls.X > le.X ? ls.X : le.X;
            miny = ls.Y < le.Y ? ls.Y : le.Y;
            maxy = ls.Y > le.Y ? ls.Y : le.Y;
            if (minx == maxx)
            {
                minx -= 1;
                maxx += 1;
            }
            if (miny == maxy)
            {
                miny -= 1;
                maxy += 1;
            }

            if (minx <= p.X && p.X <= maxx && miny <= p.Y && p.Y <= maxy)
            {
                if (fr1 == 0 || fr2 == 0)
                    return true;
                if (Math.Abs(fr1 - fr2) < 0.03)
                    return true;
            }
            return false;
        }

        #endregion

        #region Selection Methods

        private void RemoveNonConnectedWires()
        {
            int loop = 0;

            while (loop < circuit.Wires.Count)
            {
                Wire wire = circuit.Wires[loop];

                if (!circuit.Gates.Contains(wire.src.Parent) ||
                    !circuit.Gates.Contains(wire.dst.Parent))
                {
                    wire.Clear(GraphicsObject);

                    wire.dst.connectedWires.Remove(wire);
                    wire.src.connectedWires.Remove(wire);

                    circuit.Wires.Remove(wire);
                }
                else
                {
                    loop++;
                }
            }

            DrawingFinished();
        }

        //All selection

        public void SelectAllGates()
        {
            foreach (Gate gate in circuit.Gates)
            {
                gate.Select(GraphicsObject);
            }

            DrawingFinished();
        }

        public void SelectAllWires()
        {
            foreach (Wire wire in circuit.Wires)
            {
                wire.Select(GraphicsObject);
            }

            DrawingFinished();
        }

        public void SelectAll()
        {
            SelectAllGates();
            SelectAllWires();
        }

        //None selection

        public void SelectNoneGates()
        {
            foreach (Gate gate in circuit.Gates)
            {
                gate.DeSelect(GraphicsObject);
            }

            DrawingFinished();
        }

        public void SelectNoneWires()
        {
            foreach (Wire wire in circuit.Wires)
            {
                wire.DeSelect(GraphicsObject);
            }

            DrawingFinished();
        }

        public void SelectNone()
        {
            SelectNoneGates();
            SelectNoneWires();
        }

        //Invert selection

        public void InvertGatesSelection()
        {
            foreach (Gate gate in circuit.Gates)
            {
                gate.InvertSelect(GraphicsObject);
            }

            DrawingFinished();
        }

        public void InvertWiresSelection()
        {
            foreach (Wire wire in circuit.Wires)
            {
                wire.InvertSelect(GraphicsObject);
            }

            DrawingFinished();
        }

        public void InvertSelection()
        {
            InvertGatesSelection();
            InvertWiresSelection();
        }

        //Crop selection

        public void CropGatesSelection()
        {
            int loop = 0;

            bool anythingDeleted = false;

            while (loop < circuit.Gates.Count)
            {
                if (!circuit.Gates[loop].Selected)
                {
                    circuit.Gates[loop].Clear(GraphicsObject);

                    circuit.Gates.Remove(circuit.Gates[loop]);

                    anythingDeleted = true;
                }
                else
                {
                    loop++;
                }
            }

            RemoveNonConnectedWires();

            if (anythingDeleted)
            {
                Saved = false;
            }

            DrawingFinished();
        }

        public void CropWiresSelection()
        {
            int loop = 0;

            bool anythingDeleted = false;

            while (loop < circuit.Wires.Count)
            {
                Wire currentWire = circuit.Wires[loop];

                if (!currentWire.Selected)
                {
                    currentWire.dst.connectedWires.Remove(currentWire);
                    currentWire.src.connectedWires.Remove(currentWire);

                    currentWire.Clear(GraphicsObject);

                    circuit.Wires.Remove(currentWire);

                    anythingDeleted = true;
                }
                else
                {
                    loop++;
                }
            }

            if (anythingDeleted)
            {
                Saved = false;
            }

            DrawingFinished();
        }

        public void CropSelection()
        {
            CropGatesSelection();
            CropWiresSelection();
        }

        //Delete selection

        public void DeleteGatesSelection()
        {
            int loop = 0;

            bool anythingDeleted = false;

            while (loop < circuit.Gates.Count)
            {
                if (circuit.Gates[loop].Selected)
                {
                    circuit.Gates[loop].Clear(GraphicsObject);

                    circuit.Gates.Remove(circuit.Gates[loop]);

                    anythingDeleted = true;
                }
                else
                {
                    loop++;
                }
            }

            RemoveNonConnectedWires();

            if (anythingDeleted)
            {
                Saved = false;
            }

            DrawingFinished();
        }

        public void DeleteWiresSelection()
        {
            int loop = 0;

            bool anythingDeleted = false;

            while (loop < circuit.Wires.Count)
            {
                Wire currentWire = circuit.Wires[loop];

                if (currentWire.Selected)
                {
                    currentWire.dst.connectedWires.Remove(currentWire);
                    currentWire.src.connectedWires.Remove(currentWire);

                    currentWire.Clear(GraphicsObject);

                    circuit.Wires.Remove(currentWire);

                    anythingDeleted = true;
                }
                else
                {
                    loop++;
                }
            }

            if (anythingDeleted)
            {
                Saved = false;
            }

            DrawingFinished();
        }

        public void DeleteSelection()
        {
            DeleteGatesSelection();
            DeleteWiresSelection();
        }

        #endregion

        #region Align Methods

        protected Gate[] GetSelectedGates()
        {
            return circuit.SelectedGates;
        }

        protected Wire[] GetSelectedWires()
        {
            return circuit.SelectedWires;
        }

        public void AlignLeftSides()
        {
            Gate[] selectedGates = GetSelectedGates();

            ClearAllWires();

            int minLeft = this.Width;

            for (int i = 0; i < selectedGates.Length; i++)
            {
                if (minLeft > selectedGates[i].Location.X)
                {
                    minLeft = selectedGates[i].Location.X;
                }
            }

            for (int i = 0; i < selectedGates.Length; i++)
            {
                selectedGates[i].Clear(GraphicsObject);

                selectedGates[i].X = minLeft;

                selectedGates[i].Draw(GraphicsObject);
            }

            RedrawGates();
        }

        public void AlignVerticalCenters()
        {
            Gate[] selectedGates = GetSelectedGates();

            ClearAllWires();

            if (selectedGates.Length == 0)
            {
                return;
            }

            int sumOfYCoordonates = 0;

            for (int i = 0; i < selectedGates.Length; i++)
            {
                sumOfYCoordonates += selectedGates[i].Location.X;
            }

            int average;

            unchecked
            {
                average = sumOfYCoordonates / selectedGates.Length;
            }

            for (int i = 0; i < selectedGates.Length; i++)
            {
                selectedGates[i].Clear(GraphicsObject);

                selectedGates[i].X = average;

                selectedGates[i].Draw(GraphicsObject);
            }

            RedrawGates();
        }

        public void AlignRightSides()
        {
            Gate[] selectedGates = GetSelectedGates();

            ClearAllWires();

            int maxLeft = -1;

            for (int i = 0; i < selectedGates.Length; i++)
            {
                if (maxLeft < selectedGates[i].Location.X)
                {
                    maxLeft = selectedGates[i].Location.X;
                }
            }

            for (int i = 0; i < selectedGates.Length; i++)
            {
                selectedGates[i].Clear(GraphicsObject);

                selectedGates[i].X = maxLeft;

                selectedGates[i].Draw(GraphicsObject);
            }

            RedrawGates();
        }

        public void AlignTopEdges()
        {
            Gate[] selectedGates = GetSelectedGates();

            ClearAllWires();

            int minTop = this.Height;

            for (int i = 0; i < selectedGates.Length; i++)
            {
                if (minTop > selectedGates[i].Location.Y)
                {
                    minTop = selectedGates[i].Location.Y;
                }
            }

            for (int i = 0; i < selectedGates.Length; i++)
            {
                selectedGates[i].Clear(GraphicsObject);

                selectedGates[i].Y = minTop;

                selectedGates[i].Draw(GraphicsObject);
            }

            RedrawGates();
        }

        public void AlignHorizontalSides()
        {
            Gate[] selectedGates = GetSelectedGates();

            ClearAllWires();

            if (selectedGates.Length == 0)
            {
                return;
            }

            int sumOfYCoordonates = 0;

            for (int i = 0; i < selectedGates.Length; i++)
            {
                sumOfYCoordonates += selectedGates[i].Location.Y;
            }

            int average;

            unchecked
            {
                average = sumOfYCoordonates / selectedGates.Length;
            }

            for (int i = 0; i < selectedGates.Length; i++)
            {
                selectedGates[i].Clear(GraphicsObject);

                selectedGates[i].Y = average;

                selectedGates[i].Draw(GraphicsObject);
            }

            RedrawGates();
        }

        public void AlignBottomEdges()
        {
            Gate[] selectedGates = GetSelectedGates();

            int maxBottom = -1;

            ClearAllWires();

            for (int i = 0; i < selectedGates.Length; i++)
            {
                if (maxBottom < selectedGates[i].Location.Y)
                {
                    maxBottom = selectedGates[i].Location.Y;
                }
            }

            for (int i = 0; i < selectedGates.Length; i++)
            {
                selectedGates[i].Clear(GraphicsObject);

                selectedGates[i].Y = maxBottom;

                selectedGates[i].Draw(GraphicsObject);
            }

            RedrawGates();
        }

        #endregion

        #region Cut Copy Paste Undo Redo

        private const String clipboardLCDFormat = "LCDCircuit";

        public void Cut()
        {
            Copy();

            DeleteSelection();
        }

        public void Copy()
        {
            Gate[] selectedGates = GetSelectedGates();
            Wire[] selectedWires = GetSelectedWires();

            Circuit circuit = new Circuit();

            circuit.Gates.AddRange(selectedGates);
            circuit.Wires.AddRange(selectedWires);

            Clipboard.SetData(clipboardLCDFormat, circuit);
        }

        public void Paste()
        {
            SelectNone();

            if (Clipboard.ContainsData(clipboardLCDFormat))
            {
                try
                {

                    Circuit pastedCircuit = (Circuit)Clipboard.GetData(clipboardLCDFormat);

                    if (pastedCircuit != null &&
                        pastedCircuit.Gates != null &&
                        pastedCircuit.Wires != null)
                    {
                        //Change the pasted gates' location a little bit

                        

                        circuit.Gates.AddRange(pastedCircuit.Gates);
                        circuit.Wires.AddRange(pastedCircuit.Wires);

                        foreach (Gate gate in pastedCircuit.Gates)
                        {

                            MoveGate(gate, 15, 15);
                        }

                        if (pastedCircuit.Gates.Count != 0 ||
                            pastedCircuit.Wires.Count != 0)
                        {
                            Saved = false;
                        }
                    }
                }
                catch
                {

                }
            }

            RedrawGates();
        }

        public void Undo()
        {
            ClearAll();

            circuit.Undo();

            RedrawGates();
        }

        public void Redo()
        {
            ClearAll();

            circuit.Redo();

            RedrawGates();
        }

        #endregion

        #region Print

        public void Print()
        {
            PrintDialog printDialog = new PrintDialog();

            //Let it be an extended dialog
            //If it is set to false the dialog will
            //not show on some machines
            printDialog.UseEXDialog = true;
            printDialog.Document = printDoc;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        public void PrintPreview()
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDoc;
            if (printPreviewDialog.ShowDialog() == DialogResult.OK)
            {
                Print();
            }
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            RedrawGates(e.Graphics);
        }

        #endregion

        #region Miscelanous

        public void IncreaseNumberOfInputs()
        {
            foreach (Gate gate in GetSelectedGates())
            {
                gate.IncreaseNumberOfInputs();
            }
        }

        public void DecreaseNumberOfInputs()
        {
            foreach (Gate gate in GetSelectedGates())
            {
                gate.DecreaseNumberOfInputs();
            }
        }

        

        #endregion


        

        #region Wire Point Snapping

        private int _tolerance = 10;
        private bool _snapMode = false;

        public int SnapTolerance
        {
            get
            {
                return _tolerance;
            }
            set
            {
                _tolerance = value;
            }
        }

        public bool SnapMode
        {
            get
            {
                return _snapMode;
            }
            set
            {
                _snapMode = value;
            }
        }

        private Point[] GetGatesRotatedPoints(Gate g)
        {
            List<Point> aux = new List<Point>();

            Point[] normalPoints = g.GetDotPoints();

            foreach (Point p in normalPoints)
            {
                Point newPoint = LinePointUtils.RotatePoint(
                    p,
                    g.Location,
                    g.Angle);

                aux.Add(newPoint);
            }

            return aux.ToArray();
        }

        private Point SnapPoint(Point p)
        {
            if (!SnapMode)
            {
                return p;
            }

            List<Point> pointList = new List<Point>();
            Stack<Point> pointsWithMinDistance1 = new Stack<Point>();
            Stack<Point> pointsWithMinDistance2 = new Stack<Point>();

            foreach (Gate gate in circuit.Gates)
            {
                Point[] aux = GetGatesRotatedPoints(gate);

                pointList.AddRange(aux);
            }

            foreach (Wire wire in circuit.Wires)
            {
                pointList.AddRange(wire.GetWirePointsLocation());
            }

            

            int minimumDistance=SnapTolerance + 1;
            Point resultingPoint = p;

            foreach (Point currentPoint in pointList)
            {

                int distance1=Math.Abs(currentPoint.X - p.X);
                int distance2=Math.Abs(currentPoint.Y - p.Y);

                if (distance1 <= SnapTolerance)
                {
                    if (distance1 < minimumDistance)
                    {
                        minimumDistance = distance1;

                        resultingPoint = new Point(
                            currentPoint.X,p.Y);

                        pointsWithMinDistance1.Clear();
                    }
                }

                if (distance2 <= SnapTolerance)
                {
                    if (distance2 < minimumDistance)
                    {
                        minimumDistance = distance2;

                        resultingPoint = new Point(
                            p.X, currentPoint.Y);

                        pointsWithMinDistance2.Clear();
                    }
                    
                }

                if (distance1 == minimumDistance)
                {
                    pointsWithMinDistance1.Push(resultingPoint);
                }

                if (distance2 == minimumDistance)
                {
                    pointsWithMinDistance2.Push(resultingPoint);
                }
            }
            
            if (pointsWithMinDistance1.Count != 0 && pointsWithMinDistance2.Count!=0)
            {
                resultingPoint = new Point(
                    pointsWithMinDistance1.Peek().X,
                    pointsWithMinDistance2.Peek().Y);
            }
            
            return resultingPoint;
        }

        #endregion
    }
}
