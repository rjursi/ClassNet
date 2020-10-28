using System;
using System.Drawing;
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
    }
}
