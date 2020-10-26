using System;
using System.Drawing;
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

        private void SimpleLoginForm_Load(object sender, EventArgs e)
        {
            TopMost = true;

            int w = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2);
            int h = (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Height / 2);
            this.Location = new Point(w, h);
        }
    }
}
