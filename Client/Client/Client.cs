using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    public partial class Client : Form
    {
        public bool isConnected = true;

        private const int MOSHPORT = 9990;
        private const string SERVER_IP = "127.0.0.1"; 

        Socket socketServer;
        SignalObj standardSignalObj;
        CmdProcssController cmdProcessController;

        delegate void ThreadDelegate(int imgSize, Byte[] recvData);

        public Client()
        {
            InitializeComponent();

            cmdProcessController = new CmdProcssController();
            ThreadPool.SetMaxThreads(5, 5); 
        }

        public SignalObj ByteToObject(byte[] buffer)
        {
            //JObject jObj;
            string jsonData = "";
            SignalObj sObj;

            jsonData = Encoding.Default.GetString(buffer);
            
            sObj = JsonConvert.DeserializeObject<SignalObj>(jsonData);

            return sObj;
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

                    // 작업표시줄 상에서 프로그램이 표시되지 않도록 설정 → 프로젝트 진행과정에서는 작업표시줄에 표시할게! - 태우
                    // this.ShowInTaskbar = false;

                    /* 받은 이미지 풀스크린으로 띄우는 소스입니다. 수정 ㄴㄴ - 태우 */
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
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cmdProcessController.NowCtrlStatus) cmdProcessController.QuitProcess();
            socketServer.Close();

            this.Invoke(new MethodInvoker(() => { Dispose(); }));
        }

        public void receiveThread(Object obj)
        {
            Opacity = 0;

            Byte[] recvData = new Byte[327675]; // 327,675 Byte = 65,535 Byte * 5
            Byte[] sendData = Encoding.UTF8.GetBytes("recv");

            while (true)
            {
                try
                {
                    socketServer.Send(sendData);

                    socketServer.Receive(recvData);
                    standardSignalObj = ByteToObject(recvData);

                    // 서버가 현재 방송중인 상태이면
                    if (standardSignalObj.ServerBroadcastingData != null)
                    {
                        Opacity = 1;

                        // 이미지를 받아서 여기서 버퍼를 설정하는 부분
                        this.Invoke(new ThreadDelegate(outputDelegate),
                            standardSignalObj.ServerBroadcastingData.Length, standardSignalObj.ServerBroadcastingData);
                    }
                    else
                    {
                        Opacity = 0;
                    }

                    // 서버가 제어 신호가 걸린 상태이면
                    cmdProcessController.CtrlStatusEventCheck(standardSignalObj.IsServerControlling);
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

        public void outputDelegate(int imgSize, Byte[] recvData)
        {
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

                    label1.Text = imgSize.ToString();
                    pictureBox1.Image = Image.FromStream(post_ms);

                    post_ms.Close();
                }
                pre_ms.Close();
            }
        }
    }
}
