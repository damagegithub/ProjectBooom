using System.IO;

namespace MetaGameUtils
{
    public static class FileWatcherWarp
    {
        /// <summary>
        ///     创建文件系统监视器 用于触发文件变动的事件
        /// </summary>
        public static FileSystemWatcher CreateFileSystemWatcher(string Path)
        {
            FileSystemWatcher Watcher = new(Path)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            return Watcher;
        }
    }
}