using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace monitorSharing_client
{
    public partial class Form1 : Form
    {
        //int fileNum = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < 10; i++)
            {
                string fileName = "ftp://192.168.31.200:9999/ScreenCapture";
                string fileType = ".jpeg";

                pictureBox1.LoadAsync(@fileName + i + fileType);
            }
            */

            pictureBox1.LoadAsync(@"ftp://192.168.31.200:9999/ScreenCapture.jpeg");

            /*
            string fileName = "ftp://192.168.31.200:9999/ScreenCapture";
            string fileType = ".jpeg";

            pictureBox1.LoadAsync(@fileName + fileNum + fileType);

            fileNum++;
            */
        }
    }
}
