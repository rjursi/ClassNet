using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace monitorSharing_client
{
    public partial class Form1 : Form
    {
        Socket socketServer;

        Byte[] sendData;
        int imgSize = 0;

        //Byte[] imgSizeBuffer = new Byte[8];
        Byte[] imgRecv = new byte[100000];

        delegate void ThreadDelegate(int imgSize, Image img);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.31.200"), 7979); // 192.168.31.218
            socketServer.Connect(serverEndPoint);

            new Thread(() => recieveThread(socketServer)).Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            socketServer.Close();
            Dispose();
        }

        public void recieveThread(Socket socketServer)
        {
            while (true)
            {
                sendData = Encoding.UTF8.GetBytes("ready");
                socketServer.Send(sendData);

                /*socketServer.Receive(imgSizeBuffer);
                imgSize = BitConverter.ToInt32(imgSizeBuffer, 0);*/

                socketServer.Receive(imgRecv);

                try
                {
                    MemoryStream ms = new MemoryStream(imgRecv);
                    Image img = Image.FromStream(ms);

                    this.Invoke(new ThreadDelegate(outputDelegate), imgSize, img);

                    ms.Close();
                }
                catch
                {

                }
                                
                Array.Clear(imgRecv, 0, imgRecv.Length);
                //Array.Clear(imgSizeBuffer, 0, imgSizeBuffer.Length);

                Array.Clear(sendData, 0, sendData.Length);
            }
        }

        public void outputDelegate(int imgSize, Image img)
        {
            //label1.Text = imgSize.ToString();
            pictureBox1.Image = img;
        }
    }
}
