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
using System.Windows.Forms;
using LCD.Interface;
using Microsoft.VisualBasic.ApplicationServices;


namespace LCD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*string[] commandLine = Environment.GetCommandLineArgs();
            if(commandLine.Contains("/associate"))
            {
                if (VistaSecurity.IsAdmin())
                {
                    LCD_Settings.Associate();
                }
                Application.Exit();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Application.Run(new Interface.LCD());*/
            if (args.Contains("/associate"))
            {
                if (VistaSecurity.IsAdmin())
                {
                    LCD_Settings.Associate();
                }
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                LCDProgram program = new LCDProgram();
                program.Run(args);
            }
        }

        class LCDProgram : WindowsFormsApplicationBase
        {
            public LCDProgram()
            {
                this.IsSingleInstance = true;
                this.ShutdownStyle = ShutdownMode.AfterMainFormCloses;
            }

            protected override void OnCreateMainForm()
            {
                this.MainForm = new Interface.LCD();
            }

            protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            {
                ((Interface.LCD)MainForm).FileOpen(eventArgs.CommandLine.ToArray());
            }
        }
    }
}
