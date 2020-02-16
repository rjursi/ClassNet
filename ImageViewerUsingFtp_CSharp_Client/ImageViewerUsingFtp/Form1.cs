using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;



namespace ImageViewerUsingFtp
{
    public partial class Form1 : Form
    {

        Timer timer = new Timer();
   

        public Form1()
        {
            InitializeComponent();

            timer.Enabled = true;
            //일단은 학교에서 50으로 했을때 시간이 잘 맞는듯함
            timer.Interval = 50;

            timer.Tick += timer_Tick;

            timer.Start();

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        
        private void timer_Tick(Object sender, EventArgs e)
        {
            pictureBox1.LoadAsync(@"ftp://10.107.0.173/ScreenCapture.jpeg");
            //pictureBox1.LoadAsync(@"ftp://192.168.0.15/ScreenCapture.jpeg");
           



        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //폼의 크기를 조절할 수 없게끔
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormBorderStyle = FormBorderStyle.None;
            //폼의 크기를 컴퓨터 해상도에 맞게끔
            this.WindowState = FormWindowState.Maximized;
            //작업표시줄에 안뜨게 함
            //this.ShowInTaskbar = false;

            //이미지를 픽쳐박스 크기에 맞게 조정
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            ////픽쳐박스의 크기를 조정
            pictureBox1.Size = new Size(this.Width, this.Height);

            //컨트롤 박스 안뜨게
            this.ControlBox = false;


        }
    }
}
