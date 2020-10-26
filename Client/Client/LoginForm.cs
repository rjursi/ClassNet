using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class LoginForm : Form
    {
        private static CookieContainer cookie;
        public string stuInfo = "";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;

            this.BackColor = Color.White;

            PictureBox pic = new PictureBox
            {
                Image = Resource.yuhan,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 800,
                Height = 800
            };

            int logoX = (this.Width / 2) - (pic.Width / 2);
            int logoY = (this.Height / 2) - (pic.Height / 2);
            pic.Location = new Point(logoX, logoY);

            this.Controls.Add(pic);

            int w = this.Width - (panelInput.Width + 130);
            int h = this.Height - (panelInput.Height + 100);
            panelInput.Location = new Point(w, h);

            txtLoginID.Focus();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            CheckLogin();
        }

        private void TxtLoginID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                txtLoginPW.Focus();
            }
        }

        private void TxtLoginPW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                CheckLogin();
            }
        }

        private void BtnSimpleLogin_Click(object sender, EventArgs e)
        {
            SimpleLoginForm frm = new SimpleLoginForm();
            frm.ShowDialog();
            stuInfo = frm.stuInfo;
            this.Close();
        }

        private string AdminCheck()
        {
            if (ClassNetConfig.GetAppConfig("ADMIN_ID").Equals(txtLoginID.Text))
            {
                if (ClassNetConfig.GetAppConfig("ADMIN_PWD").Equals(txtLoginPW.Text))
                {
                    return txtLoginID.Text;
                }
            }
            return "";
        }

        private void CheckLogin()
        {
            Cursor.Current = Cursors.WaitCursor;

            stuInfo = AdminCheck();
            if (!stuInfo.Equals(""))
            {
                this.Close();
                return;
            }

            cookie = new CookieContainer();
            HttpWebResponse res = SetLogin(txtLoginID.Text, txtLoginPW.Text);

            if (GetInfo(res).Contains("fail"))
            {
                MessageBox.Show("아이디와 패스워드를 확인하세요.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLoginID.Focus();
            }
            else
            {
                stuInfo = GetInfo(res);
                this.Close();
            }
            cookie = null;
        }

        public HttpWebResponse SetLogin(string id, string password)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://portal.yuhan.ac.kr/user/loginProcess.face");
            string info = "userId=" + id + "&password=" + password;

            req.Method = "POST";
            req.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.106 Safari/535.2";
            req.CookieContainer = cookie;
            req.ContentLength = info.Length;
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            req.KeepAlive = true;

            TextWriter w = (TextWriter)new StreamWriter(req.GetRequestStream());
            w.Write(info);
            w.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            return res;
        }

        public string GetInfo(HttpWebResponse res)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://cyber.yuhan.ac.kr/User.do?cmd=ssoEnpassLogin");
            req.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.106 Safari/535.2";
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            req.CookieContainer = cookie;
            req.CookieContainer.Add(res.Cookies);

            res = (HttpWebResponse)req.GetResponse();
            TextReader r = (TextReader)new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

            string full = r.ReadToEnd();
            int start_idx = full.IndexOf("<p class=\"mt5\"><span>");

            if (start_idx > 0)
            {
                int last_idx = full.IndexOf("님</span></p>");
                string result = full.Substring(start_idx, last_idx - start_idx);

                return result.Substring(21);
            }
            else
            {
                return "fail";
            }
        }
    }
}
