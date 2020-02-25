using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace monitorSharing_server
{
    public partial class Server : Form
    {
        Socket socketListener;
        Socket socketClient;
        IPEndPoint serverEndPoint;
        static bool serverShutdownFlag;

        static ImageCodecInfo codec;
        static EncoderParameters param;

        public Server()
        {
            InitializeComponent();
            serverShutdownFlag = false;
        }

        private void serverForm_Load(object sender, EventArgs e)
        {
            // JPEG 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);
        }

        private void btn_serverStart_Click(object sender, EventArgs e)
        {
            // 클라이언트 연결 대기
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, 7979);

            socketListener.Bind(serverEndPoint);
            socketListener.Listen(10);

            socketListener.BeginAccept(AcceptCallback, null);

            
        }

        private void btn_serverClose_Click(object sender, EventArgs e)
        {
            serverShutdownFlag = true;

            if(socketListener != null)
                socketListener.Close();

            Dispose();
        }

        private void serverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            serverShutdownFlag = true;
            if (socketListener != null)
                socketListener.Close();
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
           

                new Thread(() => clientThread(socketClient)).Start();
            // 이미지를 전달하고 그러는 스레드를 다시 생성


                threadMultiProcessing();
            }
        }

        public static void clientThread(Socket socketClient)
        {
            while (!serverShutdownFlag)
            {
                Byte[] recvData = new Byte[5];
                try
                {
                    socketClient.Receive(recvData);
                }
                catch (SocketException)
                {
                    break;
                }
                

                // 클라이언트가 꺼졌다는 신호 추가
                // 그 것에 따라 해당 스레드가 끝나도록 조정


                String compData = Encoding.UTF8.GetString(recvData);

                if (compData.CompareTo("ready") == 0)
                {
                    Bitmap bmp = new Bitmap(1920, 1080);
                    Graphics g = Graphics.FromImage(bmp);
                    MemoryStream ms = new MemoryStream();

                    g.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));

                    //bmp.Save(ms, codec, param);

                    Bitmap bmpResize = new Bitmap(bmp, new Size(1280, 720));
                    bmpResize.Save(ms, codec, param);

                    Byte[] sendData = ms.ToArray();
                    
                    //socketClient.Send(BitConverter.GetBytes(sendData.Length));

                    try
                    {
                        socketClient.Send(sendData);
                    }
                    catch (SocketException)
                    {

                    }
                    

                    Array.Clear(sendData, 0, sendData.Length);
                    Array.Clear(recvData, 0, 5);
                }

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
                //processThread.IdealProcessor = 3; // 해당 스레드가 실행되는 기본 프로세서를 설정하는 것
                // 결국 더 필요할 경우 다른 프로세서 까지 사용



                processThread.ProcessorAffinity = currentProcess.ProcessorAffinity;
                // processThread.ProcessorAffinity = (IntPtr)8;
                // 2진수, 해당 프로세서 사용
                // 프로세서 고정 코드

            }
        }
        
    }
}
