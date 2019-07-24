using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace irc_connector.DataSourceWrappers
{
    public class WinAppWrapper
    {
        #region delegates

        public delegate bool CallBackPtr(int hwnd, int lParam);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        #endregion

        #region dll imports

        // used to navigate top level windows
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        // used to get window caption/title
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // help to get window caption/title
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        // used to find window from cursor
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(System.Drawing.Point p);

        #endregion

        #region custom wrappers

        public static string GetText(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public static bool GetWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            string wCaption = GetText(hWnd);
            if (!string.IsNullOrEmpty(wCaption))
                Console.WriteLine($"{hWnd} : {wCaption}");
            return true;
        }

        public static void GetWindows()
        {
            EnumWindowsProc callback = new EnumWindowsProc(GetWindowsCallback);
            EnumWindows(callback, IntPtr.Zero);
        }

        #endregion


    }
}
