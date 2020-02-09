using System.Windows.Forms;

namespace KeyboardMouseHookerGUI
{
    public partial class form_keyMouseCtrl : Form
    {

        Hooker hooker;
        

        public form_keyMouseCtrl()
        {
            InitializeComponent();
        }

        private void btn_ctrl_Click(object sender, System.EventArgs e)
        {
            if (!hooker.CtrlFlag)
            {
                this.hooker.SetHook();
                btn_ctrl.Text = "키보드 마우스가 제어 중입니다.";

                hooker.CtrlFlag = true;
            }

            else
            {
                this.hooker.UnHook();
                lbl_status.Text = "키보드 마우스 제어가 중지되었습니다.";
                btn_ctrl.Text = "제어 시작";
                hooker.CtrlFlag = false;
            }
            
            
            
        }

        private void form_keyMouseCtrl_Load(object sender, System.EventArgs e)
        {
            hooker = new Hooker();
        }
            
    }
}
