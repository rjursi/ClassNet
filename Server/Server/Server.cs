using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace Server
{

    public partial class Server : Form
    {
        public class TestState
        {
            public Byte[] testbuffer;
            public Socket testSocket;
        }

        private const int MOSHPORT = 9990;
        
        Socket socketListener;
        Socket socketClient;
        IPEndPoint serverEndPoint;

        static SignalObj standardSignalObj; // 서버 표준 신호 객체

        static Byte[] imageData;

        static ImageCodecInfo codec;
        static EncoderParameters param;

        public Server()
        {
            InitializeComponent();
            standardSignalObj = new SignalObj();
        }
        
        private void BroadcastOn()
        {
            //ThreadPool.SetMinThreads(35, 35);
            //ThreadPool.SetMaxThreads(50, 50);

            // 클라이언트 연결 대기
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, MOSHPORT);

            socketListener.Bind(serverEndPoint);
            socketListener.Listen(50);

            socketListener.BeginAccept(AcceptCallback, null);


        }

        private void Server_Load(object sender, EventArgs e)
        {
            BroadcastOn();
            ThreadPool.QueueUserWorkItem(imageCreate);

            // NotifyIcon에 메뉴 추가
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("공유", new EventHandler((s, ea) => btnStart_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("제어", new EventHandler((s, ea) => btnControl_Click(s, ea))));
            ctx.MenuItems.Add("-");
            ctx.MenuItems.Add(new MenuItem("종료", new EventHandler((s, ea) => btnShutdown_Click(s, ea))));
            notifyIcon.ContextMenu = ctx;
            notifyIcon.Visible = true;

            // JPEG 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            standardSignalObj.IsServerShutdown = true;
            
            if(socketListener != null) socketListener.Close();
            Dispose();
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        void AcceptCallback(IAsyncResult ar)
        {
            // 클라이언트의 연결 요청을 수락
            if (!standardSignalObj.IsServerShutdown)
            {
                socketClient = socketListener.EndAccept(ar);
                // 클라이언트의 연결 요청을 대기(다른 클라이언트가 또 연결할 수 있으므로)
                socketListener.BeginAccept(AcceptCallback, null);

                TestState test = new TestState();
                test.testbuffer = new byte[4];
                test.testSocket = socketClient;
                
                test.testSocket.BeginReceive(test.testbuffer, 0, test.testbuffer.Length, SocketFlags.None, asyncReceiveCallback, test);
                //ThreadPool.QueueUserWorkItem(clientThread, socketClient);
            }
        }

        public static void clientThread(Object ParamSocketClient)
        {
            Byte[] recvData = new Byte[4];

            Socket socketClient = (Socket)ParamSocketClient;

            //TestState test = new TestState();
            //test.testbuffer = new byte[4];
            //test.testSocket = (Socket)ParamSocketClient;

            // 서버가 꺼지지 않은 상태라면
            while (!standardSignalObj.IsServerShutdown)
            {
                socketClient.Receive(recvData);
                //test.testSocket.BeginReceive(test.testbuffer, 0, test.testbuffer.Length, SocketFlags.None, asyncReceiveCallback, test);
                if (Encoding.UTF8.GetString(recvData).Contains("recv"))
                {
                    if (standardSignalObj.ServerBroadcastingData != null)
                    {
                        // 방송중일 때는 이미지랑 같이 넣어서 보내도록 설정
                        standardSignalObj.ServerBroadcastingData = imageData;
                        socketClient.Send(SignalObjToByte(standardSignalObj));
                    }
                    else
                    {
                        // 서버 측에서 방송중인 상태가 아닐 경우에는 그냥 서버 데이터가 담긴 데이터를 일반적으로 보냄
                        socketClient.Send(SignalObjToByte(standardSignalObj));
                    }
                    Array.Clear(recvData, 0, recvData.Length);
                }
                if (Thread.Yield()) Thread.Sleep(50);
            }
        }
        
        private static void asyncReceiveCallback(IAsyncResult ar) 
        {
            TestState ts = ar.AsyncState as TestState;

            ts.testSocket.EndReceive(ar);

            //while (!standardSignalObj.IsServerShutdown)
            if(ts.testSocket.Connected) 
            {
                if (Encoding.UTF8.GetString(ts.testbuffer).Contains("recv"))
                {

                    if (standardSignalObj.ServerBroadcastingData != null)
                    {
                        // 방송중일 때는 이미지랑 같이 넣어서 보내도록 설정
                        standardSignalObj.ServerBroadcastingData = imageData;
                        //ts.testSocket.Send(SignalObjToByte(standardSignalObj));

                        ts.testSocket.BeginSend(SignalObjToByte(standardSignalObj), 0, SignalObjToByte(standardSignalObj).Length,
                           SocketFlags.None, asyncSendCallback, ts.testSocket);
                    }
                    else
                    {
                        //ts.testSocket.Send(SignalObjToByte(standardSignalObj));

                        // 서버 측에서 방송중인 상태가 아닐 경우에는 그냥 서버 데이터가 담긴 데이터를 일반적으로 보냄
                        ts.testSocket.BeginSend(SignalObjToByte(standardSignalObj), 0, SignalObjToByte(standardSignalObj).Length,
                          SocketFlags.None, asyncSendCallback, ts.testSocket);
                    }
                    Array.Clear(ts.testbuffer, 0, ts.testbuffer.Length);
                }
                    if (Thread.Yield()) Thread.Sleep(50);
                    ts.testSocket.BeginReceive(ts.testbuffer, 0, ts.testbuffer.Length, SocketFlags.None, asyncReceiveCallback, ts);
            }
        }
       
        private static void asyncSendCallback(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            if (socket.Connected)
            {
                socket.EndSend(ar);
            }
            else
            {
                socket.Close();
            }
        }

        private static byte[] SignalObjToByte(SignalObj signalObj)
        {
            string jsonData = "";
            byte[] buffer;

            jsonData = JsonConvert.SerializeObject(signalObj);

            buffer = Encoding.Default.GetBytes(jsonData);
            
            return buffer;
        }

        public static void imageCreate(Object obj)
        {
            Byte[] preData;

            //Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap bmp = new Bitmap(1920, 1080);
            Graphics g = Graphics.FromImage(bmp);

            while (!standardSignalObj.IsServerShutdown)
            {
                try
                {
                    g.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));
                }
                catch (Win32Exception)
                {
                    // 가상 데스크톱으로 이동하여 이미지를 못 찍을 일이 생길 경우 그냥 기본 검정화면으로 덮어씌움
                    g = Graphics.FromImage(bmp);
                }

                using (MemoryStream pre_ms = new MemoryStream())
                {
                    bmp.Save(pre_ms, codec, param);
                    preData = pre_ms.ToArray();

                    using (MemoryStream post_ms = new MemoryStream())
                    {
                        using (DeflateStream ds = new DeflateStream(post_ms, CompressionMode.Compress))
                        {
                            ds.Write(preData, 0, preData.Length);
                            ds.Close();
                        }
                        imageData = post_ms.ToArray();
                        post_ms.Close();
                    }
                    pre_ms.Close();
                }
                Array.Clear(preData, 0, preData.Length);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            // 스위치 역할을 하도록 수정
            if (standardSignalObj.ServerBroadcastingData == null)
            {
                standardSignalObj.ServerBroadcastingData = imageData;

                // 폼 버튼 변경
                btnStart.Text = "공유 중지";

                // 트레이 아이콘 공유 버튼 상태 변경
                notifyIcon.ContextMenu.MenuItems[0].Checked = true;
            }
            else
            {
                standardSignalObj.ServerBroadcastingData = null;

                btnStart.Text = "공유";

                notifyIcon.ContextMenu.MenuItems[0].Checked = false;
            }
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            standardSignalObj.IsServerShutdown = true;

            if (socketListener != null) socketListener.Close();
            Dispose();
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            switch (standardSignalObj.IsServerControlling)
            {
                case false:
                {
                    standardSignalObj.IsServerControlling = true;
                    notifyIcon.ContextMenu.MenuItems[1].Checked = true;
                        
                    MessageBox.Show("키보드와 마우스 제어를 시작하였습니다.", "제어 시작", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnControl.Text = "중지";
                       
                    break;
                }
                case true:
                {
                    standardSignalObj.IsServerControlling = false;
                    notifyIcon.ContextMenu.MenuItems[1].Checked = false;
                        
                    MessageBox.Show("키보드와 마우스 제어를 중지하였습니다.", "제어 중지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnControl.Text = "제어";

                    break;
                }
            }
        }
    }
}
