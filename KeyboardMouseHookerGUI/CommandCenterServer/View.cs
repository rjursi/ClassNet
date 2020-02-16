using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandCenterServer
{
    public partial class View : Form
    {

        ServerCommandSender commander;

        public View()
        {
            InitializeComponent();
            commander = new ServerCommandSender();    
        }

        private void btn_control_Click(object sender, EventArgs e)
        {
            switch (ServerCommandSender.returnedMsg)
            {
                case "control stopped":

                    commander.Send("control start");
                    lbl_status.Text = "Controlling...";
                    btn_control.Text = "키보드 마우스 제어 중입니다...";
                    break;

                case "controlling":
                    commander.Send("control stop");
                    lbl_status.Text = "Not Controlling";
                    btn_control.Text = "클라이언트 컨트롤 시작";
                    break;
            }
                
        }
    }
}
