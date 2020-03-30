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
    class CmdProcssController
    {
        
        private Process hookerProcess;
        private ProcessStartInfo hookerProcessStartInfo;
        
        private AnonymousPipeServerStream pipeServer;
        
        private StreamWriter streamWriter;
       
        private bool nowCtrlStatus;
        

        
        public bool NowCtrlStatus { get => nowCtrlStatus; set => nowCtrlStatus = value; }

        // 이전 상태와 다를 경우에만 프로세스가 새로 실행될 것인지를 결정
        public void CtrlStatusEventCheck(bool newCtrlStatus)
        {
            if(nowCtrlStatus != newCtrlStatus)
            {
                nowCtrlStatus = newCtrlStatus;
                this.HostControl(nowCtrlStatus);
            }
        }

        private void HostControl(bool ctrlStatus)
        {
            try
            {
                if (ctrlStatus) // 만약 프로세스가 돌고있는 상태가 아닐 때에만 새로운 프로세스가 생성이 되도록 설정
                {
                    pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);

                    hookerProcess = new Process();
                    hookerProcess.StartInfo = hookerProcessStartInfo;
                    hookerProcess.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
                    hookerProcess.Start();
       
                }
                              
                if (!ctrlStatus) // 만약 프로세스가 돌고있는 상태에서만 아래 코드가 동작을 하도록 설정
                {
                    pipeServer.DisposeLocalCopyOfClientHandle();
                    QuitProcess();
                                       
                }              
            }
                    
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Exception : {0}", e.Message));
            }
                     
        }
   
        public void QuitProcess()
        {

            streamWriter = new StreamWriter(pipeServer);
            streamWriter.AutoFlush = true;
            streamWriter.WriteLine("quit");


            hookerProcess.WaitForExit(); // here
        }

        public CmdProcssController()
        {
            this.NowCtrlStatus = false;

            this.hookerProcessStartInfo = new ProcessStartInfo();
            this.hookerProcessStartInfo.CreateNoWindow = false;
            this.hookerProcessStartInfo.UseShellExecute = false;
            this.hookerProcessStartInfo.FileName = "HookerProcess.exe";

        }
    }
}
