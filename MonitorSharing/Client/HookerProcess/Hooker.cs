using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace HookerProcess
{
    class Hooker
    {


        public bool ctrlFlag;
        /*
        private struct KeyboardLowLevelHookStruct
        {
            public Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }
        */

        // Keyboard stroke info save structure

        public static List<Keys> PermittedKeys = new List<Keys>(    
        new Keys[]
        { // Alphanumeric keys.
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H,
            Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P,
            Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X,
            Keys.Y, Keys.Z,
            Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6,
            Keys.D7, Keys.D8, Keys.D9,
            Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
            Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
            Keys.Decimal,

            // Punctuation and other keys
            Keys.Enter, Keys.Space,
            Keys.Subtract, Keys.Add, Keys.Divide, Keys.Multiply,
            Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.Oemcomma, Keys.OemPeriod,
            Keys.Oemplus, Keys.OemMinus, Keys.OemQuestion, Keys.OemQuotes,
            Keys.OemSemicolon, Keys.Oemtilde, Keys.OemBackslash,

            // Editing keys
            Keys.Delete, Keys.Back,
            Keys.LShiftKey, Keys.RShiftKey, Keys.Shift, Keys.ShiftKey,

            // Navigation keys
            Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.PageUp, Keys.PageDown,
            Keys.Home, Keys.End,
        });
        

        // When Want Only Filtering Shortcuts (ex. Alt+F4)






        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        // wParam -  get handle or Int, keyboard or mouse status (ex. press)
        // lParam -  get pointer value (ex. keyboard value)

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);


        // const int WM_KEYDOWN = 0x100;
        const int WH_KEYBOARD_LL = 13;
        const int WH_MOUSE_LL = 14;

        //const int WH_MOUSE = 7;
        //const int WH_KEYBOARD = 2;

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

        private LowLevelKeyboardProc _keyboardProc = keyboardHookProc;
        private LowLevelMouseProc _mouseProc = mouseHookProc;


        // private LowLevelKeyboardProc _keyboardProc = keyboardHookProc;
        // private LowLevelMouseProc _mouseProc= mouseHookProc;
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

       

        public static IntPtr mouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
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


        public static IntPtr keyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
               
            if( nCode >= 0)
            {
                
                
                // KeyboardLowLevelHookStruct KeyInfo = (KeyboardLowLevelHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardLowLevelHookStruct));


                /*
                if (!PermittedKeys.Contains(KeyInfo.key))
                {
                    return (IntPtr)1;
                    // When Keyboard pressed, dont send anything to OS
                }
                
                */
                
                
                return (IntPtr)1;
                // When Keyboard pressed, dont send anything to OS
                

            }
            
            return CallNextHookEx(keyboardHook, nCode, (int)wParam, lParam);
            
            
           
        }


        /*
        public static IntPtr mouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
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

        */

        /*
        public static IntPtr keyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {


                // KeyboardLowLevelHookStruct KeyInfo = (KeyboardLowLevelHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardLowLevelHookStruct));


                
                if (!PermittedKeys.Contains(KeyInfo.key))
                {
                    return (IntPtr)1;
                    // When Keyboard pressed, dont send anything to OS
                }
                
                


                return (IntPtr)1;
                // When Keyboard pressed, dont send anything to OS


            }

            return CallNextHookEx(keyboardHook, nCode, (int)wParam, lParam);



        }

        */

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("user32");
            // hooking on user32.dll, so all program on gui can't processing all keyboard, mouse event

            Thread.Sleep(1000);

            
            keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, hInstance, 0);
            mouseHook = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, hInstance, 0);
            

            


            /*
            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule)
            {
                keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, GetModuleHandle(currentModule.ModuleName), 0);
                mouseHook = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, GetModuleHandle(currentModule.ModuleName),0);
            }
            */


        }


        public void UnHook()
        {
            
            
            UnhookWindowsHookEx(keyboardHook);
            UnhookWindowsHookEx(mouseHook);

            
            //UnhookWindowsHookEx(keyboardHook);
            //UnhookWindowsHookEx(mouseHook);
        }

        public Hooker()
        {
            this.ctrlFlag = false;
        }
    }
}
