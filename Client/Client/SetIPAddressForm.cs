using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SetIPAddressForm_Load(object sender, EventArgs e)
        {
            int w = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2);
            int h = (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Height / 2);
            this.Location = new Point(w, h);
        }
    }
}
