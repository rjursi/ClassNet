using System;
using System.Windows.Forms;

namespace Client
{
    public partial class TransparentForm : Form
    {
        public const int ADMINFORM = 0;
        public const int USERFORM = 1;

        public int FormStatus { set; get; }

        public TransparentForm()
        {
            InitializeComponent();
        }

        private void SetUserFormTrayIcon()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("만든이", new EventHandler((s, ea) => BtnMade_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("로그아웃", new EventHandler((s, ea) => BtnLogout_Click(s, ea))));

            this.notifyIcon.ContextMenu = ctx;
            this.notifyIcon.Visible = true;
        }

        private void SetAdminFormTrayIcon()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("설정", new EventHandler((s, ea) => BtnSetServerIP_Click(s, ea))));
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
                    MessageBox.Show("서버 IP가 수정되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        private void BtnLogout_Click(object sender, EventArgs ea)
        {
            this.Close();
            new Client().BtnLogout_Click();
        }

        private void BtnMade_Click(object sender, EventArgs ea)
        {
            MessageBox.Show("[유한대학교 IT소프트웨어공학과]\n" +
                "\n201507067 김태우\n" +
                "\n201607019 김윤권\n" +
                "\n201607058 이시헌\n" +
                "\n201807031 지한영\n" +
                "\n201507062 최준수\n" +
                "\n201807010 홍성준",
                "만든이", MessageBoxButtons.OK);
        }

        private void TransparentForm_Load(object sender, EventArgs e)
        {
            switch (FormStatus)
            {
                case ADMINFORM:
                    SetAdminFormTrayIcon();
                    break;
                case USERFORM:
                    SetUserFormTrayIcon();
                    break;
            }
        }
    }
}
