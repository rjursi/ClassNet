using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private string serverIP = "";

        
        public string ServerIP {
            set
            {
                this.serverIP = value;
            }
            get
            {
                return serverIP;
            }
        }
        
   
        
        public SetIPAddressForm()
        {
            InitializeComponent();
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
