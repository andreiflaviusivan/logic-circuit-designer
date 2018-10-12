namespace LCD.Interface
{
    partial class LCD_Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LCD_Settings));
            this.groupBoxComponents = new System.Windows.Forms.GroupBox();
            this.numericUpDownWirePointRadius = new System.Windows.Forms.NumericUpDown();
            this.labelWirePointRadius = new System.Windows.Forms.Label();
            this.numericUpDownDotRadius = new System.Windows.Forms.NumericUpDown();
            this.labelDotRadius = new System.Windows.Forms.Label();
            this.groupBoxColors = new System.Windows.Forms.GroupBox();
            this.panelCircuitBackColor = new System.Windows.Forms.Panel();
            this.labelCircuitBackColor = new System.Windows.Forms.Label();
            this.panelWirePointColor = new System.Windows.Forms.Panel();
            this.labelWirePointColor = new System.Windows.Forms.Label();
            this.panelWireOffColor = new System.Windows.Forms.Panel();
            this.labelWireOffColor = new System.Windows.Forms.Label();
            this.panelWireOnColor = new System.Windows.Forms.Panel();
            this.labelWireOnColor = new System.Windows.Forms.Label();
            this.panelDotOffColor = new System.Windows.Forms.Panel();
            this.labelDotOffColor = new System.Windows.Forms.Label();
            this.panelDotOnColor = new System.Windows.Forms.Panel();
            this.labelDotOnColor = new System.Windows.Forms.Label();
            this.groupBoxOther = new System.Windows.Forms.GroupBox();
            this.btnAssociate = new System.Windows.Forms.Button();
            this.trackBarToolTipTimeout = new System.Windows.Forms.TrackBar();
            this.labelDescriptionToolTipTimeout = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.groupBoxSnapOptions = new System.Windows.Forms.GroupBox();
            this.numericUpDownSnapTolerance = new System.Windows.Forms.NumericUpDown();
            this.labelSnapTolerance = new System.Windows.Forms.Label();
            this.checkBoxSnapEnabled = new System.Windows.Forms.CheckBox();
            this.groupBoxComponents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWirePointRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDotRadius)).BeginInit();
            this.groupBoxColors.SuspendLayout();
            this.groupBoxOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarToolTipTimeout)).BeginInit();
            this.groupBoxSnapOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSnapTolerance)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxComponents
            // 
            this.groupBoxComponents.Controls.Add(this.numericUpDownWirePointRadius);
            this.groupBoxComponents.Controls.Add(this.labelWirePointRadius);
            this.groupBoxComponents.Controls.Add(this.numericUpDownDotRadius);
            this.groupBoxComponents.Controls.Add(this.labelDotRadius);
            this.groupBoxComponents.Location = new System.Drawing.Point(12, 12);
            this.groupBoxComponents.Name = "groupBoxComponents";
            this.groupBoxComponents.Size = new System.Drawing.Size(358, 64);
            this.groupBoxComponents.TabIndex = 0;
            this.groupBoxComponents.TabStop = false;
            this.groupBoxComponents.Text = "Component sizes";
            // 
            // numericUpDownWirePointRadius
            // 
            this.numericUpDownWirePointRadius.Location = new System.Drawing.Point(272, 24);
            this.numericUpDownWirePointRadius.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownWirePointRadius.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownWirePointRadius.Name = "numericUpDownWirePointRadius";
            this.numericUpDownWirePointRadius.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownWirePointRadius.TabIndex = 3;
            this.numericUpDownWirePointRadius.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // labelWirePointRadius
            // 
            this.labelWirePointRadius.AutoSize = true;
            this.labelWirePointRadius.Location = new System.Drawing.Point(163, 26);
            this.labelWirePointRadius.Name = "labelWirePointRadius";
            this.labelWirePointRadius.Size = new System.Drawing.Size(92, 13);
            this.labelWirePointRadius.TabIndex = 2;
            this.labelWirePointRadius.Text = "Wire Point Radius";
            // 
            // numericUpDownDotRadius
            // 
            this.numericUpDownDotRadius.Location = new System.Drawing.Point(72, 24);
            this.numericUpDownDotRadius.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownDotRadius.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownDotRadius.Name = "numericUpDownDotRadius";
            this.numericUpDownDotRadius.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownDotRadius.TabIndex = 1;
            this.numericUpDownDotRadius.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // labelDotRadius
            // 
            this.labelDotRadius.AutoSize = true;
            this.labelDotRadius.Location = new System.Drawing.Point(6, 26);
            this.labelDotRadius.Name = "labelDotRadius";
            this.labelDotRadius.Size = new System.Drawing.Size(60, 13);
            this.labelDotRadius.TabIndex = 0;
            this.labelDotRadius.Text = "Dot Radius";
            // 
            // groupBoxColors
            // 
            this.groupBoxColors.Controls.Add(this.panelCircuitBackColor);
            this.groupBoxColors.Controls.Add(this.labelCircuitBackColor);
            this.groupBoxColors.Controls.Add(this.panelWirePointColor);
            this.groupBoxColors.Controls.Add(this.labelWirePointColor);
            this.groupBoxColors.Controls.Add(this.panelWireOffColor);
            this.groupBoxColors.Controls.Add(this.labelWireOffColor);
            this.groupBoxColors.Controls.Add(this.panelWireOnColor);
            this.groupBoxColors.Controls.Add(this.labelWireOnColor);
            this.groupBoxColors.Controls.Add(this.panelDotOffColor);
            this.groupBoxColors.Controls.Add(this.labelDotOffColor);
            this.groupBoxColors.Controls.Add(this.panelDotOnColor);
            this.groupBoxColors.Controls.Add(this.labelDotOnColor);
            this.groupBoxColors.Location = new System.Drawing.Point(12, 82);
            this.groupBoxColors.Name = "groupBoxColors";
            this.groupBoxColors.Size = new System.Drawing.Size(358, 94);
            this.groupBoxColors.TabIndex = 1;
            this.groupBoxColors.TabStop = false;
            this.groupBoxColors.Text = "Colors";
            // 
            // panelCircuitBackColor
            // 
            this.panelCircuitBackColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCircuitBackColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelCircuitBackColor.Location = new System.Drawing.Point(301, 54);
            this.panelCircuitBackColor.Name = "panelCircuitBackColor";
            this.panelCircuitBackColor.Size = new System.Drawing.Size(27, 23);
            this.panelCircuitBackColor.TabIndex = 11;
            this.panelCircuitBackColor.Click += new System.EventHandler(this.Panel_Click);
            // 
            // labelCircuitBackColor
            // 
            this.labelCircuitBackColor.AutoSize = true;
            this.labelCircuitBackColor.Location = new System.Drawing.Point(198, 60);
            this.labelCircuitBackColor.Name = "labelCircuitBackColor";
            this.labelCircuitBackColor.Size = new System.Drawing.Size(97, 13);
            this.labelCircuitBackColor.TabIndex = 10;
            this.labelCircuitBackColor.Text = "Circuit Background";
            // 
            // panelWirePointColor
            // 
            this.panelWirePointColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWirePointColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelWirePointColor.Location = new System.Drawing.Point(301, 19);
            this.panelWirePointColor.Name = "panelWirePointColor";
            this.panelWirePointColor.Size = new System.Drawing.Size(27, 23);
            this.panelWirePointColor.TabIndex = 9;
            this.panelWirePointColor.Click += new System.EventHandler(this.Panel_Click);
            // 
            // labelWirePointColor
            // 
            this.labelWirePointColor.AutoSize = true;
            this.labelWirePointColor.Location = new System.Drawing.Point(198, 25);
            this.labelWirePointColor.Name = "labelWirePointColor";
            this.labelWirePointColor.Size = new System.Drawing.Size(56, 13);
            this.labelWirePointColor.TabIndex = 8;
            this.labelWirePointColor.Text = "Wire Point";
            // 
            // panelWireOffColor
            // 
            this.panelWireOffColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWireOffColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelWireOffColor.Location = new System.Drawing.Point(148, 54);
            this.panelWireOffColor.Name = "panelWireOffColor";
            this.panelWireOffColor.Size = new System.Drawing.Size(27, 23);
            this.panelWireOffColor.TabIndex = 7;
            this.panelWireOffColor.Click += new System.EventHandler(this.Panel_Click);
            // 
            // labelWireOffColor
            // 
            this.labelWireOffColor.AutoSize = true;
            this.labelWireOffColor.Location = new System.Drawing.Point(101, 60);
            this.labelWireOffColor.Name = "labelWireOffColor";
            this.labelWireOffColor.Size = new System.Drawing.Size(46, 13);
            this.labelWireOffColor.TabIndex = 6;
            this.labelWireOffColor.Text = "Wire Off";
            // 
            // panelWireOnColor
            // 
            this.panelWireOnColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWireOnColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelWireOnColor.Location = new System.Drawing.Point(148, 19);
            this.panelWireOnColor.Name = "panelWireOnColor";
            this.panelWireOnColor.Size = new System.Drawing.Size(27, 23);
            this.panelWireOnColor.TabIndex = 5;
            this.panelWireOnColor.Click += new System.EventHandler(this.Panel_Click);
            // 
            // labelWireOnColor
            // 
            this.labelWireOnColor.AutoSize = true;
            this.labelWireOnColor.Location = new System.Drawing.Point(101, 25);
            this.labelWireOnColor.Name = "labelWireOnColor";
            this.labelWireOnColor.Size = new System.Drawing.Size(46, 13);
            this.labelWireOnColor.TabIndex = 4;
            this.labelWireOnColor.Text = "Wire On";
            // 
            // panelDotOffColor
            // 
            this.panelDotOffColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDotOffColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelDotOffColor.Location = new System.Drawing.Point(53, 54);
            this.panelDotOffColor.Name = "panelDotOffColor";
            this.panelDotOffColor.Size = new System.Drawing.Size(27, 23);
            this.panelDotOffColor.TabIndex = 3;
            this.panelDotOffColor.Click += new System.EventHandler(this.Panel_Click);
            // 
            // labelDotOffColor
            // 
            this.labelDotOffColor.AutoSize = true;
            this.labelDotOffColor.Location = new System.Drawing.Point(6, 60);
            this.labelDotOffColor.Name = "labelDotOffColor";
            this.labelDotOffColor.Size = new System.Drawing.Size(41, 13);
            this.labelDotOffColor.TabIndex = 2;
            this.labelDotOffColor.Text = "Dot Off";
            // 
            // panelDotOnColor
            // 
            this.panelDotOnColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDotOnColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelDotOnColor.Location = new System.Drawing.Point(53, 19);
            this.panelDotOnColor.Name = "panelDotOnColor";
            this.panelDotOnColor.Size = new System.Drawing.Size(27, 23);
            this.panelDotOnColor.TabIndex = 1;
            this.panelDotOnColor.Click += new System.EventHandler(this.Panel_Click);
            // 
            // labelDotOnColor
            // 
            this.labelDotOnColor.AutoSize = true;
            this.labelDotOnColor.Location = new System.Drawing.Point(6, 25);
            this.labelDotOnColor.Name = "labelDotOnColor";
            this.labelDotOnColor.Size = new System.Drawing.Size(41, 13);
            this.labelDotOnColor.TabIndex = 0;
            this.labelDotOnColor.Text = "Dot On";
            // 
            // groupBoxOther
            // 
            this.groupBoxOther.Controls.Add(this.btnAssociate);
            this.groupBoxOther.Controls.Add(this.trackBarToolTipTimeout);
            this.groupBoxOther.Controls.Add(this.labelDescriptionToolTipTimeout);
            this.groupBoxOther.Location = new System.Drawing.Point(12, 252);
            this.groupBoxOther.Name = "groupBoxOther";
            this.groupBoxOther.Size = new System.Drawing.Size(358, 123);
            this.groupBoxOther.TabIndex = 2;
            this.groupBoxOther.TabStop = false;
            this.groupBoxOther.Text = "Other";
            // 
            // btnAssociate
            // 
            this.btnAssociate.Location = new System.Drawing.Point(9, 65);
            this.btnAssociate.Name = "btnAssociate";
            this.btnAssociate.Size = new System.Drawing.Size(343, 38);
            this.btnAssociate.TabIndex = 3;
            this.btnAssociate.Text = "Associate with .circuit files";
            this.btnAssociate.UseVisualStyleBackColor = true;
            this.btnAssociate.Click += new System.EventHandler(this.btnAssociate_Click);
            // 
            // trackBarToolTipTimeout
            // 
            this.trackBarToolTipTimeout.Location = new System.Drawing.Point(166, 19);
            this.trackBarToolTipTimeout.Name = "trackBarToolTipTimeout";
            this.trackBarToolTipTimeout.Size = new System.Drawing.Size(186, 45);
            this.trackBarToolTipTimeout.TabIndex = 2;
            this.trackBarToolTipTimeout.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarToolTipTimeout.Scroll += new System.EventHandler(this.trackBarToolTipTimeout_Scroll);
            // 
            // labelDescriptionToolTipTimeout
            // 
            this.labelDescriptionToolTipTimeout.AutoSize = true;
            this.labelDescriptionToolTipTimeout.Location = new System.Drawing.Point(6, 26);
            this.labelDescriptionToolTipTimeout.Name = "labelDescriptionToolTipTimeout";
            this.labelDescriptionToolTipTimeout.Size = new System.Drawing.Size(154, 13);
            this.labelDescriptionToolTipTimeout.TabIndex = 0;
            this.labelDescriptionToolTipTimeout.Text = "Description tooltip timeout (sec)";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(12, 383);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(102, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(133, 383);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(261, 383);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(109, 23);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // groupBoxSnapOptions
            // 
            this.groupBoxSnapOptions.Controls.Add(this.numericUpDownSnapTolerance);
            this.groupBoxSnapOptions.Controls.Add(this.labelSnapTolerance);
            this.groupBoxSnapOptions.Controls.Add(this.checkBoxSnapEnabled);
            this.groupBoxSnapOptions.Location = new System.Drawing.Point(12, 182);
            this.groupBoxSnapOptions.Name = "groupBoxSnapOptions";
            this.groupBoxSnapOptions.Size = new System.Drawing.Size(358, 64);
            this.groupBoxSnapOptions.TabIndex = 6;
            this.groupBoxSnapOptions.TabStop = false;
            this.groupBoxSnapOptions.Text = "Snap Wire points";
            // 
            // numericUpDownSnapTolerance
            // 
            this.numericUpDownSnapTolerance.Location = new System.Drawing.Point(168, 25);
            this.numericUpDownSnapTolerance.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownSnapTolerance.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownSnapTolerance.Name = "numericUpDownSnapTolerance";
            this.numericUpDownSnapTolerance.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownSnapTolerance.TabIndex = 5;
            this.numericUpDownSnapTolerance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelSnapTolerance
            // 
            this.labelSnapTolerance.AutoSize = true;
            this.labelSnapTolerance.Location = new System.Drawing.Point(92, 29);
            this.labelSnapTolerance.Name = "labelSnapTolerance";
            this.labelSnapTolerance.Size = new System.Drawing.Size(55, 13);
            this.labelSnapTolerance.TabIndex = 4;
            this.labelSnapTolerance.Text = "Tolerance";
            // 
            // checkBoxSnapEnabled
            // 
            this.checkBoxSnapEnabled.AutoSize = true;
            this.checkBoxSnapEnabled.Location = new System.Drawing.Point(9, 29);
            this.checkBoxSnapEnabled.Name = "checkBoxSnapEnabled";
            this.checkBoxSnapEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxSnapEnabled.TabIndex = 0;
            this.checkBoxSnapEnabled.Text = "Enabled";
            this.checkBoxSnapEnabled.UseVisualStyleBackColor = true;
            // 
            // LCD_Settings
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(376, 418);
            this.Controls.Add(this.groupBoxSnapOptions);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxOther);
            this.Controls.Add(this.groupBoxColors);
            this.Controls.Add(this.groupBoxComponents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LCD_Settings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LCD Settings";
            this.groupBoxComponents.ResumeLayout(false);
            this.groupBoxComponents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWirePointRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDotRadius)).EndInit();
            this.groupBoxColors.ResumeLayout(false);
            this.groupBoxColors.PerformLayout();
            this.groupBoxOther.ResumeLayout(false);
            this.groupBoxOther.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarToolTipTimeout)).EndInit();
            this.groupBoxSnapOptions.ResumeLayout(false);
            this.groupBoxSnapOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSnapTolerance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxComponents;
        private System.Windows.Forms.NumericUpDown numericUpDownWirePointRadius;
        private System.Windows.Forms.Label labelWirePointRadius;
        private System.Windows.Forms.NumericUpDown numericUpDownDotRadius;
        private System.Windows.Forms.Label labelDotRadius;
        private System.Windows.Forms.GroupBox groupBoxColors;
        private System.Windows.Forms.Label labelDotOnColor;
        private System.Windows.Forms.Panel panelDotOffColor;
        private System.Windows.Forms.Label labelDotOffColor;
        private System.Windows.Forms.Panel panelDotOnColor;
        private System.Windows.Forms.Panel panelCircuitBackColor;
        private System.Windows.Forms.Label labelCircuitBackColor;
        private System.Windows.Forms.Panel panelWirePointColor;
        private System.Windows.Forms.Label labelWirePointColor;
        private System.Windows.Forms.Panel panelWireOffColor;
        private System.Windows.Forms.Label labelWireOffColor;
        private System.Windows.Forms.Panel panelWireOnColor;
        private System.Windows.Forms.Label labelWireOnColor;
        private System.Windows.Forms.GroupBox groupBoxOther;
        private System.Windows.Forms.Label labelDescriptionToolTipTimeout;
        private System.Windows.Forms.TrackBar trackBarToolTipTimeout;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button btnAssociate;
        private System.Windows.Forms.GroupBox groupBoxSnapOptions;
        private System.Windows.Forms.CheckBox checkBoxSnapEnabled;
        private System.Windows.Forms.NumericUpDown numericUpDownSnapTolerance;
        private System.Windows.Forms.Label labelSnapTolerance;
    }
}