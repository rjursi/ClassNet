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
using System.Collections;

namespace Server
{
    public partial class Server : Form
    {
        private static SignalObj standardSignalObj; // 서버 표준 신호 객체

        private const int MOSHPORT = 53178;

        private class ClientObject
        {
            public Byte[] recvBuffer;
            public Socket socketClient;
        }

        private static Hashtable stuInfoTable;

        private static ClientsView clientsView;
        private static int clientCount; // 접속 클라이언트 수

        private Socket socketListener;
        private Socket socketObject;
        private IPEndPoint serverEndPoint;

        private static Byte[] imageData;

        private static ImageCodecInfo codec;
        private static EncoderParameters param;

        public Server()
        {
            InitializeComponent();
        }

        // Form_Load를 하고 동적으로 PictureBox 생성하는 로직
        public void ClientBoxes_Load(object sender, EventArgs e)
        {
            DynamicClientBoxesCreate();
        }

        public void DynamicClientBoxesCreate()
        {

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
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, MOSHPORT);

            socketListener.Bind(serverEndPoint);
            socketListener.Listen(50);

            socketListener.BeginAccept(AcceptCallback, null);
        }

        private void NotifyIconSetting()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("화면 전송", new EventHandler((s, ea) => BtnScreenSend_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("조작 제어", new EventHandler((s, ea) => BtnControl_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("인터넷 제어", new EventHandler((s, ea) => BtnInternetControl_Click(s, ea))));
            ctx.MenuItems.Add("-");
            ctx.MenuItems.Add(new MenuItem("종료", new EventHandler((s, ea) => BtnShutdown_Click(s, ea))));
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

            clientsView = new ClientsView();
            clientCount = 0;
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

                ClientObject tempClient = new ClientObject
                {
                    recvBuffer = new byte[32],
                    socketClient = socketObject
                };

                tempClient.socketClient.BeginReceive(tempClient.recvBuffer, 0, tempClient.recvBuffer.Length, SocketFlags.None, AsyncReceiveCallback, tempClient);
            }
        }

        private static void AsyncReceiveCallback(IAsyncResult ar)
        {
            ClientObject co = ar.AsyncState as ClientObject;

            if (co.socketClient.Connected)
            {
                try
                {
                    co.socketClient.EndReceive(ar);

                    if (Encoding.UTF8.GetString(co.recvBuffer).Contains("recv"))
                    {
                        string receiveLoginData = Encoding.UTF8.GetString(co.recvBuffer);
                        if (standardSignalObj.ServerScreenData != null) standardSignalObj.ServerScreenData = imageData;
                    }
                    else if (Encoding.UTF8.GetString(co.recvBuffer).Contains("info"))
                    {
                        string receiveLoginData = Encoding.UTF8.GetString(co.recvBuffer);
                        string clientIP = co.socketClient.RemoteEndPoint.ToString(); //.Split(':')[0]

                        SetLoginHashtable(clientIP, receiveLoginData.Split('&')[1]); // 해시테이블에 학생 정보 저장.

                        clientCount++;

                        ClientsView.currentClientCount = clientCount;
                        ClientsView.connectedClientList = stuInfoTable;
                    }

                    byte[] signal = SignalObjToByte(standardSignalObj);
                    co.socketClient.BeginSend(signal, 0, signal.Length, SocketFlags.None, AsyncSendCallback, co.socketClient);

                    signal = null;
                    Array.Clear(co.recvBuffer, 0, co.recvBuffer.Length);

                    co.socketClient.BeginReceive(co.recvBuffer, 0, co.recvBuffer.Length, SocketFlags.None, AsyncReceiveCallback, co);
                }
                catch (SocketException)
                {
                    clientCount--;
                    ClientsView.currentClientCount = clientCount;
                    ClientsView.connectedClientList.Remove(co.socketClient.RemoteEndPoint.ToString());
                    stuInfoTable.Remove(co.socketClient.RemoteEndPoint.ToString());
                    co.socketClient.Close();
                }
            }
        }

        private static void AsyncSendCallback(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            if (socket.Connected) socket.EndSend(ar);
            else socket.Close();
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

            while (!standardSignalObj.IsServerShutdown)
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

        private void BtnScreenSend_Click(object sender, EventArgs e)
        {
            // 스위치 역할을 하도록 수정
            if (standardSignalObj.ServerScreenData == null)
            {
                standardSignalObj.ServerScreenData = imageData;

                // 폼 버튼 변경
                btnScreenSend.Text = "전송 중지";

                // 트레이 아이콘 공유 버튼 상태 변경
                notifyIcon.ContextMenu.MenuItems[0].Checked = true;
            }
            else
            {
                standardSignalObj.ServerScreenData = null;

                btnScreenSend.Text = "화면 전송";

                notifyIcon.ContextMenu.MenuItems[0].Checked = false;
            }
        }

        private void BtnShutdown_Click(object sender, EventArgs e)
        {

            standardSignalObj.IsServerShutdown = true;
            standardSignalObj.IsServerInternetControlling = false;
            standardSignalObj.IsServerControlling = false;

            if (socketListener != null) socketListener.Close();
            Dispose();
        }

        private void BtnControl_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsServerControlling)
            {
                standardSignalObj.IsServerControlling = false;
                notifyIcon.ContextMenu.MenuItems[1].Checked = false;

                MessageBox.Show("키보드와 마우스 제어를 중지하였습니다.", "제어 중지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnControl.Text = "조작 제어";
            }
            else
            {
                standardSignalObj.IsServerControlling = true;
                notifyIcon.ContextMenu.MenuItems[1].Checked = true;

                MessageBox.Show("키보드와 마우스 제어를 시작하였습니다.", "제어 시작", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnControl.Text = "제어 중지";
            }
        }

        private void BtnClientsView_Click(object sender, EventArgs e)
        {
            clientsView.ShowDialog();
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            standardSignalObj.IsServerShutdown = true;
            standardSignalObj.IsServerInternetControlling = false;
            standardSignalObj.IsServerControlling = false;

            standardSignalObj = null;

            if (socketListener != null) socketListener.Close();
            Dispose();
        }

        private void BtnInternetControl_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsServerInternetControlling)
            {
                standardSignalObj.IsServerInternetControlling = false;
                notifyIcon.ContextMenu.MenuItems[2].Checked = false;

                MessageBox.Show("인터넷 제어를 중지하였습니다.", "제어 중지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnInternetControl.Text = "인터넷 제어";
            }
            else
            {
                standardSignalObj.IsServerInternetControlling = true;
                notifyIcon.ContextMenu.MenuItems[2].Checked = true;

                MessageBox.Show("인터넷제어를 시작하였습니다.", "제어 시작", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnInternetControl.Text = "제어 중지";
            }
        }
    }
}