using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string SetLoginData()
        {
            string loginData;
            loginData = loginTextBox1.Text + " " + loginTextBox2.Text;
            return loginData;
        }

        private void loginButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
