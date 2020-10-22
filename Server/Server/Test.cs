using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Server.Viewer;

namespace Server
{
    public partial class Test : Form
    {
        public static Student focusStudent;
        Timer focusingTimer;
        EventHandler focusHandler;
        public Test()
        {
            InitializeComponent();
        }

        public Test(Student stu)
        {
            InitializeComponent();
            focusStudent = stu;
            focusingTimer = new Timer();
            focusingTimer.Interval = 500;
            focusHandler = (sender, e) => InterateFocusing(stu);
            focusingTimer.Tick += focusHandler;
            focusingTimer.Start();
        }

        private void Test_Load(object sender, EventArgs e)
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
