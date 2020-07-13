using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Client
{
    public partial class Client : Form
    {
        public bool isConnected = false;

        static SignalObj standardSignalObj;

        private const int MOSHPORT = 9990;
        private const string SERVER_IP = "127.0.0.1";

        Socket socketServer;

        CmdProcssController cmdProcessController;

        delegate void ScreenOnDelegate(int imgSize, Byte[] recvData, double isOpacity);
        delegate void ScreenOffDelegate(double isOpacity);


        static Byte[] recvData = new Byte[327675]; // 327,675 Byte = 65,535 Byte * 5
        static Byte[] sendData = Encoding.UTF8.GetBytes("recv");

        //클라언트 프로그램이 주로 할 일
        Action actionInMainTask;
        //SginalObj를 반환하는 Func
        static Func<SignalObj> returnSignalObj;
        //구현 중
        delegate void FtpDownloadDelegete();



        public Client()
        {
            InitializeComponent();
        }

        public SignalObj ByteToObject(byte[] buffer)
        {
            string jsonData = "";
            SignalObj sObj;

            jsonData = Encoding.Default.GetString(buffer);

            sObj = JsonConvert.DeserializeObject<SignalObj>(jsonData);

            return sObj;
        }

        private void Client_Load(object sender, EventArgs e)
        {
            ThreadPool.SetMaxThreads(5, 5);

            cmdProcessController = new CmdProcssController();

            while (!isConnected)
            {
                try
                {
                    socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(SERVER_IP), MOSHPORT);
                    socketServer.Connect(serverEndPoint);

                    // 작업표시줄 상에서 프로그램이 표시되지 않도록 설정
                    // 개인 테스트 과정에서 불편하므로 커밋할 때는 주석처리 해주세요.
                    // this.ShowInTaskbar = false;

                    // 받은 이미지를 풀스크린으로 띄우는 설정
                    // 개인 테스트 과정에서 불편하므로 커밋할 때는 주석처리 해주세요.
                    /*FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    pictureBox1.Width = Screen.PrimaryScreen.Bounds.Width;
                    pictureBox1.Height = Screen.PrimaryScreen.Bounds.Height;*/

                    // 화면 폼을 가장 맨 위로
                    TopMost = true;

                    isConnected = true;
                }
                catch (SocketException)
                {
                    isConnected = false; // 해당 bool 변수로 인해서 다시한번 위 반복문이 실행
                }
            }

            Opacity = 0;
            
            //Func가 무엇이더냐? 미리 선언된 delegate변수, Func는 값을 반환할 수 있고 Action은 반환은 못함
            returnSignalObj = new Func<SignalObj>(receiveObj);

            //사용 방법,   line 111-128
            //receiveImage메소드가 실행된 후 콘솔창에
            //람다를
            //이용한
            //Action 추가
            //Action변수를 이용 
            //이라는 문자열이 while문 안에서 계속 출력됨
            //이런 식으로 사용하면 됨 while루프안에서 할일을 메소드로 만들어서 AddAction메소드에 매개변수로 넘기면 됨
            //코드를 넘길 수 있기 때문에 가장 아래처럼 똥꼬쇼가능
            AddAction(() => receiveImage());
            
            AddAction(() => {
                Console.WriteLine("람다를");
                Console.WriteLine("이용한");
                Console.WriteLine("Action 추가");
            });

            Action ac = new Action(() => Console.WriteLine("Action변수를 이용"));   
            AddAction(ac);

            AddAction(() =>
            {
                cnt++;
                if (cnt > 100)
                    Console.WriteLine("mainTask가 " + cnt + "번 실행됨");

            });

            //Task클래스의 run메소드 이용, Action을 매개변수로 받으며 매개변수로 받은 Action을 실행한다
            //실행하기 전에 뭘 할건지 미리 위에서 Action을 정의해놓았음
            //기존의 ThreadPool.QueueUserWorkItem와 비슷한 방식
            //단순히 메소드를 실행하는 게 아니라 작업에 대한 핸들을 반환할 수 있고 
            //이 방식이 시도할 수 있는게 많기 때문에 이게 완성이라 볼 순없고 아직 좀 더 개선할 수 있을듯
            //mainTask에서 할 일이 반환값을 요구하면 그에 맞춰 바꿀 수 있음
            AddAction(() =>
            {
                if (standardSignalObj.IsServerUpload == true)
                {
                    FtpDownload();
                }
            });
            Task.Run(()=> DoSomethingInTask());
        }
        static int cnt =0;//Action설명을 위한 변수로 없어도 됨

        public void AddAction(Action action)
        {
            actionInMainTask += action;
        }
        public SignalObj receiveObj()
        {
            recvData = new Byte[327675]; // 327,675 Byte = 65,535 Byte * 5
            sendData = Encoding.UTF8.GetBytes("recv");
            try
            {
                socketServer.Send(sendData);
                socketServer.Receive(recvData);
                
                return ByteToObject(recvData);
            }
            catch (SocketException)
            {
                if (cmdProcessController.NowCtrlStatus) cmdProcessController.QuitProcess();
                socketServer.Close();

                this.Invoke(new MethodInvoker(() => { Dispose(); }));
                return null;
            }

        }
        public void DoSomethingInTask()
        {
            while (true)
            {
                try
                {
                    using (standardSignalObj = returnSignalObj())
                    {
                        actionInMainTask();
                    }
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (JsonReaderException)
                {
                    Array.Clear(recvData, 0, recvData.Length);
                    break;
                }
                Array.Clear(recvData, 0, recvData.Length);
            }

        }


        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cmdProcessController.NowCtrlStatus) cmdProcessController.QuitProcess();
            socketServer.Close();

            this.Invoke(new MethodInvoker(() => { Dispose(); }));
        }

        public void receiveImage()
        {
            if (standardSignalObj.ServerScreenData != null)
            {
                // 이미지를 받아서 여기서 버퍼를 설정하는 부분
                this.Invoke(new ScreenOnDelegate(outputDelegate),
                    standardSignalObj.ServerScreenData.Length, standardSignalObj.ServerScreenData, 1);
            }
            else
            {
                this.Invoke(new ScreenOffDelegate(opacityDelegate), 0);
            }

        }

        public void opacityDelegate(double isOpacity)
        {
            Opacity = isOpacity;
        }

        public void outputDelegate(int imgSize, Byte[] recvData, double isOpacity)
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

        public void FtpDownload()
        {
            // 코드 단순화를 위해 하드코드함
            string ftpPath = "ftp://localhost/mosh/sample.txt";
            string user = "moshFtp";  // FTP 익명 로그인시. 아니면 로그인/암호 지정.
            string pwd = "123";
            string outputFile = "C:\\works/zzz/sample.txt";

            // WebRequest.Create로 Http,Ftp,File Request 객체를 모두 생성할 수 있다.
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpPath);
            // FTP 다운로드한다는 것을 표시
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            // 익명 로그인이 아닌 경우 로그인/암호를 제공해야
            req.Credentials = new NetworkCredential(user, pwd);

            // FTP Request 결과를 가져온다.
            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
            {
                // FTP 결과 스트림
                Stream stream = resp.GetResponseStream();

                // 결과를 문자열로 읽기 (바이너리로 읽을 수도 있다)
                string data;
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = reader.ReadToEnd();
                }

                // 로컬 파일로 출력
                File.WriteAllText(outputFile, data);
            }
        }


    }
}
