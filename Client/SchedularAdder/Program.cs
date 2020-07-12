using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.IO;
namespace SchedularAdder
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TaskService ts = new TaskService())
            {
                ts.RootFolder.CreateFolder("MOSH Client");
                TaskDefinition td = ts.NewTask();

                td.RegistrationInfo.Description = "MOSH Client 프로그램 윈도우 시작시 자동시작";
                td.Principal.RunLevel = TaskRunLevel.Highest; // 시스템 최고 권한으로 시작

                td.Principal.UserId = string.Concat(Environment.UserDomainName, "\\", Environment.UserName); //계정
                td.Principal.LogonType = TaskLogonType.InteractiveToken; // 사용자가 로그인 됬을 때만 실행이 되도록 설정

                LogonTrigger lt = new LogonTrigger(); //로그인할때 실행되도록 trigger 설정
                td.Triggers.Add(lt);

                td.Actions.Add(new ExecAction(Directory.GetCurrentDirectory() + "\\testForm.exe")); //프로그램, 인자등록.

                ts.RootFolder.RegisterTaskDefinition("MOSH Client\\Run When Logon", td);

            }
        }
    }
}
