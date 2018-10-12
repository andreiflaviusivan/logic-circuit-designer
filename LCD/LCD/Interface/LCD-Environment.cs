using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using LCD.Properties;

namespace LCD.Interface
{
    public partial class LCD:Form
    {
        #region Environment

        private const int WM_COPYDATA = 0x4A;
        protected const string mutexName="Logic Circuit Designer mutex";
        static bool createdNew;
        Mutex mutex = new Mutex(true, mutexName, out createdNew);
        private int retries = 0;

     
        //Used for WM_COPYDATA for string messages
        protected struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        protected int sendWindowsStringMessage(int hWnd, int wParam, string msg)
        {
            int result = 0;

            if (hWnd > 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = msg;
                cds.cbData = len + 1;
                result = SendMessage(hWnd, WM_COPYDATA, wParam, ref cds);
            }
            return result;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(int hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam);

        protected Process GetFirstStartedProcess(Process [] processArray)
        {
            if (processArray == null || processArray.Length == 0)
            {
                return null;
            }

            Process tempProcess = processArray[0];

            foreach (Process process in processArray)
            {
                if (process.StartTime.CompareTo(tempProcess.StartTime)==-1)
                {
                    tempProcess = process;
                }
            }

            return tempProcess;
        }

        protected Process PriorProcess()
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_COPYDATA:
                    
                    COPYDATASTRUCT CD =
                        (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                    
                    string strData = CD.lpData;
                    //strDATA e chestia primita
                    FileOpen(new String[] { strData });

                    this.Activate();
                    this.Focus();
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void ProcessCMDParameters()
        {
            string[] commandLine = Environment.GetCommandLineArgs();

            if (commandLine.GetLength(0) != 1)
            {
                String[] fileArray = new String[commandLine.GetLength(0) - 1];

                for (int i = 1; i < commandLine.Length; i++)
                {
                    fileArray[i - 1] = commandLine[i];
                }

                FileOpen(fileArray);
            }
            else
            {
                FileNew();
            }
        }

        //Must be called in the constructor
        private void HandleMutexOperations()
        {
            if (createdNew == false)
            {
                //Deja exista

                string[] par = Environment.GetCommandLineArgs();

                Process currentProcess = Process.GetCurrentProcess();
                Process[] processCollection;
                processCollection = Process.GetProcessesByName(currentProcess.ProcessName);

                Process firstStartedProcess;
                firstStartedProcess = GetFirstStartedProcess(processCollection);

                foreach (string str in par)
                {
                    if (str != par[0])
                    {
                        while (firstStartedProcess.MainWindowHandle.ToInt32()==0 && retries<5)
                        {
                            Thread.Sleep(1000);
                            processCollection = Process.GetProcessesByName(currentProcess.ProcessName);
                            firstStartedProcess = GetFirstStartedProcess(processCollection);
                            retries++;
                        }
                        sendWindowsStringMessage(firstStartedProcess.MainWindowHandle.ToInt32(), 0, str);
                    }
                }

                Application.Exit();

                Process pp = Process.GetCurrentProcess();
                pp.Kill();
            }
            else
            {
                GC.KeepAlive(mutex);
            }
        }

        #endregion
    }
}
