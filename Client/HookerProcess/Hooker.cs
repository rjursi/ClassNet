using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HookerProcess
{
    class Hooker
    {
        public bool ctrlFlag;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        // wParam -  get handle or Int, keyboard or mouse status (ex. press)
        // lParam -  get pointer value (ex. keyboard value)

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WH_MOUSE_LL = 14;

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONDBLCLICK = 0x0203;
        const int WM_LBUTTONUP = 0x0202;

        const int WM_MBUTTONDBLCLICK = 0x0209;
        const int WM_MBUTTONDOWN = 0x0207;
        const int WM_MBUTTONUP = 0x0208;

        const int WM_RBUTTONDBLCLICK = 0x0206;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_RBUTTONUP = 0x0205;

        const int WM_MOUSEWHEEL = 0x020A;
        
        private static IntPtr keyboardHook = IntPtr.Zero;
        private static IntPtr mouseHook = IntPtr.Zero;

        private readonly LowLevelKeyboardProc _keyboardProc = KeyboardHookProc;
        private readonly LowLevelMouseProc _mouseProc = MouseHookProc;

        // Dll Load for hooking

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        
        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        public static IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            switch ((int)wParam)
            {
                case WM_LBUTTONDOWN:
                case WM_LBUTTONDBLCLICK:
                case WM_LBUTTONUP:

                case WM_MBUTTONDBLCLICK:
                case WM_MBUTTONDOWN:
                case WM_MBUTTONUP:

                case WM_RBUTTONDBLCLICK:
                case WM_RBUTTONDOWN:
                case WM_RBUTTONUP:
                case WM_MOUSEWHEEL:
                    return (IntPtr)1;
            }

            return CallNextHookEx(mouseHook, nCode, (int)wParam, lParam);
        }

        public static IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0)
            {
                return (IntPtr)1;
            }
            
            return CallNextHookEx(keyboardHook, nCode, (int)wParam, lParam);
        }

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("user32");
            // hooking on user32.dll, so all program on gui can't processing all keyboard, mouse event

            Thread.Sleep(1000);

            keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, hInstance, 0);
            mouseHook = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, hInstance, 0);
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(keyboardHook);
            UnhookWindowsHookEx(mouseHook);
        }

        public Hooker()
        {
            this.ctrlFlag = false;
        }
    }
}
