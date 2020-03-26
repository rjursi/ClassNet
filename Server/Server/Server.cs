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

namespace Server
{
    public partial class Server : Form
    {
        private const int MOSHPORT = 9990;
        
        Socket socketListener;
        Socket socketClient;
        IPEndPoint serverEndPoint;

        static bool serverShutdownFlag;
        bool serverCtrlFlag;

        static ImageCodecInfo codec;
        static EncoderParameters param;

        private ServerProcMsgSender commander;

        public Thread ctrlStartThread, ctrlStopThread;

        public Server()
        {
            InitializeComponent();
            serverShutdownFlag = false;
            serverCtrlFlag = false;

            commander = new ServerProcMsgSender();
            commander.executeCommanderProcess();
        }

        private void Server_Load(object sender, EventArgs e)
        {
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

            ThreadPool.SetMinThreads(40, 40);
            ThreadPool.SetMaxThreads(50, 50);
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            serverShutdownFlag = true;
            AllThreadKill();
            commander.serverShutdown();


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
            if (!serverShutdownFlag)
            {
                socketClient = socketListener.EndAccept(ar);
                // 클라이언트의 연결 요청을 대기(다른 클라이언트가 또 연결할 수 있으므로)
                socketListener.BeginAccept(AcceptCallback, null);

                ThreadPool.QueueUserWorkItem(clientThread, socketClient);

                multiProcessing();
            }
        }

        public static void clientThread(Object ParamSocketClient)
        {
            Byte[] recvData = new Byte[4];
            Byte[] sendData;

            Socket socketClient = (Socket)ParamSocketClient;

            while (!serverShutdownFlag)
            {
                if (socketClient.Receive(recvData) > 0)
                {
                    sendData = imgCreate();
                    socketClient.Send(BitConverter.GetBytes(sendData.Length));

                    try
                    {
                        if (socketClient.Receive(recvData) > 0) socketClient.Send(sendData);
                    }
                    catch (SocketException)
                    {
                        break;
                    }

                    Array.Clear(sendData, 0, sendData.Length);
                    Array.Clear(recvData, 0, recvData.Length);
                }
                //Thread.Yield();
                if (Thread.Yield()) Thread.Sleep(40);
            }
        }

        static public Byte[] imgCreate()
        {
            Byte[] preData, postData;

            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));

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
                    postData = post_ms.ToArray();
                }
            }
            return postData;
        }

        private void multiProcessing()
        {
            Process currentProcess = Process.GetCurrentProcess();

            foreach (ProcessThread processThread in currentProcess.Threads)
            {
                processThread.ProcessorAffinity = currentProcess.ProcessorAffinity;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            notifyIcon.ContextMenu.MenuItems[0].Checked = true;

            // 초기 서버 한번 켜진 이후 다시 한번 켜지지 않도록 수정
            btnStart.Enabled = false;

            // 클라이언트 연결 대기
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, MOSHPORT);

            socketListener.Bind(serverEndPoint);
            socketListener.Listen(50);

            socketListener.BeginAccept(AcceptCallback, null);
        }

        private void AllThreadKill()
        {
            if (ctrlStopThread != null && ctrlStopThread.IsAlive)
            {
                ctrlStopThread.Abort();
            }
                
                
            if (ctrlStartThread != null && ctrlStartThread.IsAlive)
            {
                ctrlStartThread.Abort();
            }
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            serverShutdownFlag = true;

            AllThreadKill();
            commander.serverShutdown();

            if (socketListener != null) socketListener.Close();
            Dispose();
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            switch (serverCtrlFlag)
            {
                case false:
                    {
                        notifyIcon.ContextMenu.MenuItems[1].Checked = true;
                        commander.ctrlStatus = true;
                        
                        if (this.ctrlStopThread != null)
                        {
                            if (this.ctrlStopThread.IsAlive)
                            {
                                this.ctrlStopThread.Join();
                            }
                        }
                        
                        this.ctrlStartThread = new Thread(commander.ctrlStart);
                        this.ctrlStartThread.Start();

                        MessageBox.Show("키보드 마우스 제어를 시작하였습니다.", "제어 시작", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnControl.Text = "중지";
                        
                        this.serverCtrlFlag = true;
                    
                        break;
                    }
                case true:
                    {
                        notifyIcon.ContextMenu.MenuItems[1].Checked = false;
                        commander.ctrlStatus = false;

                        if (this.ctrlStartThread != null)
                        {
                            if (this.ctrlStartThread.IsAlive)
                            {
                                this.ctrlStartThread.Join();
                            }
                        }
                        
                        this.ctrlStopThread = new Thread(commander.ctrlStop);
                        this.ctrlStopThread.Start();
                        
                        MessageBox.Show("키보드 마우스 제어를 중지하였습니다.", "제어 중지", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnControl.Text = "제어";

                        this.serverCtrlFlag = false;
                    
                        break;
                    }
            }
        }

    }
}
