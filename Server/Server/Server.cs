﻿using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Server
{
    public partial class Server : Form
    {
        private Socket listener;
        private IPEndPoint ep;

        private const int CLASSNETPORT = 53178;

        private class ClientObject
        {
            public Byte[] buffer;
            public Socket client;
            public string address;
        }

        private static SignalObj standardSignalObj;
        
        private static Byte[] imageData;

        private static ImageCodecInfo codec;
        private static EncoderParameters param;

        private static Viewer clientsViewer;

        private static Screen[] sc;
        private static int selectedScreen;

        private static Bitmap bmp;
        private static Graphics g;

        private static bool isFinish;
        private static bool systemShutdown = false;
        private static int WM_QUERYENDSESSION = 0x11; // 윈도우가 종료, 로그오프, 재시작시 발생하는 메시지 상수

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == WM_QUERYENDSESSION)
            {
                systemShutdown = true;
            }
            base.WndProc(ref m);
        }

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

        private static bool LoginRecord(string clientAddr, string clientInfo)
        {
            if (!clientsViewer.clientsList.ContainsKey(clientAddr))
            {
                Student stu = new Student()
                {
                    info = clientInfo.Trim('\0'),
                    img = null
                };
                try
                {
                    clientsViewer.clientsList.Add(clientAddr, stu as Student);
                    if (clientsViewer.IsHandleCreated) clientsViewer.Invoke(clientsViewer.IPD, clientAddr);
                    else clientsViewer.clientsViewPanel.Controls.Add(clientsViewer.AddClientPanel(clientAddr));

                    return true;
                }
                catch(NullReferenceException)
                {
                    return false;
                }                
            }
            else return false;
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

            notifyIcon.ContextMenu = ctx;
            notifyIcon.Visible = true;
        }

        private void Server_Load(object sender, EventArgs e)
        {
            isFinish = false;

            // Viewer 객체 생성
            clientsViewer = new Viewer();
            clientsViewer.FormClosed += new FormClosedEventHandler(Viewer_FormClosed);

            // 작업 표시줄에서 제거
            this.ShowInTaskbar = false;

            // 표준 신호 객체 생성
            standardSignalObj = new SignalObj();

            // NotifyIcon에 메뉴 추가
            NotifyIconSetting();

            // JPEG 손실 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);

            // 선택 가능한 스크린 설정
            sc = Screen.AllScreens;
            foreach(var mon in sc)
            {
                cbMonitor.Items.Add(Regex.Replace(mon.DeviceName, @"[^0-9a-zA-Z가-힣]", "").Trim());
            }
            cbMonitor.SelectedIndex = 0;
            selectedScreen = cbMonitor.SelectedIndex;

            // 스크린 캡처에 필요한 변수 초기화
            bmp = null;
            g = null;

            // 클라이언트 연결 대기
            SocketOn();

            // 스크린 캡처를 통한 이미지 생성 태스크 실행
            Task.Run(() => ImageCreate());
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
                        string receiveLoginData = Encoding.UTF8.GetString(co.buffer);
                        if (standardSignalObj.ServerScreenData != null) standardSignalObj.ServerScreenData = imageData;
                    }
                    else if (Encoding.UTF8.GetString(co.buffer).Contains("info"))
                    {
                        string receiveLoginData = Encoding.UTF8.GetString(co.buffer);

                        // 해시테이블에 학생 정보 저장 또는 연결 해제
                        if (!LoginRecord(co.address, receiveLoginData.Split('&')[1])) co.client.Close();
                    }
                    else if (co.buffer.Length > 4) ImageOutput(co.address, co.buffer);

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
                    if (!isFinish)
                    {
                        Control[] ctrl = clientsViewer.clientsViewPanel.Controls.Find("pan_" + co.address, false);
                        if (ctrl.Length > 0)
                        {
                            if (clientsViewer.IsHandleCreated) clientsViewer.Invoke(clientsViewer.DPD, ctrl[0] as Panel);
                            else clientsViewer.clientsViewPanel.Controls.Remove(ctrl[0] as Panel);
                        }

                        if (clientsViewer.clientsList.ContainsKey(co.address)) clientsViewer.clientsList.Remove(co.address);

                        //co.client.Close();
                        //co = null;
                    }
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }
        }

        private static void AsyncSendCallback(IAsyncResult ar)
        {
            Socket client = ar.AsyncState as Socket;
            try
            {
                if (client.Connected) client.EndSend(ar);
                else client.Close();
            }
            catch(SocketException)
            {
                return;
            }            
        }

        private static Byte[] SignalObjToByte(SignalObj signalObj)
        {
            string jsonData = JsonConvert.SerializeObject(signalObj);
            Byte[] buffer = Encoding.Default.GetBytes(jsonData);

            return buffer;
        }

        public static void ImageCreate()
        {
            Byte[] preData;

            while (!standardSignalObj.IsShutdown)
            {
                bmp = new Bitmap(sc[selectedScreen].WorkingArea.Width, sc[selectedScreen].WorkingArea.Height);
                g = Graphics.FromImage(bmp);

                try
                {
                    g.CopyFromScreen(new Point(sc[selectedScreen].Bounds.X, sc[selectedScreen].Bounds.Y),
                        new Point(0, 0), new Size(bmp.Width, bmp.Height));
                } // 가상 데스크톱으로 이동하면 화면을 복사하지 않음.
                catch (Win32Exception) { }

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
            try
            {
                using (MemoryStream pre_ms = new MemoryStream(recvData))
                {
                    using (MemoryStream post_ms = new MemoryStream())
                    {
                        using (DeflateStream ds = new DeflateStream(pre_ms, CompressionMode.Decompress))
                        {
                            ds.CopyTo(post_ms);
                            ds.Close();
                        }

                        if (clientsViewer.clientsList.ContainsKey(clientAddr))
                        {
                            clientsViewer.clientsList[clientAddr].img = Image.FromStream(post_ms);
                        }
                        post_ms.Close();
                    }
                    pre_ms.Close();
                }
            }
            catch (InvalidDataException) { }
        }

        private void BtnStreaming_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsMonitoring)
            {
                notifyIcon.ContextMenu.MenuItems[1].Checked = false;
                clientsViewer.Close();
                Thread.Sleep(100);
            }

            // 스위치 역할을 하도록 수정
            if (standardSignalObj.ServerScreenData == null)
            {
                standardSignalObj.ServerScreenData = imageData;

                // 폼 버튼 변경
                btnStreaming.Image = Resource._01imgStreaming_off;

                // 트레이 아이콘 공유 버튼 상태 변경
                notifyIcon.ContextMenu.MenuItems[0].Checked = true;
            }
            else
            {
                standardSignalObj.ServerScreenData = null;

                btnStreaming.Image = Resource._01imgStreaming_on;

                notifyIcon.ContextMenu.MenuItems[0].Checked = false;
            }
        }

        private void BtnMonitoring_Click(object sender, EventArgs e)
        {
            if (notifyIcon.ContextMenu.MenuItems[0].Checked)
            {
                standardSignalObj.ServerScreenData = null;
                btnStreaming.Image = Resource._01imgStreaming_on;
                notifyIcon.ContextMenu.MenuItems[0].Checked = false;

                Thread.Sleep(100);
            }

            if(!standardSignalObj.IsMonitoring)
            {
                standardSignalObj.IsMonitoring = true;
                notifyIcon.ContextMenu.MenuItems[1].Checked = true;
                clientsViewer.ShowDialog();
            }
            else clientsViewer.Close();
        }

        private void BtnInternet_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsInternet)
            {
                standardSignalObj.IsInternet = false;
                notifyIcon.ContextMenu.MenuItems[2].Checked = false;

                btnInternet.Image = Resource._03imgInternet_on;

                MessageBox.Show("인터넷 차단을 해제하였습니다.", "인터넷 차단 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                standardSignalObj.IsInternet = true;
                notifyIcon.ContextMenu.MenuItems[2].Checked = true;

                btnInternet.Image = Resource._03imgInternet_off;

                MessageBox.Show("인터넷 차단을 설정하였습니다.", "인터넷 차단", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnLock_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsLock)
            {
                standardSignalObj.IsLock = false;
                notifyIcon.ContextMenu.MenuItems[3].Checked = false;

                btnLock.Image = Resource._04imgLock_on;

                MessageBox.Show("입력 잠금을 해제하였습니다.", "입력 잠금 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                standardSignalObj.IsLock = true;
                notifyIcon.ContextMenu.MenuItems[3].Checked = true;

                btnLock.Image = Resource._04imgLock_off;

                MessageBox.Show("입력 잠금을 설정하였습니다.", "입력 잠금", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCtrlTaskMgr_Click(object sender, EventArgs e)
        {
            if (standardSignalObj.IsTaskMgrEnabled)
            {
                standardSignalObj.IsTaskMgrEnabled = false;
                notifyIcon.ContextMenu.MenuItems[4].Checked = false;

                btnCtrlTaskMgr.Image = Resource._05imgCtrlTaskMgr_off;

                MessageBox.Show("작업관리자 잠금을 설정하였습니다.", "작업관리자 잠금", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                standardSignalObj.IsTaskMgrEnabled = true;
                notifyIcon.ContextMenu.MenuItems[4].Checked = true;

                btnCtrlTaskMgr.Image = Resource._05imgCtrlTaskMgr_on;

                MessageBox.Show("작업관리자 잠금을 해제하였습니다.", "작업관리자 잠금 해제", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnPower_Click(object sender, EventArgs e)
        {
            isFinish = true;

            if (MessageBox.Show("연결된 학생 PC 전체를 종료하시겠습니까?", "학생 PC 전체 종료",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                standardSignalObj.IsPower = true;
                standardSignalObj.ServerScreenData = null;
                standardSignalObj.IsMonitoring = false;
                standardSignalObj.IsInternet = false;
                standardSignalObj.IsLock = false;
                standardSignalObj.IsTaskMgrEnabled = false;
                Thread.Sleep(1500);

                notifyIcon.Visible = false;
                this.Dispose();
                Application.Restart();
            }
        }

        private void CbMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedScreen = cbMonitor.SelectedIndex;
        }

        private void Viewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            standardSignalObj.IsMonitoring = false;
            notifyIcon.ContextMenu.MenuItems[1].Checked = false;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.TopMost = true;
            this.TopMost = false;
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!systemShutdown)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}