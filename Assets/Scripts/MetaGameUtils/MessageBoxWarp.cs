using System;
using System.Runtime.InteropServices;

namespace MetaGameUtils
{
    public static class MessageBoxWarp
    {
        [Flags]
        public enum MessageBoxType
        {
            OK = 0x00000000,
            OKCancel = 0x00000001,
            AbortRetryIgnore = 0x00000002,
            YesNoCancel = 0x00000003,
            YesNo = 0x00000004,
            RetryCancel = 0x00000005,
            CancelTryContinue = 0x00000006,
        }

        [Flags]
        public enum MessageBoxResult
        {
            OK = 1,
            Cancel = 2,
            Abort = 3,
            Retry = 4,
            Ignore = 5,
            Yes = 6,
            No = 7,
        }

        [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr handle, string message, string title, int type);
    }
}