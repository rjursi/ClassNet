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
    public partial class TransparentForm : Form
    {
        public const int ADMINFORM = 0;
        public const int USERFORM = 1;

        private int formStatus;
        public int FormStatus {
            set
            {
                formStatus = value;
            }
            get
            {
                return formStatus;
            }
        }

        public TransparentForm()
        {
            InitializeComponent();
        }
        private void setUserFormTrayIcon()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("로그아웃", new EventHandler((s, ea) => BtnLogout_Click(s, ea))));

            this.notifyIcon.ContextMenu = ctx;
            this.notifyIcon.Visible = true;
        }

        private void setAdminFormTrayIcon()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("서버 IP 설정", new EventHandler((s, ea) => BtnSetServerIP_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("로그아웃", new EventHandler((s, ea) => BtnLogout_Click(s, ea))));

            this.notifyIcon.ContextMenu = ctx;
            this.notifyIcon.Visible = true;
        }

        private void BtnSetServerIP_Click(object sender, EventArgs ea)
        {
            using (SetIPAddressForm setIPAddressForm = new SetIPAddressForm())
            {
                var dialogResult = setIPAddressForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    ClassNetConfig.SetAppConfig("SERVER_IP", setIPAddressForm.ServerIP);

                    MessageBox.Show("서버 IP 가 수정이 되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        private void BtnLogout_Click(object sender, EventArgs ea)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void TransparentForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            switch (FormStatus)
            {
                case ADMINFORM:
                    
                    setAdminFormTrayIcon();
                    break;
                case USERFORM:
                    setUserFormTrayIcon();
                    break;
            }
        }
    }
}
