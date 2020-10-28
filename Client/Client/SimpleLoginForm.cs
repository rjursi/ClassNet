using System;
using System.Drawing;
using System.Text.RegularExpressions;
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

        private void SimpleLoginForm_Load(object sender, EventArgs e)
        {
            TopMost = true;

            int w = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2);
            int h = (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Height / 2);
            this.Location = new Point(w, h);

            txtName.Focus();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            CheckTextbox();
        }

        private void TxtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                txtStuCode.Focus();
            }
        }

        private void TxtStuCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                CheckTextbox();
            }
        }

        private void CheckTextbox()
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("이름을 입력해주세요.", "이름 미입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
            }
            else if (String.IsNullOrWhiteSpace(txtStuCode.Text))
            {
                MessageBox.Show("학번을 입력해주세요.", "학번 미입력", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStuCode.Focus();
            }
            else
            {
                stuInfo = $"{txtStuCode.Text}({txtName.Text})";
                this.Close();
            }
        }
    }
}
