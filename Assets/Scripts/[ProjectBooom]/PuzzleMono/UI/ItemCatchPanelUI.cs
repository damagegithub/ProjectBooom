using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _ProjectBooom_.PuzzleMono.UI
{
    /// <summary>
    ///     实现拖放物体到物品栏功能的面板
    /// </summary>
    public class ItemCatchPanelUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("是否正拿着物品并在捕获面板上")]
        public bool IsHovered;

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}