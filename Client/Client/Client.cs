using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading.Tasks;
using InternetControl;
using System.Drawing.Imaging;

namespace Client
{
    public partial class Client : Form
    {
        private const int CLASSNETPORT = 53178;
        private const string SERVER_IP = "127.0.0.1";

        private Socket server;
        private bool isConnected = false;

        private LoginForm loginForm;
        private string stuInfo; // 로그인 데이터를 담을 변수
        private bool isLogin = false;

        private static Action mainAction;

        private static SignalObj standardSignalObj;

        private CmdProcessController cmdProcessController;
        private FirewallPortBlock firewallPortBlocker;

        private delegate void ScreenOnDelegate(int imgSize, Byte[] recvData, double isOpacity);
        private delegate void ScreenOffDelegate(double isOpacity);

        private static Byte[] recvData;
        private static Byte[] sendData;

        private static ImageCodecInfo codec;
        private static EncoderParameters param;

        private static bool isFirst;
        private static bool isCapture;

        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            while(!isLogin)
            {
                loginForm = new LoginForm();
                loginForm.ShowDialog(); // ShowDialog 실행, 닫힐 때 까지 프로그램은 일시정지.
                stuInfo = loginForm.stuInfo; // 로그인 데이터를 변수에 담음.

                if(stuInfo.Length > 0)
                {
                    isLogin = true;
                }
            }

            while (!isConnected)
            {
                try
                {
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(SERVER_IP), CLASSNETPORT);
                    server.Connect(ep);

                    // 작업표시줄 상에서 프로그램이 표시되지 않도록 설정
                    // 개인 테스트 과정에서 불편하므로 커밋할 때는 주석처리 해주세요.
                    //this.ShowInTaskbar = false;

                    // 받은 이미지를 풀스크린으로 띄우는 설정
                    // 개인 테스트 과정에서 불편하므로 커밋할 때는 주석처리 해주세요.
                    /*FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    screenImage.Width = Screen.PrimaryScreen.Bounds.Width;
                    screenImage.Height = Screen.PrimaryScreen.Bounds.Height;*/

                    // 화면 폼을 가장 맨 위로
                    // 개인 테스트 과정에서 불편하므로 커밋할 때는 주석처리 해주세요.
                    //TopMost = true;

                    isConnected = true;
                }
                catch (SocketException)
                {
                    isConnected = false; // 연결이 안 되면 대기상태 유지
                }
            }

            cmdProcessController = new CmdProcessController();
            firewallPortBlocker = new FirewallPortBlock();

            recvData = new Byte[327675]; // 327,675 Byte = 65,535 Byte * 5
            isFirst = true;
            isCapture = false;

            // JPEG 손실 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);

            Opacity = 0;

            // 이런식으로 구현 기능에 대한 메소드를 추가하기 위해 아래와 같이 람다식으로 작성
            InsertAction(() => ControllingLock());
            InsertAction(() => ControllingInternet());
            InsertAction(() => ControllingPower());
            InsertAction(() => ImageProcessing());
            InsertAction(() => CaptureProcessing());

            Task.Run(()=> MainTask());
        }

        public void InsertAction(Action action)
        {
            mainAction += action;
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

        public SignalObj ByteToObject(Byte[] buffer)
        {
            string jsonData = Encoding.Default.GetString(buffer);
            SignalObj signal = JsonConvert.DeserializeObject<SignalObj>(jsonData);
            return signal;
        }

        public SignalObj ReceiveObject()
        {
            try
            {
                if (isFirst)
                {
                    sendData = Encoding.UTF8.GetBytes("info" + "&" + stuInfo);
                    isFirst = false;
                }
                else if (isCapture)
                {
                    sendData = CaptureImage();
                    isCapture = false;
                }
                else sendData = Encoding.UTF8.GetBytes("recv");

                server.Send(sendData);
                server.Receive(recvData);

                return ByteToObject(recvData);
            }
            catch (SocketException)
            {
                server.Close();
                if (cmdProcessController.NowCtrlStatus) cmdProcessController.QuitProcess();
                this.Invoke(new MethodInvoker(() => { Dispose(); }));

                return null;
            }
        }

        public void MainTask()
        {
            while (true)
            {
                try
                {
                    using (standardSignalObj = ReceiveObject())
                    {
                        if (standardSignalObj != null) mainAction();
                    }
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (JsonReaderException)
                {
                    break;
                }
                finally
                {
                    Array.Clear(recvData, 0, recvData.Length);
                }
            }
        }

        public void ControllingLock()
        {
            cmdProcessController.CtrlStatusEventCheck(standardSignalObj.IsLock);
        }

        public void ControllingInternet()
        {
            firewallPortBlocker.CtrlStatusEventCheck(standardSignalObj.IsInternet);
        }

        public void ControllingPower()
        {
            if (standardSignalObj.IsPower) System.Diagnostics.Process.Start("ShutDown.exe", "-s -f -t 00");
        }

        public void ImageProcessing()
        {
            if (standardSignalObj.ServerScreenData != null)
            {
                // 이미지를 받아서 여기서 버퍼를 설정하는 부분
                this.Invoke(new ScreenOnDelegate(OutputDelegate),
                    standardSignalObj.ServerScreenData.Length, standardSignalObj.ServerScreenData, 1);
            }
            else
            {
                this.Invoke(new ScreenOffDelegate(OpacityDelegate), 0);
            }
        }

        public void CaptureProcessing()
        {
            if (standardSignalObj.IsMonitoring) isCapture = true;
        }

        public Byte[] CaptureImage()
        {
            Byte[] preData;
            Byte[] tempData;

            Bitmap bmp = new Bitmap(1920, 1080);
            // 실제 서비스할 땐 해상도가 다른 PC들을 포괄적으로 수용하기 위해 아래 소스를 사용할 것임.
            // Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

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
                    tempData = post_ms.ToArray();
                    post_ms.Close();
                }
                pre_ms.Close();
            }
            return tempData;
        }

        public void OpacityDelegate(double isOpacity)
        {
            Opacity = isOpacity;
        }

        public void OutputDelegate(int imgSize, Byte[] recvData, double isOpacity)
        {
            Opacity = isOpacity;

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
                    imageSize.Text = imgSize.ToString();
                    screenImage.Image = Image.FromStream(post_ms);

                    post_ms.Close();
                }
                pre_ms.Close();
            }
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Close();
            this.Invoke(new MethodInvoker(() => { Dispose(); }));
        }
    }
}
