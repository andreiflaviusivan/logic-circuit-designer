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
using LCD.Properties;
using Microsoft.Win32;

namespace LCD.Interface
{
    public partial class LCD_Settings : Form
    {
        String tooltipTimeoutFormat = "Description tooltip timeout ({0} sec)";

        public delegate void ApplyButtonClickHandler(object sender, EventArgs e);
        public ApplyButtonClickHandler ApplyButtonClick;

        public LCD_Settings()
        {
            InitializeComponent();

            try
            {
                LoadSettings();
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "The following error occured: \"" + e.Message+"\"\nThe fail-safe settings will be loaded instead.",
                    "Setting error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                LoadFailSafeSettings();
            }
            if (!VistaSecurity.IsAdmin())
            {
                VistaSecurity.AddShieldToButton(btnAssociate);
            }
        }

        private void LoadSettings()
        {
            numericUpDownDotRadius.Value = Settings.Default.DotRadius;
            numericUpDownWirePointRadius.Value = Settings.Default.WirePointRadius;

            panelDotOnColor.BackColor = Settings.Default.DotOnColor;
            panelDotOffColor.BackColor = Settings.Default.DotOffColor;

            panelWireOnColor.BackColor = Settings.Default.WireOnColor;
            panelWireOffColor.BackColor = Settings.Default.WireOffColor;

            panelWirePointColor.BackColor = Settings.Default.WirePointColor;
            panelCircuitBackColor.BackColor = Settings.Default.CircuitBackColor;

            trackBarToolTipTimeout.Value = (int)Settings.Default.DescriptionToolTipTimeOut;

            trackBarToolTipTimeout_Scroll(trackBarToolTipTimeout, new EventArgs());

            checkBoxSnapEnabled.Checked = Settings.Default.SnapEnabled;
            numericUpDownSnapTolerance.Value = Settings.Default.SnapTolerance;
        }

        private void LoadFailSafeSettings()
        {
            numericUpDownDotRadius.Value = numericUpDownDotRadius.Minimum;
            numericUpDownWirePointRadius.Value = numericUpDownWirePointRadius.Minimum;

            panelDotOnColor.BackColor = Color.Blue;
            panelDotOffColor.BackColor = Color.Red;

            panelWireOnColor.BackColor = Color.Firebrick;
            panelWireOffColor.BackColor = Color.Black;

            panelWirePointColor.BackColor = Color.DarkSalmon;
            panelCircuitBackColor.BackColor = Color.White;

            trackBarToolTipTimeout.Value = 2;
            
            trackBarToolTipTimeout_Scroll(trackBarToolTipTimeout, new EventArgs());

            checkBoxSnapEnabled.Checked = false;
            numericUpDownSnapTolerance.Value = numericUpDownSnapTolerance.Minimum;
        }

        private void SaveSettings()
        {
            Settings.Default.DotRadius=(int)numericUpDownDotRadius.Value;
            Settings.Default.WirePointRadius=(int)numericUpDownWirePointRadius.Value;

            Settings.Default.DotOnColor=panelDotOnColor.BackColor;
            Settings.Default.DotOffColor=panelDotOffColor.BackColor;

            Settings.Default.WireOnColor=panelWireOnColor.BackColor;
            Settings.Default.WireOffColor=panelWireOffColor.BackColor;

            Settings.Default.WirePointColor=panelWirePointColor.BackColor;
            Settings.Default.CircuitBackColor=panelCircuitBackColor.BackColor;

            Settings.Default.DescriptionToolTipTimeOut=trackBarToolTipTimeout.Value;

            Settings.Default.SnapEnabled = checkBoxSnapEnabled.Checked;
            Settings.Default.SnapTolerance = (int)numericUpDownSnapTolerance.Value;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            SaveSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            SaveSettings();

            if (ApplyButtonClick != null)
            {
                ApplyButtonClick(this, e);
            }
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                Panel currentPanel = (Panel)sender;

                ColorDialog colorDialog = new ColorDialog();

                colorDialog.Color = currentPanel.BackColor;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    currentPanel.BackColor = colorDialog.Color;
                }
            }
        }

        private void trackBarToolTipTimeout_Scroll(object sender, EventArgs e)
        {
            String text = String.Format(tooltipTimeoutFormat, trackBarToolTipTimeout.Value);

            labelDescriptionToolTipTimeout.Text = text;
        }

        private void btnAssociate_Click(object sender, EventArgs e)
        {
            if (VistaSecurity.IsAdmin())
            {
                Associate();
            }
            else
            {
                AssociateAsAdmin();
            }
        }

        private void AssociateAsAdmin()
        {
            VistaSecurity.RestartElevatedAndAssociate();
        }

        public static void Associate()
        {
            RegistryKey Key = Registry.ClassesRoot;
            string[] s = Key.GetSubKeyNames();

            if (!s.Contains(".circuit"))
            {
                Key.CreateSubKey(".circuit");
                Key = Key.OpenSubKey(".circuit", true);
                Key.DeleteValue("(Default)", false);
                Key.SetValue("", "LCDFiles");
                Key = Registry.ClassesRoot.CreateSubKey("LCDFiles");
                Key = Registry.ClassesRoot.OpenSubKey("LCDFiles", true);
                Key.SetValue("", "Logic Circuit Designer File");
                Key.CreateSubKey("shell");
                Key.CreateSubKey("DefaultIcon");
                Key.OpenSubKey("DefaultIcon", true).SetValue("", Application.ExecutablePath);
                Key = Key.OpenSubKey("shell", true);
                Key.CreateSubKey("open");
                Key = Key.OpenSubKey("open", true);
                Key.CreateSubKey("command");
                Key = Key.OpenSubKey("command", true);
                Key.SetValue("", (char)34 + Application.ExecutablePath + (char)34 + " " + (char)34 + "%L" + (char)34);

                MessageBox.Show("The application was successfully associated with \".circuit\" files",
                    "Association successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                Key = Key.OpenSubKey(".circuit", true);
                object o = Key.GetValue("");
                string aux = (String)o;

                if (aux != "LCDFiles")
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(".circuit");
                    Associate();
                }
                else
                {
                    MessageBox.Show("The application was already associated with \".circuit\" files",
                        "Association successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

    }
}