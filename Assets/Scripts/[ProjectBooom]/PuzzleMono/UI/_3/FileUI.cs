using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI._3
{
    /// <summary>
    ///  文件可拖拽UI
    /// </summary>
    public class FileUI : MonoBehaviour, IPointerDownHandler
    {
        [Header("拖拽时的半透明图标")] public GhostFileUI GhostFileUI;

        public RectTransform FileRectTrans;
        public Image FileImage;
        public TextMeshProUGUI FileNameText;

        public void OnPointerDown(PointerEventData eventData)
        {
            GhostFileUI.StartDrag(this);
        }
    }
}