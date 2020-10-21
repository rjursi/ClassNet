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

    

        private void btnSetServerIP_Click(object sender, EventArgs e)
        {
            ServerIP = this.ipAddressControl.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
