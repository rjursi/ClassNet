using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    class ClientListener
    {
        const int MOSH_CMDSERVICE_PORT = 9991;

        private UdpClient udp = null;
        private Process hookerProcess;
        private ProcessStartInfo hookerProcessStartInfo;
        //private form_keyMouseControlling ctrlForm;
        private AnonymousPipeServerStream pipeServer;
        public bool ctrlStatus;
        private StreamWriter streamWriter;
        
        public void Start()
        {
            if(udp != null)
            {
                throw new Exception("Already started, stop first plz...");
            }
            Receive();
        }

        private void Receive()
        {
            udp = new UdpClient();

            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, MOSH_CMDSERVICE_PORT);

            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udp.ExclusiveAddressUse = false;
            // use one port many client

            udp.Client.Bind(localEp);

            var from = new IPEndPoint(0, 0);

            while (true)
            {
                //Console.WriteLine("Waiting...");
                byte[] recvBuffer = udp.Receive(ref from);
                


                try
                {
                    if (udp != null)
                    {
                        string message = Encoding.UTF8.GetString(recvBuffer);
                        

                        if (message.Equals("ctrlstart"))
                        {

                            if (!ctrlStatus) // 만약 프로세스가 돌고있는 상태가 아닐 때에만 새로운 프로세스가 생성이 되도록 설정


                            {

                                pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);

                                hookerProcess = new Process();
                                hookerProcess.StartInfo = hookerProcessStartInfo;
                                hookerProcess.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
                                hookerProcess.Start();

                                this.ctrlStatus = true;
                                // Console.WriteLine("Hooking Status : Hooking....");
                                
                            }
                            
                        }
                        else if (message.Equals("ctrlstop"))
                        {
                            if (ctrlStatus) // 만약 프로세스가 돌고있는 상태에서만 아래 코드가 동작을 하도록 설정
                            {
                                pipeServer.DisposeLocalCopyOfClientHandle();


                                QuitProcess();
                                
                                this.ctrlStatus = false;

                               
                            }
                        }
                        else if (message.Equals("server shutdown"))
                        {
                            if (ctrlStatus) { QuitProcess(); }

                            

                            this.Stop();
                            break;
                        }
                    }
                }
                catch (SocketException se)
                {
                    MessageBox.Show(string.Format("SocketException : {0}", se.Message));
                }
                /*
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("Exception : {0}", e.Message));
                }
                */
                
            }
        }
        

        public void QuitProcess()
        {

            streamWriter = new StreamWriter(pipeServer);
            streamWriter.AutoFlush = true;
            streamWriter.WriteLine("quit");
            

            hookerProcess.WaitForExit(); // here
        }


        public void Stop()
        {

           
            try
            {
                if(udp != null)
                {
                    udp.Close();
                    udp = null;
                }
            }
            catch { }
        }

        public ClientListener()
        {
            this.ctrlStatus = false;


            this.hookerProcessStartInfo = new ProcessStartInfo();
            this.hookerProcessStartInfo.CreateNoWindow = false;
            this.hookerProcessStartInfo.UseShellExecute = false;
            this.hookerProcessStartInfo.FileName = "HookerProcess.exe";


            
            


        }
    }
}
