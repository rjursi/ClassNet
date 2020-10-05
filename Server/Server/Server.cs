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
using System.Collections;

namespace Server
{
    public partial class Server : Form
    {
        private static SignalObj standardSignalObj; // 서버 표준 신호 객체

        private const int CLASSNETPORT = 53178;

        private class ClientObject
        {
            public Byte[] buffer;
            public Socket client;
            public string address;
        }

        private static Hashtable stuInfoTable;

        private static Viewer clientsViewer;
        private static int clientsCount; // 접속 클라이언트 수

        private Socket listener;
        private IPEndPoint ep;

        private static Byte[] imageData;

        private static ImageCodecInfo codec;
        private static EncoderParameters param;

        public Server()
        {
            InitializeComponent();
        }

        private static void SetLoginHashtable(string clientAddr, string clientInfo)
        {
            if (!stuInfoTable.ContainsKey(clientAddr))
            {
                Console.WriteLine(clientAddr + " : " + clientInfo); // 해시테이블 입력 값 확인
                stuInfoTable.Add(clientAddr, clientInfo);
            }
        }

        private void SocketOn()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ep = new IPEndPoint(IPAddress.Any, CLASSNETPORT);

            listener.Bind(ep);
            listener.Listen(50);

            listener.BeginAccept(AcceptCallback, null);
        }

        private void NotifyIconSetting()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("실시간 방송", new EventHandler((s, ea) => BtnStreaming_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("학습자 PC 모니터링", new EventHandler((s, ea) => BtnMonitoring_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("키보드 및 마우스 잠금", new EventHandler((s, ea) => BtnLock_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("인터넷 차단", new EventHandler((s, ea) => BtnInternet_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("강의실 PC 전원 종료", new EventHandler((s, ea) => BtnPower_Click(s, ea))));
            ctx.MenuItems.Add("-");
            ctx.MenuItems.Add(new MenuItem("클래스넷 종료", new EventHandler((s, ea) => BtnShutdown_Click(s, ea))));
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
            ThreadPool.QueueUserWorkItem(ImageCreate);

            // JPEG 손실 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);

            stuInfoTable = new Hashtable();

            clientsViewer = new Viewer();
            clientsCount = 0;
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
            if (!standardSignalObj.IsShutdown)
            {
                Socket tempSocket = listener.EndAccept(ar);

                // 클라이언트의 연결 요청을 대기 (다른 클라이언트가 또 연결할 수 있으므로)
                listener.BeginAccept(AcceptCallback, null);

                ClientObject tempClient = new ClientObject
                {
                    buffer = new byte[32],
                    client = tempSocket,
                    address = tempSocket.RemoteEndPoint.ToString().Split(':')[0]
                };

                tempClient.client.BeginReceive(tempClient.buffer, 0, tempClient.buffer.Length, SocketFlags.None, AsyncReceiveCallback, tempClient);
            }
        }

        private static void AsyncReceiveCallback(IAsyncResult ar)
        {
            ClientObject co = ar.AsyncState as ClientObject;

            if (co.client.Connected)
            {
                try
                {
                    co.client.EndReceive(ar);

                    if (Encoding.UTF8.GetString(co.buffer).Contains("recv"))
                    {
                        string receiveLoginData = Encoding.UTF8.GetString(co.buffer);
                        if (standardSignalObj.ServerScreenData != null) standardSignalObj.ServerScreenData = imageData;
                    }
                    else if (Encoding.UTF8.GetString(co.buffer).Contains("info"))
                    {
                        string receiveLoginData = Encoding.UTF8.GetString(co.buffer);

                        SetLoginHashtable(co.address, receiveLoginData.Split('&')[1]); // 해시테이블에 학생 정보 저장.

                        clientsCount++;

                        Viewer.currentClientsCount = clientsCount;
                        Viewer.connectedClientsList = stuInfoTable;
                    }

                    byte[] signal = SignalObjToByte(standardSignalObj);
                    co.client.BeginSend(signal, 0, signal.Length, SocketFlags.None, AsyncSendCallback, co.client);

                    signal = null;
                    Array.Clear(co.buffer, 0, co.buffer.Length);

                    co.client.BeginReceive(co.buffer, 0, co.buffer.Length, SocketFlags.None, AsyncReceiveCallback, co);
                }
                catch (SocketException)
                {
                    stuInfoTable.Remove(co.address);

                    clientsCount--;

                    Viewer.currentClientsCount = clientsCount;
                    Viewer.connectedClientsList = stuInfoTable;

                    co.client.Close();
                    co = null;
                }
            }
        }

        private static void AsyncSendCallback(IAsyncResult ar)
        {
            Socket client = ar.AsyncState as Socket;
            if (client.Connected) client.EndSend(ar);
            else client.Close();
        }

        private static byte[] SignalObjToByte(SignalObj signalObj)
        {
            string jsonData = JsonConvert.SerializeObject(signalObj);
            byte[] buffer = Encoding.Default.GetBytes(jsonData);

            return buffer;
        }

        public static void ImageCreate(Object obj)
        {
            Byte[] preData;

            Bitmap bmp = new Bitmap(1920, 1080);
            // 실제 서비스할 땐 해상도가 다른 PC들을 포괄적으로 수용하기 위해 아래 소스를 사용할 것임.
            // Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics g = Graphics.FromImage(bmp);

            while (!standardSignalObj.IsShutdown)
            {
                try
                {
                    g.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));
                }
                catch (Win32Exception)
                {
                    // 가상 데스크톱으로 이동하면 화면을 복사하지 않음.
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

        private void BtnStreaming_Click(object sender, EventArgs e)
        {
            // 스위치 역할을 하도록 수정
            if (standardSignalObj.ServerScreenData == null)
            {
                standardSignalObj.ServerScreenData = imageData;

                // 폼 버튼 변경
                btnStreaming.Text = "방송 중지";

                // 트레이 아이콘 공유 버튼 상태 변경
                notifyIcon.ContextMenu.MenuItems[0].Checked = true;
            }
            else
            {
                standardSignalObj.ServerScreenData = null;

                btnStreaming.Text = "실시간 방송";

                notifyIcon.ContextMenu.MenuItems[0].Checked = false;
            }
        }

        private void BtnMonitoring_Click(object sender, EventArgs e)
        {
            clientsViewer.ShowDialog();
        }

        private void BtnLock_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsLock)
            {
                standardSignalObj.IsLock = false;
                notifyIcon.ContextMenu.MenuItems[1].Checked = false;

                MessageBox.Show("키보드 및 마우스 잠금을 해제하였습니다.", "잠금 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLock.Text = "키보드 및 마우스 잠금";
            }
            else
            {
                standardSignalObj.IsLock = true;
                notifyIcon.ContextMenu.MenuItems[1].Checked = true;

                MessageBox.Show("키보드와 마우스 잠금을 설정하였습니다.", "잠금 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLock.Text = "잠금 해제";
            }
        }

        private void BtnInternet_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsInternet)
            {
                standardSignalObj.IsInternet = false;
                notifyIcon.ContextMenu.MenuItems[2].Checked = false;

                MessageBox.Show("인터넷 차단을 해제하였습니다.", "차단 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnInternet.Text = "인터넷 차단";
            }
            else
            {
                standardSignalObj.IsInternet = true;
                notifyIcon.ContextMenu.MenuItems[2].Checked = true;

                MessageBox.Show("인터넷 차단을 설정하였습니다.", "차단 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnInternet.Text = "차단 해제";
            }
        }

        private void BtnPower_Click(object sender, EventArgs e)
        {
            standardSignalObj.IsPower = true;
            Thread.Sleep(500);
            standardSignalObj.IsPower = false;
        }

        private void BtnShutdown_Click(object sender, EventArgs e)
        {
            standardSignalObj.IsShutdown = true;
            standardSignalObj.IsInternet = false;
            standardSignalObj.IsLock = false;

            if (listener != null) listener.Close();
            Dispose();
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            standardSignalObj.IsShutdown = true;
            standardSignalObj.IsInternet = false;
            standardSignalObj.IsLock = false;

            if (listener != null) listener.Close();
            Dispose();
        }
    }
}