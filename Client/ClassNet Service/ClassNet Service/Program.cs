using System;
using System.Diagnostics;
using System.Threading;

namespace ClassNet_Service
{
    class Program
    {
        private static Process[] processList;
        private static string filename;

        static void Main(string[] args)
        {
            filename = AppDomain.CurrentDomain.BaseDirectory + "ClassNet Client.exe";
            while (true)
            {
                processList = Process.GetProcessesByName("ClassNet Client");

                if (processList.Length < 1)
                {
                    Process.Start(filename);
                    processList = Process.GetProcessesByName("ClassNet Client");
                }
                Thread.Sleep(1000);
            }
        }
    }
}
