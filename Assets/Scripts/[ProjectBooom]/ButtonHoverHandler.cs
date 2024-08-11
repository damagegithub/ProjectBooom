using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject SelectedSign; // 选中标记
    // 当鼠标进入按钮区域时调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectedSign?.SetActive(true); 
    }

    // 当鼠标离开按钮区域时调用
    public void OnPointerExit(PointerEventData eventData)
    {
        SelectedSign?.SetActive(false); 
    }
    
}
