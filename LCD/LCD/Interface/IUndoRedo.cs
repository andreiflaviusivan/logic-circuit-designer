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