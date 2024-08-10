using System.Collections;
using _ProjectBooom_.PuzzleMono.UI._3;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    /// 走道会议室
    /// </summary>
    public class _3_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        [Header("全屏遮挡画布")] public CanvasGroup BlackCanvasGroup;

        [Header("博士对话框")] public CanvasGroup DoctorCanvasGroup;

        [Header("博士对话文字")] public TextMeshProUGUI TMP_DoctorText;

        [Header("文件接收处")] public FoldFileReceiveUI FoldFileReceiveUI;

        [Header("简陋电脑UI")] public RectTransform DesktopRectTrans;

        private void Awake()
        {
            BlackCanvasGroup.alpha = 1;
            DesktopRectTrans.localScale = Vector3.zero;
        }

        /// <summary>
        /// 开场动画
        ///  </summary>
        public void StartInitAnimation()
        {
            DOTween.Sequence()
                .Append(BlackCanvasGroup.DOFade(0f, 1f))
                .OnComplete(() => { StoryController.TryFinishCurrentStory(); })
                .SetId(this);
        }

        public void StartStory()
        {
            StartCoroutine(StartStoryCoroutine());
        }

        public IEnumerator DocSpeak(string text, float time)
        {
            TMP_DoctorText.maxVisibleCharacters = 0;
            TMP_DoctorText.text = text;
            DOTween.Sequence()
                .Append(DoctorCanvasGroup.DOFade(1f, 0.5f))
                .Append(DOTween.To(
                    () => TMP_DoctorText.maxVisibleCharacters,
                    x => TMP_DoctorText.maxVisibleCharacters = x,
                    text.Length,
                    time
                ))
                .Append(DoctorCanvasGroup.DOFade(0f, 0.5f))
                .SetId(this);

            // 等待动画结束
            while (DOTween.IsTweening(this))
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator StartStoryCoroutine()
        {
            yield return new WaitForEndOfFrame();
            const string doctorText1 = "博士：接下来需要为会议做准备，我需要你的帮助。";
            yield return DocSpeak(doctorText1, 4f);
            const string doctorText2 = "博士：请帮我准备好会议所需的文件";
            yield return DocSpeak(doctorText2, 4f);

            // 打开电脑
            DesktopRectTrans.DOScale(Vector3.one, 1f).SetId(this);
            const string doctorText3 = "博士：文件应该都已经在桌面上了 请查看一下";
            yield return DocSpeak(doctorText3, 4f);

            const string doctorText4 = "博士：请将文件拖拽到会议文件夹中";
            float waitTime = 5f;
            // 等待完成
            while (FoldFileReceiveUI.FileUIs.Count < 3)
            {
                yield return new WaitForEndOfFrame();
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    waitTime = 5f;
                    yield return DocSpeak(doctorText4, 4f);
                }
            }

            DesktopRectTrans.DOScale(Vector3.zero, 1f).SetId(this);
            const string doctorText5 = "博士：非常感谢你的帮助";
            yield return DocSpeak(doctorText5, 4f);

            while (DOTween.IsTweening(this))
            {
                yield return new WaitForEndOfFrame();
            }

            StoryController.TryFinishCurrentStory();
        }

        /// <summary>
        ///  结束动画
        /// </summary>
        public void FinishAnimation()
        {
            DOTween.Sequence()
                .Append(BlackCanvasGroup.DOFade(1f, 1f))
                .OnComplete(() => { StoryController.TryFinishCurrentStory(); })
                .SetId(this);
        }
    }
}