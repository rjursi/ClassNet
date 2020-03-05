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
        const int PORT = 9990;

        private UdpClient udp = null;
        private Process hookerProcess;
        private ProcessStartInfo hookerProcessStartInfo;
        //private form_keyMouseControlling ctrlForm;
        private AnonymousPipeServerStream pipeServer;
        
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

            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, PORT);

            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udp.ExclusiveAddressUse = false;
            // use one port many client

            udp.Client.Bind(localEp);

            var from = new IPEndPoint(0, 0);

            while (true)
            {
                Console.WriteLine("Waiting...");
                byte[] recvBuffer = udp.Receive(ref from);

                try
                {
                    if (udp != null)
                    {
                        string message = Encoding.UTF8.GetString(recvBuffer);

                        byte[] returnMsgBytes;
                        string returnMsg;

                        if (message.Equals("control start"))
                        {
                            hookerProcess = new Process();
                            hookerProcess.StartInfo = hookerProcessStartInfo;

                            pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
                            
                            hookerProcess.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
                            hookerProcess.Start();

                            Console.WriteLine("Hooking Status : Hooking....");
                            returnMsg = "controlling";
                            returnMsgBytes = Encoding.Unicode.GetBytes(returnMsg);

                            udp.Send(returnMsgBytes, returnMsgBytes.Length, from);
                        }
                        else if (message.Equals("control stop"))
                        {
                            pipeServer.DisposeLocalCopyOfClientHandle();

                            try
                            {
                                using (StreamWriter streamWriter = new StreamWriter(pipeServer))
                                {
                                    streamWriter.AutoFlush = true;
                                    streamWriter.WriteLine("quit");
                                }

                            }catch(IOException e)
                            {
                                MessageBox.Show(e.Message);
                            }

                            //hookerProcess.Kill();
                            hookerProcess.WaitForExit();
                            
                            Console.WriteLine("Hooking Status : No Hooking");
                            returnMsg = "control stopped";
                            returnMsgBytes = Encoding.Unicode.GetBytes(returnMsg);

                            udp.Send(returnMsgBytes, returnMsgBytes.Length, from);
                        }
                        else if (message.Equals("server shutdown"))
                        {
                            this.Stop();
                            break;
                        }
                    }
                }
                catch (SocketException se)
                {
                    MessageBox.Show(string.Format("SocketException : {0}", se.Message));
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("Exception : {0}", e.Message));
                }

            }
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
            //this.hooker = new Hooker();
            this.hookerProcessStartInfo = new ProcessStartInfo();
            this.hookerProcessStartInfo.CreateNoWindow = false;
            this.hookerProcessStartInfo.UseShellExecute = false;
            this.hookerProcessStartInfo.FileName = "HookerProcess.exe";
        }
    }
}
