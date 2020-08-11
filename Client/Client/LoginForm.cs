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

        public string SetLoginData()    // 로그인 데이터를 반환하는 메소드
        {
            string loginData;
            loginData = loginTextBoxID.Text + " " + loginTextBoxName.Text;
            return loginData;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            this.Close(); // ShowDialog를 종료
        }
    }
}
