using System;
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

        private void LoginButton_Click(object sender, EventArgs e)
        {
            cookie = new CookieContainer();
            HttpWebResponse res = SetLogin(txtLoginID.Text, txtLoginPW.Text);
            //string result = SetLogin(txtLoginID.Text, txtLoginPW.Text);

            if (GetInfo(res).Contains("fail") /*result.Contains("fail")*/)
            {
                MessageBox.Show("아이디와 패스워드를 확인하세요.");
            }
            else
            {
                stuInfo = GetInfo(res);
                //stuInfo = result;
                this.Close();
            }

            cookie = null;
        }

        public HttpWebResponse /*string*/ SetLogin(string id, string password)
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

            /*TextReader r = (TextReader)new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

            string full = r.ReadToEnd();
            int start_idx = full.IndexOf("<span class=\"name\"><strong>");

            if (start_idx > 0)
            {
                int last_idx = full.IndexOf("</strong>님</span>");
                string result = full.Substring(start_idx, last_idx - start_idx);
                return result.Substring(27);
            }
            else
            {
                return "fail";
            }*/

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

        private void btnSimpleLogin_Click(object sender, EventArgs e)
        {
            SimpleLoginForm frm = new SimpleLoginForm();
            frm.ShowDialog();
            stuInfo = frm.stuInfo;
            this.Close();
        }
    }
}
