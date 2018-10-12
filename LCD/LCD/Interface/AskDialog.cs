using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LCD.Interface
{
    public partial class AskDialog : Form
    {


        public AskDialog()
        {
            InitializeComponent();
        }

        public AskDialog(String questionText):this()
        {
            QuestionText = questionText;
        }

        public AskDialog(String questionText,String caption)
            : this(questionText)
        {
            Caption = caption;
        }

        public AskDialog(String questionText, String caption,bool askMeAgain)
            : this(questionText)
        {
            AskMeAgain = askMeAgain;
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;

            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;

            this.Close();
        }

        public String QuestionText
        {
            get
            {
                return labelQuestion.Text;
            }
            set
            {
                labelQuestion.Text = value;
            }
        }

        public String Caption
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public bool AskMeAgain
        {
            get
            {
                return checkBoxAskMeAgain.Checked;
            }
            set
            {
                checkBoxAskMeAgain.Checked = value;
            }
        }
    }
}
