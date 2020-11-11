using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

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
                    ts.RootFolder.CreateFolder("ClassNet Client");
                    TaskDefinition td = ts.NewTask();

                    td.RegistrationInfo.Description = "ClassNet Client 프로그램 윈도우 시작시 자동시작";
                    td.Principal.RunLevel = TaskRunLevel.Highest; // 시스템 최고 권한으로 시작

                    td.Principal.UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                   
                    td.Principal.LogonType = TaskLogonType.InteractiveToken; // 사용자가 로그인 됬을 때만 실행이 되도록 설정
                    td.Settings.DisallowStartIfOnBatteries = false;
                    LogonTrigger lt = new LogonTrigger(); // 로그인할때 실 행되도록 trigger 설정
                    td.Triggers.Add(lt);

                    td.Actions.Add(new ExecAction(currentExePath + "\\ClassNet Client.exe")); // 프로그램, 인자등록.
                    
                    ts.RootFolder.RegisterTaskDefinition("ClassNet Client\\Run When Logon", td);
                    Console.WriteLine("Schedular Adder : 작업 등록이 성공적으로 완료되었습니다...");
                    Thread.Sleep(1000);

                    DialogResult result = MessageBox.Show("설치를 정상적으로 완료할려면 재부팅이 필요합니다. 지금 재부팅 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if(result == DialogResult.Yes)
                    {
                        Process.Start("Shutdown.exe", "-r -t 0");
                    }
                    else
                    {
                        MessageBox.Show("설치가 완료되었습니다. 재부팅을 권장드립니다.","알림",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                }
                catch(Exception e) { 
                    MessageBox.Show($"{e.Message} : {e.StackTrace}");
                }
            }
        }
    }
}
