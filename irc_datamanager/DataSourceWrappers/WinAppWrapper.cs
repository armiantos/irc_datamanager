using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace irc_datamanager.DataSourceWrappers
{
    public class WinAppWrapper
    {


        public delegate bool CallBackPtr(int hwnd, int lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static bool GetWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            Console.WriteLine(hWnd);
            return true;
        }

        public static void GetWindows()
        {
            EnumWindowsProc callback = new EnumWindowsProc(GetWindowsCallback);
            EnumWindows(callback, IntPtr.Zero);
        }
    }
}
