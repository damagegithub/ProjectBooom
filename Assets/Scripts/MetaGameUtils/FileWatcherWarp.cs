using System.IO;

namespace MetaGameUtils
{
    public static class FileWatcherWarp
    {
        /// <summary>
        ///     创建文件系统监视器 用于触发文件变动的事件
        /// </summary>
        public static FileSystemWatcher CreateFileSystemWatcher(string path)
        {
            FileSystemWatcher watcher = new(path)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            return watcher;
        }
    }
}