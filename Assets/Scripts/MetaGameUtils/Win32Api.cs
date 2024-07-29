using System;

namespace MetaGameUtils
{
    public class Win32Api
    {
        /// <summary>
        ///     设置窗口置顶并获取焦点
        /// </summary>
        public static void SetWindowOnTop(string windowName = @"需要置顶的窗口标题")
            => SetWindowOnTopWarp.SetWindowOnTop(windowName);

        /// <summary>
        ///     创建文件系统监视器 用于触发文件变动的事件
        /// </summary>
        public static void CreateFileSystemWatcher(string path)
            => FileWatcherWarp.CreateFileSystemWatcher(path);

        /// <summary>
        ///     显示消息框
        /// </summary>
        public static MessageBoxWarp.MessageBoxResult ShowMessageBox(
            string message,
            string title,
            MessageBoxWarp.MessageBoxType type
        ) => (MessageBoxWarp.MessageBoxResult)MessageBoxWarp.MessageBox(
            IntPtr.Zero,
            message,
            title,
            (int)type
        );
    }
}