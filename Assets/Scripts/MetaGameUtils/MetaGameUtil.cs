using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MetaGameUtils
{
    
    public class ShowDesktop
    {
        
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        const int VK_LWIN = 0x5B;  // 左Windows键的虚拟键码
        const int VK_D = 0x44;     // D键的虚拟键码
        const int KEYEVENTF_KEYUP = 0x02;  // 模拟释放按键
   
        public static void ShowDesktopFunc()
        {
            keybd_event((byte)VK_LWIN, 0, 0, UIntPtr.Zero);  // 模拟按下左Windows键
            keybd_event((byte)VK_D, 0, 0, UIntPtr.Zero);     // 模拟按下D键
            keybd_event((byte)VK_D, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);   // 释放D键
            keybd_event((byte)VK_LWIN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);  // 释放左Windows键
        }
        
    }
    public class MetaGameUtil
    {
        public static void CloseGame()
        {
            UnityEngine.Application.Quit();
        }
        
        public static void CreateFolderOnDesktop(string folderName)
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string folderPath = desktopPath + "/" + folderName;
            System.IO.Directory.CreateDirectory(folderPath);
        }
        
        public static void CreateFileOnDesktop(string folderName,string fileName, string content)
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string filePath = desktopPath+"/"+ folderName+ "/" + fileName;
            System.IO.File.WriteAllText(filePath, content);
        }
        
        public static bool CheckPlayerDesktopHasFile(string folderName,string fileName)
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);        
            string filePath = desktopPath +"/"+ folderName+ "/" + fileName;
           return System.IO.File.Exists(filePath);
        }
        
        public static void RunBatScript(string batFilePath)
        {
            //获取resources目录
            string resourcesPath = Application.dataPath + "/Resources";
            //拼接bat脚本路径
            batFilePath = resourcesPath + batFilePath;
            Debug.Log("RunBatScript: " + batFilePath);
            if (File.Exists(batFilePath))
            {
                ProcessStartInfo psi = new ProcessStartInfo(batFilePath)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = psi
                };

                process.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log(args.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                process.Close();
            }
        }
    }
}