using _ProjectBooom_.DataStruct;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI._2
{
    /// <summary>
    ///     验证码UI
    /// </summary>
    public class CaptchaControl : MonoBehaviour
    {
        public CanvasGroup     CanvasGroup;
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI ContentText;
        public Transform       AnswerRoot;
        public GameObject      AnswerPrefab;

        public UnityEvent OnAnswerClick;

        public bool IsAnswered { get; private set; }
        public bool IsCorrect  { get; private set; }

        private CaptchaInfo CaptchaInfo;

        public void Show(CaptchaInfo captchaInfo)
        {
            CaptchaInfo = captchaInfo;
            TitleText.text = captchaInfo.Title;
            ContentText.text = captchaInfo.Content;

            // 暂时不考虑性能开销
            for (int i = AnswerRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(AnswerRoot.GetChild(i).gameObject);
            }

            for (int i = 0; i < captchaInfo.Answers.Length; i++)
            {
                string answer = captchaInfo.Answers[i];
                GameObject answerGo = Instantiate(AnswerPrefab, AnswerRoot);
                answerGo.GetComponentInChildren<TextMeshProUGUI>().text = answer;
                answerGo.GetComponentInChildren<Button>().onClick.AddListener(() => OnAnyAnswerClick(answer));
            }

            IsCorrect = false;
            IsAnswered = false;
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
        }

        public void OnAnyAnswerClick(string answer)
        {
            IsCorrect = string.Equals(answer, CaptchaInfo.CorrectAnswer);
            CaptchaInfo = null;
            IsAnswered = true;
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            OnAnswerClick?.Invoke();
        }
    }
}