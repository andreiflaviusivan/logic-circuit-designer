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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LCD.Components.Abstract;
using System.IO;
using LCD.Properties;
using System.Diagnostics;

namespace LCD.Interface
{
    public partial class LCD : Form
    {
        private Simulate simulate=null;

        public LCD()
        {
            InitializeComponent();
            tsNrInputs.SelectedIndex = 0;
            ProcessCMDParameters();
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

        private String GetNewTabPageName()
        {
            int value = 1;
            bool found = false;
            String circuitName="Circuit";
            
            while (!found)
            {
                circuitName = String.Format("Circuit{0}", value);

                found = true;

                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    if (circuitName.Equals(tabPage.Text))
                    {
                        value++;

                        found = false;

                        break;
                    }
                }
            }

            return circuitName;
        }

        private void CreateNewTabPage(String currentFileName)
        {
            if(!File.Exists(currentFileName))
            {
                MessageBox.Show("The file name received (" + currentFileName + ") points to an innexistent file",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CircuitView cv = new CircuitView(currentFileName);

            NoMousewheelTabPage t = new NoMousewheelTabPage(GetNewTabPageName());
            
            t.AutoScroll = true;
            t.BackColor = Settings.Default.CircuitBackColor;
            t.Text = Path.GetFileNameWithoutExtension(currentFileName);

            tabControl.TabPages.Add(t);
            
            cv.Size = t.Size;
            cv.OnGateSelected += new CircuitView.GateSelectedEvent(cv_OnGateSelected);
            cv.Location = new Point(0, 0);

            cv.Dock = DockStyle.None;
            t.Controls.Add(cv);

            tabControl.SelectedTab = t;

            cv.ResizeAtLoad();
        }

        #region Add gates

        private void AddGatesAnd()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new And(tsNrInputs.SelectedIndex+2, new Point(10, 10)));
        }

        private void AddGatesOr()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Or(tsNrInputs.SelectedIndex + 2, new Point(10, 10)));
        }

        private void AddGatesNot()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Not(new Point(10, 10)));
        }

        private void AddGatesXor()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Xor(tsNrInputs.SelectedIndex + 2, new Point(10, 10)));
        }

        private void AddGatesNand()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Nand(tsNrInputs.SelectedIndex + 2, new Point(10, 10)));
        }

        private void AddGatesNor()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Nor(tsNrInputs.SelectedIndex + 2, new Point(10, 10)));
        }
        private void AddGatesNxor()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Nxor(tsNrInputs.SelectedIndex + 2, new Point(10, 10)));
        }

        private void AddGatesBtn()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Btn(new Point(10, 10)));
        }

        private void AddGatesLed()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Led(new Point(10, 10)));
        }

        private void AddGatesClk()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Clk(new Point(10, 10)));
        }

        private void AddGatesInput()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new Input(new Point(10, 10)));
        }

        private void AddGatesSSD()
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(new SSD(new Point(10, 10)));
        }

        private void AddGatesMod()
        {
            if (tsSimulate.Checked)
            {
                return;
            }

            if (tabControl.SelectedTab != null)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Multiselect = true;

                openDialog.Title = "Select any circuit to use it as a module";
                openDialog.InitialDirectory=
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
                    "Circuits";

                openDialog.Filter = "Circuit files(*.circuit)|*.circuit";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < openDialog.FileNames.GetLength(0); i++)
                    {
                        try
                        {
                            Gate g = new Module((String)openDialog.FileNames.GetValue(i), new Point(10, i * 10));

                            (tabControl.SelectedTab.Controls[0] as CircuitView).AddFloatingGate(g
                                );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                    ex.Message,
                                    "Error creating module from " + openDialog.FileNames.GetValue(i),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                        }
                    }
                }

            }
                
        }

        #endregion

        void cv_OnGateSelected(Gate[] gateArray)
        {
            propertyGrid.SelectedObjects = gateArray;
        }

        private void tsSimulate_CheckStateChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null)
            {
                tsSimulate.Checked = false;
                return;
            }
            if (simulate != null)
            {
                simulate.stop();
                simulate = null;
            }
            if (tsSimulate.Checked)
            {
                simulate = new Simulate(tabControl.SelectedTab.Controls[0] as CircuitView);
                simulate.start();

                propertyGrid.SelectedObject = null;
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).ClearBackground();

            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).RedrawGates();
            

            
        }

        //Verify the saved status for the current tab
        //returns false if the close tab event should be canceled
        private bool VerifySavedStatus()
        {
            CircuitView selected = null;
            if (tabControl.SelectedTab != null)
            {
                try
                {
                    selected = tabControl.SelectedTab.Controls[0] as CircuitView;
                }
                catch
                {
                    return true;
                }

                if (!selected.Saved)
                {
                    String text=
                        String.Format("The circuit '{0}' has not been saved. Do you want to save it?",
                        tabControl.SelectedTab.Text);

                    DialogResult result =
                        MessageBox.Show(
                        text,
                        "Question",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        FileSave();
                    }

                    if (result == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Verifies the saved status for all tabs
        private bool VerifySavedStatusAll()
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabControl.SelectedTab = tabPage;

                if (!VerifySavedStatus())
                {
                    return false;
                }
            }

            return true;
        }

        #region AddGates Toolstrip and menu

        private void aNDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesAnd();
        }

        private void oRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesOr();
        }

        private void nOTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesNot();
        }

        private void xORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesXor();
        }

        private void nANDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesNand();
        }

        private void nORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesNor();
        }

        private void nXORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesNxor();
        }

        private void buttonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesBtn();
        }

        private void lEDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesLed();
        }

        private void clockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGatesClk();
        }

        private void tsInput_Click(object sender, EventArgs e)
        {
            AddGatesInput();
        }

        private void tsSSD_Click(object sender, EventArgs e)
        {
            AddGatesSSD();
        }

        private void tsModule_Click(object sender, EventArgs e)
        {
            AddGatesMod();
        }

        #endregion

        #region Tab menu methods

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FileNew();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseTab();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CloseAll();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllButActive();
        }

        private void contextMenuStripTabControl_Opening(object sender, CancelEventArgs e)
        {
            if (!AnotherTabPageWasSelected())
            {
                e.Cancel = true;
            }
        }

        private bool CloseTab()
        {
            bool acceptClose = VerifySavedStatus();

            if (acceptClose && tabControl.TabPages.Count!=0)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }

            return acceptClose;
        }

        private void CloseAll()
        {
            int loop = 0;

            while (loop < tabControl.TabPages.Count)
            {
                tabControl.SelectedTab = tabControl.TabPages[loop];

                if (!CloseTab())
                {
                    loop++;
                }
            }
        }

        private void CloseAllButActive()
        {
            int loop = 0;
            TabPage activeTab = tabControl.SelectedTab;

            while (loop < tabControl.TabPages.Count)
            {
                if (tabControl.TabPages[loop] == activeTab)
                {
                    loop++;

                    continue;
                }

                tabControl.SelectedTab = tabControl.TabPages[loop];

                if (!CloseTab())
                {
                    loop++;
                }
            }
        }

        private void NextTab()
        {
            if (tabControl.TabPages.Count != 0)
            {
                int currentIndex = tabControl.SelectedIndex;

                if (currentIndex < tabControl.TabPages.Count - 1)
                {
                    currentIndex++;
                }

                tabControl.SelectedTab = tabControl.TabPages[currentIndex];
            }
        }

        private void PreviousTab()
        {
            if (tabControl.TabPages.Count != 0)
            {
                int currentIndex = tabControl.SelectedIndex;

                if (currentIndex > 0)
                {
                    currentIndex--;
                }

                tabControl.SelectedTab = tabControl.TabPages[currentIndex];
            }
        }
        private bool AnotherTabPageWasSelected()
        {
            TabPage clickedTabPage = HoverTab();

            if (clickedTabPage == null)
            {
                return false;
            }
            else
            {
                tabControl.SelectedTab = clickedTabPage;

                return true;
            }
        }

        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            AnotherTabPageWasSelected();

            if (e.Button == MouseButtons.Middle)
            {
                CloseTab();
            }

            if (e.Delta > 0)
            {
                NextTab();
            }

            if (e.Delta < 0)
            {
                PreviousTab();
            }

        }

        private TabPage HoverTab()
        {
            for (int index = 0; index <= tabControl.TabCount - 1; index++)
            {
                if (tabControl.GetTabRect(index).Contains(tabControl.PointToClient(Cursor.Position)))
                {
                    return tabControl.TabPages[index];
                }
            }
            return null;
        }

        #endregion

        #region File toolstrip functions

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileOpen();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSaveAs();
        }

        private void toolStripMenuItemSaveAll_Click(object sender, EventArgs e)
        {
            FileSaveAll();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).Print();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).PrintPreview();
        }

        private void FileNew()
        {
            CircuitView cv = new CircuitView();
            NoMousewheelTabPage tp = new NoMousewheelTabPage(GetNewTabPageName());

            tp.Size = tabControl.Size;
            cv.Size = tabControl.Size;

            tp.AutoScroll = true;
            tp.BackColor = Settings.Default.CircuitBackColor;
            tp.Controls.Add(cv);
            //cv.Dock = DockStyle.Fill;
            cv.OnGateSelected += new CircuitView.GateSelectedEvent(cv_OnGateSelected);
            tabControl.TabPages.Add(tp);
            
            tabControl.SelectedTab = tp;
        }

        private void FileSave()
        {
            CircuitView selected = null;
            if (tabControl.SelectedTab != null)
            {
                selected = tabControl.SelectedTab.Controls[0] as CircuitView;
                if (!String.IsNullOrEmpty(selected.FileName))
                    selected.DumpToDisk(selected.FileName);
                else
                    FileSaveAs();
            }
        }

        private void FileSaveAs()
        {
            CircuitView selected = null;
            if (tabControl.SelectedTab != null)
            {
                selected = tabControl.SelectedTab.Controls[0] as CircuitView;
            }
            if (selected == null) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "Circuits";
            sfd.Filter = "Circuit files(*.circuit)|*.circuit";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                selected.DumpToDisk(sfd.FileName);
                selected.FileName = sfd.FileName;

                tabControl.SelectedTab.Text = 
                    Path.GetFileNameWithoutExtension(sfd.FileName);
            }
            sfd.Dispose();
        }

        private void FileSaveAll()
        {
            TabPage currentSelectedTab = tabControl.SelectedTab;

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabControl.SelectedTab = tabPage;

                FileSave();
            }

            tabControl.SelectedTab = currentSelectedTab;
        }

        private void FileOpen()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "Circuits";
            openDialog.Filter = "Circuit files(*.circuit)|*.circuit";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;
            openDialog.Multiselect = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {

                for (int i = 0; i < openDialog.FileNames.GetLength(0); i++)
                {
                    String currentFileName = (String)openDialog.FileNames.GetValue(i);

                    Boolean wasAlreadyOpened = false;

                    // sa nu deschida de 2 ori acelasi fisier
                    foreach (TabPage tp in tabControl.TabPages)
                    {
                        if (tp.Controls[0] is CircuitView)
                        {
                            if ((tp.Controls[0] as CircuitView).FileName!=null && 
                                (tp.Controls[0] as CircuitView).FileName.Equals(currentFileName))
                            {
                                tabControl.SelectedTab = tp;

                                wasAlreadyOpened = true;

                                break;
                            }
                        }
                    }

                    if (!wasAlreadyOpened)
                    {
                        
                        try
                        {
                            CreateNewTabPage(currentFileName);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(
                                ex.Message,
                                "Error opening "+currentFileName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        
                    }
                }
            }
            openDialog.Dispose();
        }

        public void FileOpen(String[] fileArray)
        {
            if (fileArray == null)
            {
                return;
            }

            for (int i = 0; i < fileArray.GetLength(0); i++)
            {
                String currentFileName = fileArray[i];
                if (fileArray[i] == "/associate") continue;
                Boolean wasAlreadyOpened = false;

                // sa nu deschida de 2 ori acelasi fisier
                foreach (TabPage tp in tabControl.TabPages)
                {
                    if (tp.Controls[0] is CircuitView)
                    {
                        if ((tp.Controls[0] as CircuitView).FileName!=null && 
                            (tp.Controls[0] as CircuitView).FileName.Equals(currentFileName))
                        {
                            tabControl.SelectedTab = tp;
                            wasAlreadyOpened = true;
                            break;
                        }
                    }
                }

                if (!wasAlreadyOpened)
                {
                    CreateNewTabPage(currentFileName);
                }
            }
        }

        #endregion
       

        #region Selection Methods

        private void gatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).SelectAllGates();
            }
        }

        private void wiresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).SelectAllWires();
            }
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).SelectAll();
            }
        }

        private void gatesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).SelectNoneGates();
            }
        }

        private void wiresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).SelectNoneWires();
            }
        }

        private void allToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).SelectNone();
            }
        }

        private void gatesToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).InvertGatesSelection();
            }
        }

        private void wiresToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).InvertWiresSelection();
            }
        }

        private void allToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).InvertSelection();
            }
        }

        private void gaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).CropGatesSelection();
            }
        }

        private void wiresToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).CropWiresSelection();
            }
        }

        private void allToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).CropSelection();
            }
        }

        private void gatesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).DeleteGatesSelection();
            }
        }

        private void wiresToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).DeleteWiresSelection();
            }
        }

        private void allToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).DeleteSelection();
            }
        }

        #endregion

        #region Align Methods

        private void toolStripDropDownButtonAlignLeftSides_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).AlignLeftSides();
            }
        }

        private void toolStripDropDownButtonAlignVerticalCenters_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).AlignVerticalCenters();
            }
        }

        private void toolStripDropDownButtonAlignRightSides_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).AlignRightSides();
            }
        }

        private void toolStripDropDownButtonAlignTopEdges_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).AlignTopEdges();
            }
        }

        private void toolStripDropDownButtonAlignHorizontalCenters_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).AlignHorizontalSides();
            }
        }

        private void toolStripDropDownButtonAlignBottomEdges_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).AlignBottomEdges();
            }
        }

        #endregion

        #region Cut Copy Paste Undo Redo

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).Cut();
            }
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).Copy();
            }
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).Paste();
            }
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).Undo();
            }
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                (tabControl.SelectedTab.Controls[0] as CircuitView).Redo();
            }
        }

        #endregion

        private void LCD_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (simulate != null)
            {
                simulate.stop();
                simulate = null;
            }

            if (!VerifySavedStatusAll())
            {
                e.Cancel = true;
            }
        }

        private void LCD_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }

        #region options

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOptions();
        }

        private void ShowOptions()
        {
            LCD_Settings lcdSettings = new LCD_Settings();

            lcdSettings.ApplyButtonClick += new LCD_Settings.ApplyButtonClickHandler(ButtonApply_Click);
            lcdSettings.ShowDialog();

            RedrawAll();
        }

        private void RedrawAll()
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                (tabPage.Controls[0] as CircuitView).ClearBackground();
                (tabPage.Controls[0] as CircuitView).RedrawGates();
                (tabPage.Controls[0] as CircuitView).UpdateSnapSettings();
            }
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            RedrawAll();
        }
        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LCDAbout aboutBox = new LCDAbout();

            aboutBox.ShowDialog();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (simulate != null)
            {
                simulate.stop();
                simulate = null;
            }
            tsSimulate.Checked = false;
        }

        private void tabControl_SizeChanged(object sender, EventArgs e)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.Size = tabControl.Size;

                (tabPage.Controls[0] as CircuitView).ResizeAtLoad();
            }
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("Help\\help.html");
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message,
                    "Launch Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        

        #region Move gates by direction keys

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).MoveSelectedGatesUp();
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).MoveSelectedGatesDown();
        }

        private void moveLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).MoveSelectedGatesLeft();
        }

        private void moveRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
                (tabControl.SelectedTab.Controls[0] as CircuitView).MoveSelectedGatesRight();
        }

        #endregion



    }
}
