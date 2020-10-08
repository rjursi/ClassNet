using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace HookerProcess
{
    public partial class KeyMouseController : Form
    {
        public string[] parentQuitMsg;

        private static Hooker hooker;
        private static TaskMgrController taskMgrController;
        private static CtrlAltDeleteScreen ctrlAltDeleteScreenMgr;

        private Thread childProcessQuitThread;

        public KeyMouseController(string[] parentQuitMsg)
        {
            InitializeComponent();

            hooker = new Hooker();
            taskMgrController = new TaskMgrController();
            ctrlAltDeleteScreenMgr = new CtrlAltDeleteScreen();

            this.parentQuitMsg = parentQuitMsg;
        }
        
        private void KeyMouseController_Load(object sender, System.EventArgs e)
        {
            // 작업표시줄 상에서 프로그램이 표시되지 않도록 설정
            this.ShowInTaskbar = false;

            TopMost = true;

            childProcessQuitThread = new Thread(ChildProcessQuit);
            childProcessQuitThread.Start(this.parentQuitMsg);

            // 제어 기능 사용 시 아래 주석 제거
            
            taskMgrController.KillTaskMgr();
            
            hooker.SetHook();
            ctrlAltDeleteScreenMgr.StartListeningForDesktopSwitch(hooker);
            

           
        }

        private void KeyMouseController_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskMgrController.EnableTaskMgr();
            hooker.UnHook();
        }

        private void ChildProcessQuit(object parentQuitMsg)
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
                this.Close();
                Application.Exit();
            }
        }

       
    }
}