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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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

        private Socket listener;
        private IPEndPoint ep;

        private static Byte[] imageData;

        private static ImageCodecInfo codec;
        private static EncoderParameters param;

        private Viewer clientsViewer;

        private static Screen[] sc;
        private static int selectedScreen;

        private static Bitmap bmp;
        private static Graphics g;

        public Server()
        {
            InitializeComponent();
        }

        // DPI 설정 부분 시작
        private enum ProcessDPIAwareness
        {
            ProcessDPIUnaware = 0,
            ProcessSystemDPIAware = 1,
            ProcessPerMonitorDPIAware = 2
        }

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(ProcessDPIAwareness value);

        private static void SetDpiAwareness()
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                    SetProcessDpiAwareness(ProcessDPIAwareness.ProcessPerMonitorDPIAware);
            }
            catch (EntryPointNotFoundException) { } // OS가 해당 API를 구현하지 않을 경우 예외가 발생하지만 무시
        }
        // DPI 설정 부분 끝

        private static void LoginRecord(string clientAddr, string clientInfo)
        {
            if (!Viewer.clientsList.ContainsKey(clientAddr))
            {
                Console.WriteLine(clientAddr + " : " + clientInfo); // 해시테이블 입력 값 확인
                Viewer.Student stu = new Viewer.Student()
                {
                    info = clientInfo,
                    img = null
                };
                Viewer.clientsList.Add(clientAddr, stu);
            }
        }

        private void SocketOn()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ep = new IPEndPoint(IPAddress.Any, CLASSNETPORT);

            listener.Bind(ep);
            listener.Listen(50);

            listener.BeginAccept(AcceptCallback, null);

            // DPI 설정 메소드 호출
            SetDpiAwareness();
        }

        private void NotifyIconSetting()
        {
            ContextMenu ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("실시간 방송", new EventHandler((s, ea) => BtnStreaming_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("화면 모니터링", new EventHandler((s, ea) => BtnMonitoring_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("인터넷 차단", new EventHandler((s, ea) => BtnInternet_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("입력 잠금", new EventHandler((s, ea) => BtnLock_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("작업관리자 잠금 해제", new EventHandler((s, ea) => BtnCtrlTaskMgr_Click(s, ea))));
            ctx.MenuItems.Add(new MenuItem("학생 PC 전체 종료", new EventHandler((s, ea) => BtnPower_Click(s, ea))));
            ctx.MenuItems.Add("-");
            ctx.MenuItems.Add(new MenuItem("클래스넷 종료", new EventHandler((s, ea) => { })));

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

            // JPEG 손실 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);

            clientsViewer = new Viewer();
            clientsViewer.FormClosed += new FormClosedEventHandler(Viewer_FormClosed);

            
            sc = Screen.AllScreens;
            foreach(var mon in sc)
            {
                cbMonitor.Items.Add(Regex.Replace(mon.DeviceName, @"[^0-9a-zA-Z가-힣]", "").Trim());
            }
            cbMonitor.SelectedIndex = 0;
            selectedScreen = cbMonitor.SelectedIndex;

            bmp = null;
            g = null;


            ThreadPool.QueueUserWorkItem(ImageCreate);
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
                    buffer = new Byte[32],
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
                        Console.WriteLine(co.buffer.Length);
                        string receiveLoginData = Encoding.UTF8.GetString(co.buffer);
                        if (standardSignalObj.ServerScreenData != null) standardSignalObj.ServerScreenData = imageData;
                    }
                    else if (Encoding.UTF8.GetString(co.buffer).Contains("info"))
                    {
                        Console.WriteLine(co.buffer.Length);
                        string receiveLoginData = Encoding.UTF8.GetString(co.buffer);

                        LoginRecord(co.address, receiveLoginData.Split('&')[1]); // 해시테이블에 학생 정보 저장.

                        Viewer.currentClientsCount++;
                    }
                    else
                    {
                        if(co.buffer.Length > 4) ImageOutput(co.address, co.buffer);
                    }

                    Byte[] signal = SignalObjToByte(standardSignalObj);
                    co.client.BeginSend(signal, 0, signal.Length, SocketFlags.None, AsyncSendCallback, co.client);

                    signal = null;
                    Array.Clear(co.buffer, 0, co.buffer.Length);

                    if (standardSignalObj.IsMonitoring) co.buffer = new Byte[327675];
                    else co.buffer = new Byte[4];

                    co.client.BeginReceive(co.buffer, 0, co.buffer.Length, SocketFlags.None, AsyncReceiveCallback, co);
                }
                catch (SocketException)
                {
                    Viewer.clientsList.Remove(co.address);
                    Viewer.currentClientsCount--;

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

        private static Byte[] SignalObjToByte(SignalObj signalObj)
        {
            string jsonData = JsonConvert.SerializeObject(signalObj);
            Byte[] buffer = Encoding.Default.GetBytes(jsonData);

            return buffer;
        }

        public static void ImageCreate(object obj)
        {
            Byte[] preData;

            while (!standardSignalObj.IsShutdown)
            {
                bmp = new Bitmap(sc[selectedScreen].Bounds.Width, sc[selectedScreen].Bounds.Height);
                g = Graphics.FromImage(bmp);

                try
                {
                    g.CopyFromScreen(new Point(sc[selectedScreen].Bounds.X, sc[selectedScreen].Bounds.Y), new Point(0, 0), new Size(bmp.Width, bmp.Height));
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

        public static void ImageOutput(string clientAddr, Byte[] recvData)
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
                        finally
                        {
                            ds.Close();
                        }
                    }
                    Viewer.Student stu = Viewer.clientsList[clientAddr];
                    stu.img = Image.FromStream(post_ms);
                    Viewer.clientsList[clientAddr] = stu;

                    post_ms.Close();
                }
                pre_ms.Close();
            }
        }

        private void BtnStreaming_Click(object sender, EventArgs e)
        {
            // 스위치 역할을 하도록 수정
            if (standardSignalObj.ServerScreenData == null)
            {
                standardSignalObj.ServerScreenData = imageData;

                // 폼 버튼 변경

                //btnStreaming.Text = "방송 중지";
                btnStreaming.Image = Resource._01imgStreaming_off;

                // 트레이 아이콘 공유 버튼 상태 변경
                notifyIcon.ContextMenu.MenuItems[0].Checked = true;
            }
            else
            {
                standardSignalObj.ServerScreenData = null;

                //btnStreaming.Text = "실시간 방송";
                btnStreaming.Image = Resource._01imgStreaming_on;

                notifyIcon.ContextMenu.MenuItems[0].Checked = false;
            }
        }

        private void BtnMonitoring_Click(object sender, EventArgs e)
        {


            standardSignalObj.IsMonitoring = true;
            clientsViewer.ShowDialog();

        }

        private void BtnInternet_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsInternet)
            {
                standardSignalObj.IsInternet = false;
                btnInternet.Text = "인터넷 차단";
                notifyIcon.ContextMenu.MenuItems[2].Checked = false;

                MessageBox.Show("인터넷 차단을 해제하였습니다.", "인터넷 차단 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnInternet.Image = Resource._03imgInternet_on;

            }
            else
            {
                standardSignalObj.IsInternet = true;
                btnInternet.Text = "인터넷 차단 해제";
                notifyIcon.ContextMenu.MenuItems[2].Checked = true;


                MessageBox.Show("인터넷 차단을 설정하였습니다.", "인터넷 차단", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnInternet.Image = Resource._03imgInternet_off;

            }
        }

        private void BtnLock_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsLock)
            {
                standardSignalObj.IsLock = false;
                btnLock.Text = "입력 잠금";
                notifyIcon.ContextMenu.MenuItems[3].Checked = false;

                MessageBox.Show("입력 잠금을 해제하였습니다.", "입력 잠금 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                standardSignalObj.IsLock = true;
                btnLock.Text = "입력 잠금 해제";
                notifyIcon.ContextMenu.MenuItems[3].Checked = true;

                MessageBox.Show("입력 잠금을 설정하였습니다.", "입력 잠금", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCtrlTaskMgr_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsTaskMgrEnabled)
            {
                standardSignalObj.IsTaskMgrEnabled = false;
                btnCtrlTaskMgr.Text = "작업관리자 잠금 해제";
                notifyIcon.ContextMenu.MenuItems[4].Checked = false;

                MessageBox.Show("작업관리자 잠금을 해제하였습니다.", "작업관리자 잠금 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                standardSignalObj.IsTaskMgrEnabled = true;

                btnCtrlTaskMgr.Text = "작업관리자 잠금";
                notifyIcon.ContextMenu.MenuItems[4].Checked = true;

                btnCtrlTaskMgr.Image = Resource._05imgCtrlTaskMgr_on;
                notifyIcon.ContextMenu.MenuItems[4].Text = "작업관리자 비활성화";


                MessageBox.Show("작업관리자 잠금을 설정하였습니다.", "작업관리자 잠금", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnPower_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("연결된 학생 PC 전체를 종료하시겠습니까?", "학생 PC 전체 종료",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {

                standardSignalObj.IsPower = true;
                Thread.Sleep(500);
                standardSignalObj.IsPower = false;

            }
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            standardSignalObj.IsShutdown = true;
            standardSignalObj.IsMonitoring = false;
            standardSignalObj.IsInternet = false;
            standardSignalObj.IsLock = false;
            standardSignalObj.IsTaskMgrEnabled = true;

            if (listener != null) listener.Close();
            Dispose();

            standardSignalObj.IsMonitoring = false;
        }

        private void Viewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            standardSignalObj.IsMonitoring = false;
            clientsViewer.Close();
        }

        private void CbMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedScreen = cbMonitor.SelectedIndex;
        }
    }
}