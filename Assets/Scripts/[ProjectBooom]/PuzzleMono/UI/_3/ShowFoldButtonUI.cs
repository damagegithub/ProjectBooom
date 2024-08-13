using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _ProjectBooom_.PuzzleMono.UI._3
{
    /// <summary>
    ///  打开文件夹按钮UI
    /// </summary>
    public class ShowFoldButtonUI : MonoBehaviour, IPointerDownHandler
    {
        public CanvasGroup FoldCanvasGroup;

        private void OnEnable()
        {
            FoldCanvasGroup.alpha = 0f;
            FoldCanvasGroup.blocksRaycasts = false;
            FoldCanvasGroup.interactable = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (DOTween.IsTweening(this))
            {
                return;
            }

            if (Mathf.Approximately(1f, FoldCanvasGroup.alpha))
            {
                return;
            }

            FoldCanvasGroup.DOFade(1f, 0.5f).SetId(this);
            FoldCanvasGroup.blocksRaycasts = true;
            FoldCanvasGroup.interactable = true;
        }
    }
}