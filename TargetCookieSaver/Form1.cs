using System;
using System.Windows.Forms;

namespace TargetCookieSaver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            TargetSiteBrowser targetSiteBrowser = new TargetSiteBrowser(checkBox1.Checked);
            targetSiteBrowser.Login(txtUserName.Text,txtpassword.Text,progressBar1);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            TargetSiteBrowser targetSiteBrowser = new TargetSiteBrowser(checkBox1.Checked);
            targetSiteBrowser.LoginWithCookie("",progressBar1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
