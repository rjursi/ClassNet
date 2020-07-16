using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Client
{
    public partial class Client : Form
    {
        public bool isConnected = false;

        static SignalObj standardSignalObj;

        private const int MOSHPORT = 9990;
        private const string SERVER_IP = "127.0.0.1";

        Socket socketServer;

        CmdProcssController cmdProcessController;

        delegate void ScreenOnDelegate(int imgSize, Byte[] recvData, double isOpacity);
        delegate void ScreenOffDelegate(double isOpacity);

        public Client()
        {
            InitializeComponent();
        }

        public SignalObj ByteToObject(byte[] buffer)
        {
            string jsonData = "";
            SignalObj sObj;

            jsonData = Encoding.Default.GetString(buffer);
            
            sObj = JsonConvert.DeserializeObject<SignalObj>(jsonData);

            return sObj;
        }

        private void Client_Load(object sender, EventArgs e)
        {
            ThreadPool.SetMaxThreads(5, 5);

            cmdProcessController = new CmdProcssController();

            while (!isConnected)
            {
                try
                {
                    socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(SERVER_IP), MOSHPORT);
                    socketServer.Connect(serverEndPoint);

                    // 작업표시줄 상에서 프로그램이 표시되지 않도록 설정
                    // 개인 테스트 과정에서 불편하므로 커밋할 때는 주석처리 해주세요.
                    // this.ShowInTaskbar = false;

                    // 받은 이미지를 풀스크린으로 띄우는 설정
                    // 개인 테스트 과정에서 불편하므로 커밋할 때는 주석처리 해주세요.
                    /*FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    pictureBox1.Width = Screen.PrimaryScreen.Bounds.Width;
                    pictureBox1.Height = Screen.PrimaryScreen.Bounds.Height;
                    */



                    // 화면 폼을 가장 맨 위로
                    TopMost = true;

                    isConnected = true;
                }
                catch (SocketException)
                {
                    isConnected = false; // 해당 bool 변수로 인해서 다시한번 위 반복문이 실행
                }
            }

            Opacity = 0;
            ThreadPool.QueueUserWorkItem(receiveThread);
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cmdProcessController.NowCtrlStatus) cmdProcessController.QuitProcess();
            socketServer.Close();

            this.Invoke(new MethodInvoker(() => { Dispose(); }));
        }

        public void receiveThread(Object obj)
        {
            Byte[] recvData = new Byte[327675]; // 327,675 Byte = 65,535 Byte * 5
            Byte[] sendData = Encoding.UTF8.GetBytes("recv");

            while (true)
            {
                try
                {
                    socketServer.Send(sendData);

                    socketServer.Receive(recvData);

                    using(standardSignalObj = ByteToObject(recvData))
                    {
                        // 서버가 현재 방송중인 상태이면
                        if (standardSignalObj.ServerScreenData != null)
                        {
                            // 이미지를 받아서 여기서 버퍼를 설정하는 부분
                            this.Invoke(new ScreenOnDelegate(outputDelegate),
                                standardSignalObj.ServerScreenData.Length, standardSignalObj.ServerScreenData, 1);
                        }
                        else
                        {
                            this.Invoke(new ScreenOffDelegate(opacityDelegate), 0);
                        }

                        // 서버가 제어 신호가 걸린 상태이면
                        cmdProcessController.CtrlStatusEventCheck(standardSignalObj.IsServerControlling);
                    }
                }
                catch (SocketException)
                {
                    if (cmdProcessController.NowCtrlStatus) cmdProcessController.QuitProcess();
                    socketServer.Close();

                    this.Invoke(new MethodInvoker(() => { Dispose(); }));
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (JsonReaderException)
                {
                    Array.Clear(recvData, 0, recvData.Length);
                    continue;
                }
                Array.Clear(recvData, 0, recvData.Length);

                Thread.Yield();
            }
        }

        public void opacityDelegate(double isOpacity)
        {
            Opacity = isOpacity;
        }

        public void outputDelegate(int imgSize, Byte[] recvData, double isOpacity)
        {
            Opacity = isOpacity;

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
                        finally
                        {
                            ds.Close();
                        }
                    }
                    imageSize.Text = imgSize.ToString();
                    screenImage.Image = Image.FromStream(post_ms);

                    post_ms.Close();
                }
                pre_ms.Close();
            }
        }
    }
}
