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
        public bool clientShutdownFlag;

        private const int MOSHPORT = 9990;
        private const string SERVER_IP = "127.0.0.1"; 

        Socket socketServer;
        SignalObj standardSignalObj;
        CmdProcssController cmdProcessController;

        delegate void ThreadDelegate(int imgSize, Byte[] recvData);
        delegate void clientShutdownDelegate();

        public Client()
        {
            InitializeComponent();
            clientShutdownFlag = false;

            cmdProcessController = new CmdProcssController();
            ThreadPool.SetMaxThreads(3, 3); 
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
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Invoke(new clientShutdownDelegate(clientShutdown));
        }

        public void receiveThread(Object ob)
        {
            Byte[] sendData = new Byte[4];
            Byte[] recvData = new Byte[4];
            Byte[] lenData = new Byte[4];
            Byte[] imgData = new Byte[4];

            int imgSize = 0;
            int recvDataSize = 0;

            while (!clientShutdownFlag)
            {
                try
                {
                    sendData = Encoding.UTF8.GetBytes("size");
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
                    // 서버측에서 전송한 객체를 byte로 받고 객체로 변환
                    socketServer.Receive(lenData);

                    recvDataSize = BitConverter.ToInt32(lenData, 0);
                    Array.Resize<Byte>(ref recvData, recvDataSize);

                    try
                    {
                        sendData = Encoding.UTF8.GetBytes("recv");
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
                    standardSignalObj = ByteToObject(recvData);

                    // 서버가 현재 방송중인 상태이면
                    if (standardSignalObj.IsServerBroadcasting)
                    {
                        // 이미지를 받아서 여기서 버퍼를 설정하는 부분
                        imgData = standardSignalObj.ImgData;

                        imgSize = imgData.Length;
                        Array.Resize<Byte>(ref imgData, imgSize);

                        this.Invoke(new ThreadDelegate(outputDelegate), imgSize, imgData);

                        imgSize = 0;
                    }

                    // 서버가 제어 신호가 걸린 상태이면
                    cmdProcessController.CtrlStatusEventCheck(standardSignalObj.IsServerControlling);
                }
                catch (SocketException) { }
                catch (ObjectDisposedException) { }
                catch (JsonReaderException)
                {
                    Array.Clear(sendData, 0, sendData.Length);
                    Array.Clear(recvData, 0, recvData.Length);
                    Array.Clear(lenData, 0, lenData.Length);
                    continue;
                }

                Array.Clear(sendData, 0, sendData.Length);
                Array.Clear(imgData, 0, imgData.Length);
                Array.Clear(recvData, 0, recvData.Length);
                Array.Clear(lenData, 0, lenData.Length);

                //Thread.Yield();
                if (Thread.Yield()) Thread.Sleep(50);
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
            if (cmdProcessController.NowCtrlStatus)
            {
                cmdProcessController.QuitProcess();
            }
           
            socketServer.Close();
            
            this.Invoke(new MethodInvoker(() => { Dispose(); }));
        }

    }
}
