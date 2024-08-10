using System.Collections.Generic;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.UI._3
{
    /// <summary>
    ///  文件夹接收处UI
    /// </summary>
    public class FoldFileReceiveUI : FileReceiveUI
    {
        public List<FileUI> FileUIs = new List<FileUI>();

        public override void ReceiveFile(GhostFileUI ghostFileUI)
        {
            FileUIs.Add(ghostFileUI.SourceFileUI);
            ghostFileUI.SourceFileUI.gameObject.SetActive(false);
        }
    }
}