using System;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace Client
{
    public partial class SetIPAddressForm : Form
    {
        private IPAddressControlLib.IPAddressControl ipAddressControl;

        public string ServerIP { set; get; } = "";

        public SetIPAddressForm()
        {
            InitializeComponent();
        }


        private void BtnSetServerIP_Click(object sender, EventArgs e)
        {
            ServerIP = this.ipAddressControl.Text;

            if (ServerIP.Equals("..."))
            {
                MessageBox.Show("서버 IP를 입력해주세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SetIPAddressForm_Load(object sender, EventArgs e)
        {
            int w = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2);
            int h = (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Height / 2);
            this.Location = new Point(w, h);
        }


        private bool IsValidateIP()
        {
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|[2[0-4][0-9]|25[0-5])){3}$";

            Regex check = new Regex(pattern);

            return check.IsMatch(ServerIP, 0);
        }

        private void btnSetServerIP_Click(object sender, EventArgs e)
        {
            ServerIP = this.ipAddressControl.Text;

            if (!IsValidateIP())
            {
                MessageBox.Show("잘못된 IP를 입력하셨습니다.", "잘못된 IP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
