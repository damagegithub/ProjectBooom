using TMPro;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.UI._2
{
    /// <summary>
    ///  验证码UI
    /// </summary>
    public class CaptchaControl : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI ContentText;
        public Transform AnswerRoot;
        public GameObject AnswerPrefab;

        public void Show()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
        }

        public void Hide()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
        }
    }
}