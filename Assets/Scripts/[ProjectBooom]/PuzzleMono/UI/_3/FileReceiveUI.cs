using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI._3
{
    /// <summary>
    ///  文件接收处UI
    /// </summary>
    public class FileReceiveUI : MonoBehaviour
    {
        [Header("拖拽时的半透明图标")] public GhostFileUI GhostFileUI;

        public RectTransform FoldRectTrans;
        public Image FoldImage;
        public TextMeshProUGUI FoldNameText;

        public virtual void ReceiveFile(GhostFileUI ghostFileUI)
        {
            // 接收文件
        }
    }
}