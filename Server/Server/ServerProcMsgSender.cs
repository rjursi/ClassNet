using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

using System.Threading;

namespace Server
{
    class ServerProcMsgSender
    {

        private Process commanderProcess;
        private ProcessStartInfo commanderProcessStartInfo;
        
        private AnonymousPipeServerStream pipeServer;

        public StreamWriter streamWriter;
        public bool ctrlStatus;

       

        public void ctrlStart()
        {
            do
            {
                streamWriter.WriteLine("ctrlstart");
                Thread.Sleep(1000);
            } while (this.ctrlStatus);
            
        }
        public void ctrlStop()
        {
            
            do
            {
                streamWriter.WriteLine("ctrlstop");
                Thread.Sleep(1000);
            } while (!this.ctrlStatus);
            
        }


        public void serverShutdown()
        {
            streamWriter.WriteLine("serverShutdown");
            commanderProcess.WaitForExit();
        }

        public void executeCommanderProcess()
        {
            commanderProcess = new Process();
            commanderProcess.StartInfo = commanderProcessStartInfo;

            pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
           
            commanderProcess.StartInfo.Arguments = pipeServer.GetClientHandleAsString();

            // pipeServer.DisposeLocalCopyOfClientHandle();
            commanderProcess.Start();

            this.streamWriter = new StreamWriter(pipeServer);
            this.streamWriter.AutoFlush = true;

           
        }
        public ServerProcMsgSender()
        {

            this.ctrlStatus = false;
            
            this.commanderProcessStartInfo = new ProcessStartInfo();
            this.commanderProcessStartInfo.CreateNoWindow = true;
            this.commanderProcessStartInfo.UseShellExecute = false;
            this.commanderProcessStartInfo.FileName = "ServerCtrlCmdCenter.exe";

            

        }

    }
}
