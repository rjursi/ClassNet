using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace monitorSharing_server
{
    public partial class Form1 : Form
    {
        Socket socketListener;
        IPEndPoint serverEndPoint;

        static ImageCodecInfo codec;
        static EncoderParameters param;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // JPEG 압축 수준 설정
            codec = GetEncoder(ImageFormat.Jpeg);
            param = new EncoderParameters();
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 클라이언트 연결 대기
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(IPAddress.Any, 7979);

            socketListener.Bind(serverEndPoint);
            socketListener.Listen(10);

            socketListener.BeginAccept(AcceptCallback, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            socketListener.Close();
            Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            Socket socketClient = socketListener.EndAccept(ar);

            // 클라이언트의 연결 요청을 대기(다른 클라이언트가 또 연결할 수 있으므로)
            socketListener.BeginAccept(AcceptCallback, null);

            new Thread(() => clientThread(socketClient)).Start();
        }

        public static void clientThread(Socket socketClient)
        {
            while (true)
            {
                Byte[] recvData = new Byte[5];
                socketClient.Receive(recvData);

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
                    socketClient.Send(sendData);

                    Array.Clear(sendData, 0, sendData.Length);
                    Array.Clear(recvData, 0, 5);
                }
            }
        }
    }
}
