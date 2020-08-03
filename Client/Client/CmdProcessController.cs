using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Windows.Forms;

namespace Client
{
    class CmdProcessController
    {
        private Process hookerProcess;
        private readonly ProcessStartInfo hookerProcessStartInfo;
        
        private AnonymousPipeServerStream pipeServer;
        
        private StreamWriter streamWriter;

        public bool NowCtrlStatus { get; set; }
        
        public void CtrlStatusEventCheck(bool newCtrlStatus)
        {
            if(NowCtrlStatus != newCtrlStatus)
            {
                NowCtrlStatus = newCtrlStatus;
                this.HostControl(NowCtrlStatus);
            }
        }

        private void HostControl(bool ctrlStatus)
        {
            try
            {
                if (ctrlStatus) // 만약 프로세스가 돌고있는 상태가 아닐 때에만 새로운 프로세스가 생성이 되도록 설정
                {
                    pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);

                    hookerProcess = new Process
                    {
                        StartInfo = hookerProcessStartInfo
                    };
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
            streamWriter = new StreamWriter(pipeServer)
            {
                AutoFlush = true
            };
            streamWriter.WriteLine("quit");

            hookerProcess.WaitForExit();
        }

        public CmdProcessController()
        {
            this.NowCtrlStatus = false;

            this.hookerProcessStartInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "HookerProcess.exe"
            };
        }
    }
}
