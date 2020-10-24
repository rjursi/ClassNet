using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchedularRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TaskService ts = new TaskService())
            {
                ts.GetFolder("ClassNet Server").DeleteTask("Run When Logon");
                ts.RootFolder.DeleteFolder("ClassNet Server");
            }


            Console.WriteLine("자동 실행 스케줄을 성공적으로 제거했습니다.");
           
        }
    }
}
