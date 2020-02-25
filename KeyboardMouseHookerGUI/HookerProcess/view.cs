using System.Windows.Forms;

namespace HookerProcess
{
    public partial class form_keyMouseControlling : Form
    {
        Hooker hooker;
        TaskMgrController taskMgrController;
        CtrlAltDeleteScreen ctrlAltDeleteScreenMgr;
        bool isHookerDoing;

        public form_keyMouseControlling()
        {
            InitializeComponent();

            hooker = new Hooker();
            taskMgrController = new TaskMgrController();
            ctrlAltDeleteScreenMgr = new CtrlAltDeleteScreen();
            isHookerDoing = false;


            
        }
        
        private void form_keyMouseControlling_Load(object sender, System.EventArgs e)
        {
            taskMgrController.KillTaskMgr();
            //hooker.SetHook();
            ctrlAltDeleteScreenMgr.StartListeningForDesktopSwitch(hooker);
            isHookerDoing = true;

        }

        
        private void form_keyMouseControlling_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskMgrController.EnableTaskMgr();
            //hooker.UnHook();

            isHookerDoing = false;
        }
    }
}
