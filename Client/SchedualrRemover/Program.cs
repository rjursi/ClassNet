using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SchedualrRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TaskService ts = new TaskService())
            {
                ts.GetFolder("ClassNet Client").DeleteTask("Run When Logon");
                ts.RootFolder.DeleteFolder("ClassNet Client");
            }


            Console.WriteLine("자동 실행 스케줄을 성공적으로 제거했습니다.");
            Thread.Sleep(1000);

            DialogResult result = MessageBox.Show("프로그램을 정상적으로 제거하기 위하여 재부팅이 필요합니다. 재부팅 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if(result == DialogResult.Yes)
            {
                Process.Start("shutdown.exe", "-r -t 0");
            }
            else
            {
                MessageBox.Show("프로그램 제거가 완료되었으나 재부팅을 권장합니다.","알림",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
    }
}
