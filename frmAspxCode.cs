using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormGenerator
{
    public partial class frmAspxCode : Form
    {
        public frmAspxCode()
        {
            InitializeComponent();
            
        }

        private String htmlCode;
        public String HtmlCode
        {
            get { return htmlCode; }
            set
            {
                htmlCode = value;
                txtHtml.Text = value;
            }
        }

        private String codeBehind;
        public String CodeBehind
        {
            get { return codeBehind; }
            set
            {
                codeBehind = value;
                txtCodeBehind.Text = value;
            }
        }


        private void txtHtml_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                txtHtml.SelectAll();
            }
        }

        private void txtGetSet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                txtCodeBehind.SelectAll();
            }
        }

        private void btnFinished_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmAspxCode_Load(object sender, EventArgs e)
        {

        }

    }
}
