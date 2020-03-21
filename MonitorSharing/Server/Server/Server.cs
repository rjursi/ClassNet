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

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            serverShutdownFlag = true;
            AllThreadKill();
            commander.serverShutdown();


            if (socketListener != null)
                socketListener.Close();
            Dispose();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            



            // JPEG 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);

            
            ThreadPool.SetMinThreads(40, 40);
            ThreadPool.SetMaxThreads(60, 60);
            
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

                //new Thread(() => clientThread(socketClient)).Start();
                // 이미지를 전달하는 스레드 생성

                ThreadPool.QueueUserWorkItem(clientThread, socketClient);

                threadMultiProcessing();
            }
        }

        public static void clientThread(Object ParamSocketClient)
        {
            Byte[] recvData = new Byte[5], preData, sendData;
            String compData = null;

            Socket socketClient = (Socket)ParamSocketClient;

            while (!serverShutdownFlag)
            {
                try
                {
                    socketClient.Receive(recvData);
                }
                catch (SocketException)
                {
                    break;
                }
                // 클라이언트가 꺼졌다는 신호 추가
                // 그것에 따라 해당 스레드가 끝나도록 조정

                compData = Encoding.UTF8.GetString(recvData);

                if (compData.CompareTo("ready") == 0)
                {
                    Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));

                    using (MemoryStream pre_ms = new MemoryStream())
                    {
                        /*Bitmap bmpResize = new Bitmap(bmp, new Size(1280, 720));
                        bmpResize.Save(pre_ms, codec, param);*/

                        bmp.Save(pre_ms, codec, param);
                        preData = pre_ms.ToArray();

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Compress))
                            {
                                ds.Write(preData, 0, preData.Length);
                                ds.Close();
                            }
                            sendData = ms.ToArray();

                            try
                            {
                                socketClient.Send(BitConverter.GetBytes(sendData.Length));

                                try
                                {
                                    socketClient.Receive(recvData);
                                }
                                catch (SocketException)
                                {
                                    break;
                                }

                                compData = Encoding.UTF8.GetString(recvData);

                                if (compData.CompareTo("start") == 0)
                                {
                                    socketClient.Send(sendData);
                                }
                            }
                            catch (SocketException) { }

                            Array.Clear(sendData, 0, sendData.Length);
                            Array.Clear(preData, 0, preData.Length);
                            Array.Clear(recvData, 0, recvData.Length);

                            ms.Close();
                        }
                        pre_ms.Close();
                    }
                }
                //Thread.Sleep(100);
                Thread.Yield();
                //if (Thread.Yield()) Thread.Sleep(100);
            }
        }

        private void threadMultiProcessing()
        {
            Process currentProcess = Process.GetCurrentProcess();

            foreach (ProcessThread processThread in currentProcess.Threads)
            {
                IntPtr test = currentProcess.ProcessorAffinity; // 해당 프로세스의 실행을 예약할 수 있는 프로세서를 가져옴

                // 0x000000ff - 11111111, 즉 모든 프로세서 사용

                // 이 프로세스에 포함된 스레드의 실행을 예약할 수 있는 프로세스를 가져오거나 설정
                // processThread.IdealProcessor = 3; // 해당 스레드가 실행되는 기본 프로세서를 설정하는 것
                // 결국 더 필요할 경우 다른 프로세서 까지 사용

                processThread.ProcessorAffinity = currentProcess.ProcessorAffinity;
                // processThread.ProcessorAffinity = (IntPtr)8;
                // 2진수, 해당 프로세서 사용
                // 프로세서 고정 코드
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // 초기 서버 한번 켜진 이후 다시 한번 켜지지 않도록 수정
            btnStart.Enabled = false;

            // 클라이언트 연결 대기
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, MOSHPORT);

            socketListener.Bind(serverEndPoint);
            socketListener.Listen(10);

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
