using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace HookerProcess
{
    public partial class form_keyMouseControlling : Form
    {
        Hooker hooker;
        TaskMgrController taskMgrController;
        CtrlAltDeleteScreen ctrlAltDeleteScreenMgr;
        bool isHookerDoing;
        string[] parentQuitMsg;

        Thread childProcessQuitThread;

        public form_keyMouseControlling(string[] parentQuitMsg)
        {
            InitializeComponent();

            hooker = new Hooker();
            taskMgrController = new TaskMgrController();
            ctrlAltDeleteScreenMgr = new CtrlAltDeleteScreen();
            isHookerDoing = false;

            this.parentQuitMsg = parentQuitMsg;
        }
        
        private void form_keyMouseControlling_Load(object sender, System.EventArgs e)
        {
            // 작업표시줄 상에서 프로그램이 표시되지 않도록 설정
            this.ShowInTaskbar = false;

            int x = Screen.PrimaryScreen.Bounds.Width / 2 -  this.Size.Width / 2;
            int y = Screen.PrimaryScreen.Bounds.Height - (this.Size.Height  + 20);
            this.Location = new Point(x, y);

            childProcessQuitThread = new Thread(childProcessQuit);
            childProcessQuitThread.Start(this.parentQuitMsg);

            /*taskMgrController.KillTaskMgr();
            hooker.SetHook();
            ctrlAltDeleteScreenMgr.StartListeningForDesktopSwitch(hooker);*/

            isHookerDoing = true;
        }

        private void form_keyMouseControlling_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*taskMgrController.EnableTaskMgr();
            hooker.UnHook();*/

            isHookerDoing = false;
        }

        private void childProcessQuit(object parentQuitMsg)
        {
            string[] quitMsg = (string[])parentQuitMsg;

            if(quitMsg.Length > 0)
            {
                using(PipeStream pipeClient = new AnonymousPipeClientStream(PipeDirection.In, quitMsg[0]))
                {
                    using(StreamReader streamReader = new StreamReader(pipeClient))
                    {
                        string msg;

                        do
                        {
                            msg = streamReader.ReadLine();
                        } 
                        while (!msg.StartsWith("quit"));
                    }
                }

                Application.Exit();
            }
        }
    }
}
