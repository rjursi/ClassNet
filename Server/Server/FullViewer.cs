using System;
using System.Windows.Forms;
using static Server.Viewer;

namespace Server
{
    public partial class FullViewer : Form
    {
        public static Student focusStudent;
        Timer focusingTimer;
        EventHandler focusHandler;
        public FullViewer()
        {
            InitializeComponent();
        }

        public FullViewer(Student stu)
        {
            InitializeComponent();
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

            picFocusView.Image = stu.img;
        }
    }
}
