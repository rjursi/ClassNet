using System;
using System.Windows.Forms;
using static Server.Viewer;

namespace Server
{
    public partial class FullViewer : Form
    {
        public static Student focusStudent;
        readonly Timer focusingTimer;
        readonly EventHandler focusHandler;
        private static Boolean isStop;

        DialogResult result;

        public FullViewer()
        {
            InitializeComponent();
        }

        public FullViewer(Student stu)
        {
            InitializeComponent();
            isStop = true;
            focusStudent = stu;
            focusingTimer = new Timer();
            focusingTimer.Interval = 500;
            focusHandler = (sender, e) => InterateFocusing(stu);
            focusingTimer.Tick += focusHandler;
            focusingTimer.Start();
        }

        private void FullViewer_Load(object sender, EventArgs e)
        {
            picFocusView.Image = focusStudent.img;
            Text = focusStudent.info;
        }

        void InterateFocusing(Student stu)
        {
            if (clientsList.ContainsValue(stu))
            {

                picFocusView.Image = stu.img;
                
            }
            else
            {
                if (isStop)
                {
                    isStop = false;
                    result = MessageBox.Show("해당 학습자가 연결을 종료하였습니다.", "에러!", MessageBoxButtons.OK, MessageBoxIcon.Error); //연결 끊긴 이후 후 처리 요망

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        focusingTimer.Stop();
                        this.Close();
                    }

                    
                }
            }
        }
    }
}
