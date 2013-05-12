using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;

    public delegate void GlobalKeyDownEvent(int keyCode);

    public class InterceptKeyboard
    {
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        
        public IntPtr _hookID = IntPtr.Zero;
        public event GlobalKeyDownEvent KeyEvent;

        public LowLevelKeyboardProc hook;

        public InterceptKeyboard()
        {
            hook = HookCallback;
        }
        
        public static IntPtr SetHook(LowLevelKeyboardProc process)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, process,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        public IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (KeyEvent != null)
                {
                    //KeyEvent.BeginInvoke(vkCode, null, null);
                    KeyEvent.Invoke(vkCode);
                }
                //Console.WriteLine((Keys)vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
