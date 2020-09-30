using System;
using System.Windows.Forms;

namespace Client
{
    public partial class SimpleLoginForm : Form
    {
        public string stuInfo = "";

        public SimpleLoginForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            stuInfo = $"{txtStuCode.Text}({txtName.Text})";
            this.Close();
        }
    }
}
