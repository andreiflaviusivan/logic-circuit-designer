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
using LCD.Components.Abstract;
using LCD.UndoRedo;


namespace LCD.Components
{
    [Serializable]
    public class Circuit:IUndoRedo
    {
        public List<Gate> Gates { get; set; }
        public List<Wire> Wires { get; set; }

        [NonSerialized]
        private UndoRedoObject<List<Gate>> undoRedoGates = new UndoRedoObject<List<Gate>>();
        [NonSerialized]
        private UndoRedoObject<List<Wire>> undoRedoWires = new UndoRedoObject<List<Wire>>();
        
        public Circuit()
        {
            Gates = new List<Gate>();
            Wires = new List<Wire>();
        }

        private void InitializeUndoRedo()
        {
            undoRedoGates = new UndoRedoObject<List<Gate>>();
            undoRedoWires = new UndoRedoObject<List<Wire>>();
        }

        public void Simulate()
        {
            
            
            foreach (Gate g in Gates)
            {
                g.Simulate();
            }
        }

        public int CountButtons()
        {
            int nr = 0;
            foreach (Gate g in Gates)
                if (g is Btn)
                    nr++;
            return nr;
        }

        public int CountLeds()
        {
            int nr = 0;
            foreach (Gate g in Gates)
                if (g is Led)
                    nr++;
            return nr;
        }

        public Btn GetButton(int idx)
        {
            int nr = 0;
            foreach (Gate g in Gates)
                if (g is Btn)
                {
                    if (nr == idx)
                        return (Btn)g;
                    nr++;
                }
            return null;
        }

        public Led GetLed(int idx)
        {
            int nr = 0;
            foreach (Gate g in Gates)
                if (g is Led)
                {
                    if (nr == idx)
                        return (Led)g;
                    nr++;
                }
            return null;
        }

        public Boolean AlreadyConnected(Dot source, Dot destination)
        {
            foreach (Wire wire in Wires)
            {
                if (wire.dst == source && wire.src == destination ||
                    wire.dst == destination && wire.src == source)
                {
                    return true;
                }
            }

            return false;
        }

        #region Selected Components

        public Gate[] SelectedGates
        {
            get
            {
                List<Gate> selectedGateList = new List<Gate>();

                foreach (Gate gate in Gates)
                {
                    if (gate.Selected)
                    {
                        selectedGateList.Add(gate);
                    }
                }

                return selectedGateList.ToArray();
            }
        }

        public Wire[] SelectedWires
        {
            get
            {
                List<Wire> selectedWireList = new List<Wire>();

                foreach (Wire wire in Wires)
                {
                    if (wire.Selected)
                    {
                        selectedWireList.Add(wire);
                    }
                }

                return selectedWireList.ToArray();
            }
        }

        public Wire[] GetWiresConnectedTo(Gate g)
        {
            List<Wire> wireList = new List<Wire>();

            foreach (Wire wire in Wires)
            {
                if (wire.dst.Parent == g || wire.src.Parent == g)
                {
                    wireList.Add(wire);
                }
            }

            return wireList.ToArray();
        }

        public Wire[] GetSelectedWiresConnectedTo(Gate g)
        {
            List<Wire> wireList = new List<Wire>();

            foreach (Wire wire in Wires)
            {
                if (wire.dst.Parent == g || wire.src.Parent == g)
                {
                    if (wire.Selected)
                    {
                        wireList.Add(wire);
                    }
                }
            }

            return wireList.ToArray();
        }

        #endregion

        #region IUndoRedo Members

        public void Undo()
        {
            
            if (undoRedoGates == null || undoRedoWires == null)
            {
                InitializeUndoRedo();
            }

            List<Gate> undoGateList = undoRedoGates.Undo();

            if (undoGateList != null)
            {
                this.Gates = new List<Gate>(undoGateList);
            }

            List<Wire> undoWireList = undoRedoWires.Undo();

            if (undoWireList != null)
            {
                this.Wires = new List<Wire>(undoWireList);
            }
            
        }

        public void Redo()
        {
            
            if (undoRedoGates == null || undoRedoWires == null)
            {
                InitializeUndoRedo();
            }

            List<Gate> redoGateList = undoRedoGates.Redo();

            if (redoGateList != null)
            {
                this.Gates = new List<Gate>(redoGateList);
            }

            List<Wire> redoWireList = undoRedoWires.Redo();

            if (redoWireList != null)
            {
                this.Wires = new List<Wire>(redoWireList);
            }
            
        }

        public void SaveState()
        {
            
            List<Gate> newGateList = new List<Gate>(Gates);
            List<Wire> newWireList = new List<Wire>(Wires);

            if (undoRedoGates == null ||
                undoRedoWires == null)
            {
                InitializeUndoRedo();
            }

            undoRedoGates.SaveState(newGateList);
            undoRedoWires.SaveState(newWireList);
            
        }

        #endregion
    }
}