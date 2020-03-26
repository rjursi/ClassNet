
namespace ServerCtrlCmdCenter
{
    class Program
    {

        static void Main(string[] args)
            // process pipe 값도 같이 받음
        {

            StatusBroadcastCenter statusBroadcastCenter = new StatusBroadcastCenter(args);

            statusBroadcastCenter.startListenThread();
        }
    }
}
