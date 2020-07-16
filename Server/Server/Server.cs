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
using Newtonsoft.Json;
using System.ComponentModel;
using System.Collections.Generic;

namespace Server
{
    public partial class Server : Form
    {
        static SignalObj standardSignalObj; // 서버 표준 신호 객체

        private const int MOSHPORT = 9990;

        public class ClientObject
        {
            public Byte[] recvBuffer;
            public Socket socketClient;
        }

        static int clientCount = 0; //접속 클라이언트 수
        static public Dictionary<Socket, string> connectedClientList = new Dictionary<Socket, string>(); //클라이언트 리스트
       
        Socket socketListener;
        Socket socketObject;
        IPEndPoint serverEndPoint;

        static Byte[] imageData;

        static ImageCodecInfo codec;
        static EncoderParameters param;

        public Server()
        {
            InitializeComponent();
        }

        //동적으로 PictureBox 생성하는 로직:
        //Form_Load를 하고 
        public void ClientBoxes_Load(object sender,EventArgs e)
        {
            DynamicClientBoxesCreate();
        }

        public void DynamicClientBoxesCreate()
        {

        }



        private void SocketOn()
        {
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, MOSHPORT);

            socketListener.Bind(serverEndPoint);
            socketListener.Listen(50);

            socketListener.BeginAccept(AcceptCallback, null);
        }

        private void NotifyIconSetting()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("공유", new EventHandler((s, ea) => btnStart_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("제어", new EventHandler((s, ea) => btnControl_Click(s, ea))));
            ctx.MenuItems.Add("-");
            ctx.MenuItems.Add(new MenuItem("종료", new EventHandler((s, ea) => btnShutdown_Click(s, ea))));
            notifyIcon.ContextMenu = ctx;
            notifyIcon.Visible = true;
        }

        private void Server_Load(object sender, EventArgs e)
        {
            ThreadPool.SetMaxThreads(50, 50);

            standardSignalObj = new SignalObj();

            // 클라이언트 연결 대기
            SocketOn();

            // NotifyIcon에 메뉴 추가
            NotifyIconSetting();

            // 화면 이미지 객체 생성
            ThreadPool.QueueUserWorkItem(imageCreate);

            // JPEG 손실 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            standardSignalObj.IsServerShutdown = true;
            standardSignalObj = null;

            if (socketListener != null) socketListener.Close();
            Dispose();
        }

        // 이미지 파일 형식(포맷) 인코더
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
                socketObject = socketListener.EndAccept(ar);

                // 클라이언트의 연결 요청을 대기(다른 클라이언트가 또 연결할 수 있으므로)
                socketListener.BeginAccept(AcceptCallback, null);


                ClientObject tempClient = new ClientObject();

                tempClient.recvBuffer = new byte[4];
                tempClient.socketClient = socketObject;

                clientCount++;
                connectedClientList.Add(socketObject, "테수투!");
                Console.WriteLine($"{clientCount}테스트! {connectedClientList.Values}");
                tempClient.socketClient.BeginReceive(tempClient.recvBuffer, 0, tempClient.recvBuffer.Length, SocketFlags.None, asyncReceiveCallback, tempClient);
            }
        }

        private static void asyncReceiveCallback(IAsyncResult ar)
        {
            ClientObject co = ar.AsyncState as ClientObject;
            co.socketClient.EndReceive(ar);

            if (co.socketClient.Connected)
            {
               
                
                if (Encoding.UTF8.GetString(co.recvBuffer).Contains("recv"))
                {
                    byte[] serializeData;
                    if (standardSignalObj.ServerScreenData != null)
                    {
                        // 방송중일 때는 이미지랑 같이 넣어서 보내도록 설정
                        standardSignalObj.ServerScreenData = imageData;
                        
                        serializeData = SignalObjToByte(standardSignalObj);
                        co.socketClient.BeginSend(serializeData, 0, serializeData.Length,
                           SocketFlags.None, asyncSendCallback, co.socketClient);
                    }
                    else
                    {
                        serializeData = SignalObjToByte(standardSignalObj);
                        // 서버 측에서 방송중인 상태가 아닐 경우에는 그냥 서버 데이터가 담긴 데이터를 일반적으로 보냄
                        co.socketClient.BeginSend(serializeData, 0, serializeData.Length,
                          SocketFlags.None, asyncSendCallback, co.socketClient);
                    }
                    Array.Clear(co.recvBuffer, 0, co.recvBuffer.Length);
                }
                if (Thread.Yield()) Thread.Sleep(40);
                co.socketClient.BeginReceive(co.recvBuffer, 0, co.recvBuffer.Length, SocketFlags.None, asyncReceiveCallback, co);
            }
        }

        private static void asyncSendCallback(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            if (socket.Connected) socket.EndSend(ar);
            else socket.Close();
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

            // 실제 서비스할 땐 해상도가 다른 PC들을 포괄적으로 수용하기 위해 위에 소스를 사용할 것임.
            // 다만, 테스트 과정에서 창모드를 사용하기 위해서는 아래 소스처럼 픽셀값을 지정해줘야 함.
            // DPI 문제라고는 하나 이거까지 알 필요는 없다고 봄.
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
            if (standardSignalObj.ServerScreenData == null)
            {
                standardSignalObj.ServerScreenData = imageData;

                // 폼 버튼 변경
                btnStart.Text = "공유 중지";

                // 트레이 아이콘 공유 버튼 상태 변경
                notifyIcon.ContextMenu.MenuItems[0].Checked = true;
            }
            else
            {
                standardSignalObj.ServerScreenData = null;

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
            if (standardSignalObj.IsServerControlling)
            {
                standardSignalObj.IsServerControlling = false;
                notifyIcon.ContextMenu.MenuItems[1].Checked = false;

                MessageBox.Show("키보드와 마우스 제어를 중지하였습니다.", "제어 중지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnControl.Text = "제어";
            }
            else
            {
                standardSignalObj.IsServerControlling = true;
                notifyIcon.ContextMenu.MenuItems[1].Checked = true;

                MessageBox.Show("키보드와 마우스 제어를 시작하였습니다.", "제어 시작", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnControl.Text = "중지";
            }
        }
    }
}