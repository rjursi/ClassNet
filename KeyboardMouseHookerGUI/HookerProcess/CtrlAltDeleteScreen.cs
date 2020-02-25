using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace HookerProcess
{
    class CtrlAltDeleteScreen
    {

        private static readonly WinEventDelegate callback = EventCallback;
        const uint WINEVENT_OUTOFCONTEXT = 0x0000;
        const uint WINEVENT_SKIPOWNTHREAD = 0x0001;
        const uint EVENT_SYSTEM_DESKTOPSWITCH = 0x0020;
        static int desktopSwitchCnt = 0;

        static Hooker hookerDelegater;

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,    
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread,
            uint dwmsEventTime);



        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);


        public void StartListeningForDesktopSwitch(Hooker hooker)
        {

            hookerDelegater = hooker;
            
            SetWinEventHook(EVENT_SYSTEM_DESKTOPSWITCH, EVENT_SYSTEM_DESKTOPSWITCH,
                IntPtr.Zero, callback, 0, 0, WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNTHREAD);

        }

        public static void EventCallback(IntPtr hWinEventHook, uint eventType,
               IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            desktopSwitchCnt++;
            
            if(desktopSwitchCnt == 2)
            {
                Thread.Sleep(1000);
                //hookerDelegater.UnHook();
                
                SendKeys.Send("{Enter}");
                //Thread.Sleep(1000);
                //hookerDelegater.SetHook();
                desktopSwitchCnt = 0;
            }
            

            

        }
    }
}
