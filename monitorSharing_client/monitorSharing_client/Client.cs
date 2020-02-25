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
    public partial class ClientForm : Form
    {
        Socket socketServer;
        Thread clientReceiveThread;
        bool clientShutdownFlag;
        Byte[] sendData;
        int imgSize = 0;

        //Byte[] imgSizeBuffer = new Byte[8];
        Byte[] imgRecv = new byte[100000];

        delegate void ThreadDelegate(int imgSize, Image img);
        delegate void clientShutdownDelegate();
        public ClientForm()
        {
            InitializeComponent();
            clientShutdownFlag = false;
        }

     

        private void btn_connect_Click(object sender, EventArgs e)
        {

            try
            {
                socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("Private IP Input Here"), 7979); // 192.168.31.218
                socketServer.Connect(serverEndPoint);

                clientReceiveThread = new Thread(() => receiveThread());
                clientReceiveThread.Start();
            }catch(SocketException)
            {
                MessageBox.Show("서버가 아직 동작중이지 않습니다.", "서버 확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void clientForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            this.Invoke(new clientShutdownDelegate(clientShutdown));
            
        }

        public void receiveThread()
        {
            while (!clientShutdownFlag)
            {
               
                sendData = Encoding.UTF8.GetBytes("ready");
                try
                {
                    socketServer.Send(sendData);
                }
                catch (SocketException)
                {
                    MessageBox.Show("서버 종료로 인해 클라이언트를 종료합니다.");
                    clientShutdown();
                }
                

                /*socketServer.Receive(imgSizeBuffer);
                imgSize = BitConverter.ToInt32(imgSizeBuffer, 0);*/
                try
                {
                    socketServer.Receive(imgRecv);

                    if (Encoding.UTF8.GetString(imgRecv).Equals("server finished"))
                    {
                        this.Invoke(new clientShutdownDelegate(clientShutdown));
                        break;
                    }
                }
                catch (SocketException)
                {

                }
                

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


        public void clientShutdown()
        {
            clientShutdownFlag = true;

            socketServer.Close();
            Dispose();
        }
     
    }
}
