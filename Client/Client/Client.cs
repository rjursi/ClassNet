using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Client
{
    public partial class Client : Form
    {
        public bool isConnected = true;
        public bool clientShutdownFlag;

        private const int MOSHPORT = 9990;
        private const string SERVER_IP = "127.0.0.1"; 

        Socket socketServer;
        ClientListener clientCommandListener;

        delegate void ThreadDelegate(int imgSize, Byte[] recvData);
        delegate void clientShutdownDelegate();

        public Client()
        {
            InitializeComponent();
            clientShutdownFlag = false;

            clientCommandListener = new ClientListener();
            ThreadPool.SetMaxThreads(3, 3);
        }

        private void Client_Load(object sender, EventArgs e)
        {
            while (isConnected)
            {
                try
                {
                    socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(SERVER_IP), MOSHPORT); // 192.168.31.218 // 192.168.31.200
                    socketServer.Connect(serverEndPoint);

                    // 작업표시줄 상에서 프로그램이 표시되지 않도록 설정
                    this.ShowInTaskbar = false;

                    /*받은 이미지 풀스크린으로 띄우는 소스입니다. 수정 ㄴㄴ*/
                    /*FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    TopMost = true;

                    pictureBox1.Width = Screen.PrimaryScreen.Bounds.Width;
                    pictureBox1.Height = Screen.PrimaryScreen.Bounds.Height;*/

                    isConnected = false;
                }
                catch (SocketException)
                {
                    isConnected = true; // 해당 bool 변수로 인해서 다시한번 위 반복문이 실행
                }
            }
            ThreadPool.QueueUserWorkItem(receiveThread);
            ThreadPool.QueueUserWorkItem(runClientListenerThread);
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Invoke(new clientShutdownDelegate(clientShutdown));
        }

        private void runClientListenerThread(Object ob)
        {
            clientCommandListener.Start();
        }

        public void receiveThread(Object ob)
        {
            Byte[] sendData, recvData = new Byte[4];
            int imgSize = 0;

            while (!clientShutdownFlag)
            {
                sendData = Encoding.UTF8.GetBytes("next");

                try
                {
                    socketServer.Send(sendData);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (SocketException)
                {
                    clientShutdown();
                    break;
                }

                try
                {
                    socketServer.Receive(recvData);
                    imgSize = BitConverter.ToInt32(recvData, 0);
                    Array.Resize<Byte>(ref recvData, imgSize);

                    try
                    {
                        socketServer.Send(sendData);
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    catch (SocketException)
                    {
                        clientShutdown();
                        break;
                    }
                    socketServer.Receive(recvData);

                    this.Invoke(new ThreadDelegate(outputDelegate), imgSize, recvData);

                    Array.Clear(recvData, 0, recvData.Length);
                    Array.Clear(sendData, 0, sendData.Length);

                    imgSize = 0;
                }
                catch (SocketException) { }
                catch (ObjectDisposedException) { }

                //Thread.Yield();
                if (Thread.Yield()) Thread.Sleep(40);
            }
        }

        public void outputDelegate(int imgSize, Byte[] recvData)
        {
            Image img;

            using (MemoryStream pre_ms = new MemoryStream(recvData))
            {
                using (MemoryStream post_ms = new MemoryStream())
                {
                    using (DeflateStream ds = new DeflateStream(pre_ms, CompressionMode.Decompress))
                    {
                        try
                        {
                            ds.CopyTo(post_ms);
                        }
                        catch { }
                        ds.Close();
                    }
                    img = Image.FromStream(post_ms);
                    post_ms.Close();
                }
                pre_ms.Close();
            }

            label1.Text = imgSize.ToString();
            pictureBox1.Image = img;
        }

        public void clientShutdown()
        {
            clientShutdownFlag = true;
            clientCommandListener.setClientShutdownFlagToCtrlPart(clientShutdownFlag);

            if (clientCommandListener.ctrlStatus)
            {
                clientCommandListener.QuitProcess();
            }

            socketServer.Close();
            clientCommandListener.CloseControlUdpSocket();

            this.Invoke(new MethodInvoker(() => { Dispose(); }));
        }

    }
}
