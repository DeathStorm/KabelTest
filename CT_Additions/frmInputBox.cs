using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CT_Additions
{
    public partial class frmInputBox : Form
    {
        public frmInputBox(String title, String message, String preDefined)
        {
            
            InitializeComponent(); 
            this.tbMessage.Text = message;
            this.Text = title;
            this.tbInput.Text = preDefined;
            
        }

        private void frmInputBox_Load(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public static string NewInput(String title, String message) { return NewInput(title, message, ""); }
        public static string NewInput(String title, String message, String preDefined)
        {
            frmInputBox input = new frmInputBox(title,message,preDefined);
            input.ShowDialog();
            if (input.DialogResult == DialogResult.OK)
            { return input.tbInput.Text; }
            else
            { return ""; }
    }

        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
