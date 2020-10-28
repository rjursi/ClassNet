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
                    result = MessageBox.Show("해당 학습자와 연결이 끊겼습니다.", "전체 화면 오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error); // 연결 끊긴 이후 후 처리 요망

                    if (result == DialogResult.OK)
                    {
                        focusingTimer.Stop();
                        this.Close();
                    }
                }
            }
        }

        private void FullViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            focusingTimer.Stop();
            focusingTimer.Dispose();
        }
    }
}
