using System;
using System.Runtime.InteropServices;

namespace MetaGameUtils
{
    public static class SetWindowOnTopWarp
    {
        #region WIN32API

        public static readonly IntPtr HWND_TOPMOST = new(-1);
        public static readonly IntPtr HWND_NOT_TOPMOST = new(-2);
        public const uint SWP_SHOWWINDOW = 0x0040;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int X
            {
                get => Left;
                set
                {
                    Right -= Left - value;
                    Left = value;
                }
            }

            public int Y
            {
                get => Top;
                set
                {
                    Bottom -= Top - value;
                    Top = value;
                }
            }

            public int Height
            {
                get => Bottom - Top;
                set => Bottom = value + Top;
            }

            public int Width
            {
                get => Right - Left;
                set => Right = value + Left;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint uFlags
        );

        #endregion


        /// <summary>
        ///     设置窗口置顶并获取焦点
        /// </summary>
        public static void SetWindowOnTop(string windowName, bool makeTopmost = true)
        {
            IntPtr hWnd = FindWindow(null, windowName);
            GetWindowRect(new HandleRef(null, hWnd), out RECT rect);
            IntPtr hWndInsertAfter = makeTopmost ? HWND_TOPMOST : HWND_NOT_TOPMOST;
            SetWindowPos(hWnd, hWndInsertAfter, rect.X, rect.Y, rect.Width, rect.Height, SWP_SHOWWINDOW);
        }
    }
}