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

namespace SchedularAdder
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TaskService ts = new TaskService())
            {
                string currentExePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                try
                {
                    ts.RootFolder.CreateFolder("ClassNet Server");
                    TaskDefinition td = ts.NewTask();

                    td.RegistrationInfo.Description = "ClassNet Server 프로그램 윈도우 시작시 자동시작";
                    td.Principal.RunLevel = TaskRunLevel.Highest; // 시스템 최고 권한으로 시작


                    td.Principal.UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                    td.Principal.LogonType = TaskLogonType.InteractiveToken; // 사용자가 로그인 됬을 때만 실행이 되도록 설정
                    td.Settings.DisallowStartIfOnBatteries = false;
                    LogonTrigger lt = new LogonTrigger(); //로그인할때 실 행되도록 trigger 설정
                    td.Triggers.Add(lt);



                    td.Actions.Add(new ExecAction(currentExePath + "\\Server.exe")); //프로그램, 인자등록.


                    ts.RootFolder.RegisterTaskDefinition("ClassNet Server\\Run When Logon", td);
                    Console.WriteLine("Schedular Adder : 작업 등록이 성공적으로 완료되었습니다...");
                    Thread.Sleep(1500);
                }
                catch (Exception e)
                {

                    MessageBox.Show($"{e.Message} : {e.StackTrace}");   
                }
            }
        }
    }
}
