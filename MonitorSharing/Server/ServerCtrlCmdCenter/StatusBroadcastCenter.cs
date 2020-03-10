using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCtrlCmdCenter
{
    class StatusBroadcastCenter
    {
        

        private const int CMDSERVICE_PORT = 9991;

        
        UdpClient client;
        IPEndPoint ip;
        byte[] bytes;
        

        Thread listenCtrlThread;

        string[] parentCtrlMsg;

        public void Send(string message)
        {
            bytes = Encoding.UTF8.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);


        }

        public StatusBroadcastCenter(string[] parentCtrlMsg)
        {

            this.parentCtrlMsg = parentCtrlMsg;

            this.client = new UdpClient();
            this.ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), CMDSERVICE_PORT);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

           

        }

        private void childProcessShutdown()
        {
            string shutdownMsg;
            byte[] shutdownMsgBytes;

            shutdownMsg = "server shutdown";

            shutdownMsgBytes = Encoding.UTF8.GetBytes(shutdownMsg);
            client.Send(shutdownMsgBytes, shutdownMsgBytes.Length, ip);
            client.Close();
        }


        
        public void startListenThread()
        {
            this.listenCtrlThread = new Thread(ListenMsgPipeServer);
            this.listenCtrlThread.Start(this.parentCtrlMsg);

        }

        public void ListenMsgPipeServer(object parentCtrlMsg)
        {

            string[] ctrlMsg = (string[])parentCtrlMsg;



            if (ctrlMsg.Length > 0)
            {
                using (PipeStream pipeClient = new AnonymousPipeClientStream(PipeDirection.In, ctrlMsg[0]))
                {
                    string msg;

                    using (StreamReader streamReader = new StreamReader(pipeClient))
                    {
                        while (true)
                        {
                            msg = streamReader.ReadLine();

                            if(msg.CompareTo("serverShutdown") == 0)
                            {
                                break;
                            }

                            Send(msg);

                            Thread.Sleep(1000);
                            // 3초 간격으로 클라이언트에게 서버가 제어중인지 상태를 보냄
                        }

                        childProcessShutdown();

                    }
                }

            }
        }


        // 스레드를 통하여 메시지를 받도록 설정


    }
}
