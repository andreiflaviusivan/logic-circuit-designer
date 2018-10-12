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
using LCD.Components;
using System.Threading;

namespace LCD.Interface
{
    public class Simulate
    {
        public Circuit c;
        public CircuitView cw = null;
        public bool isRunning;

        public Simulate()
        {
            isRunning = false;
        }
        public Simulate(Circuit c)
            : this()
        {
            this.c = c;
        }

        public Simulate(Circuit c,CircuitView cw)
            : this(c)
        {
            this.cw = cw;
        }

        public Simulate(CircuitView cw)
            : this(cw.circuit, cw) { }

        public void Sim()
        {
            while (isRunning)
                lock (cw)
                {
                    c.Simulate();
                    //c.Simulate();

                    //if (cw != null)
                        cw.RedrawGates();
                    Thread.Sleep(100);
                }
        }

        public void start()
        {
            isRunning = true;
            Thread t = new Thread(new ThreadStart(Sim));
            t.Start();
            if (cw != null)
                cw.Simulating = true;
        }

        public void stop()
        {
            isRunning = false;
            if (cw != null)
                cw.Simulating = false;
        }
    }
}
