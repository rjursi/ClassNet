using System;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    public partial class FullViewer : Form
    {
        public FullViewer()
        {
            InitializeComponent();
        }

        public void AccessControl(Image img, String txt)
        {
            this.picFocusView.Image = img;
            this.Text = txt;
        }

        private void FullViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.picFocusView.Image = null;
            this.Text = "";
        }
    }
}
