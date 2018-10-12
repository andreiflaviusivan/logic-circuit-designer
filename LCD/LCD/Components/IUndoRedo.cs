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

namespace LCD.UndoRedo
{
    public interface IUndoRedo
    {
        void Undo();
        void Redo();
        void SaveState();
    }

    public interface IUndoRedoAbstract<T>
    {
        T Undo();
        T Redo();
        void SaveState(T currentState);
    }

    public class UndoRedoObject<T> : IUndoRedoAbstract<T> where T:class
    {
        [NonSerialized]
        private Stack<T> undoStack = new Stack<T>();
        [NonSerialized]
        private Stack<T> redoStack = new Stack<T>();
        [NonSerialized]
        private T currentState;
        
        public UndoRedoObject()
        {

        }

        #region IUndoRedo<T> Members

        public T Undo()
        {
            if (undoStack.Count != 0)
            {
                T returnValue = undoStack.Pop();

                redoStack.Push(currentState);

                currentState = returnValue;

                return returnValue;
            }
            else
            {
                return default(T);
            }
        }

        public T Redo()
        {
            if (redoStack.Count != 0)
            {
                T returnValue= redoStack.Pop();

                undoStack.Push(currentState);
                
                currentState = returnValue;

                return returnValue;
            }
            else
            {
                return default(T);
            }
        }

        public void SaveState(T currentState)
        {
            if (this.currentState != default(T))
            {
                undoStack.Push(this.currentState);
            }
                
            this.currentState = currentState;

            redoStack.Clear();
        }

        #endregion
    }
}