using System.Collections;
using _ProjectBooom_.PuzzleMono.UI;
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

        [Header("博士对话脚本")] public DoctorSpeakController DoctorSpeakController;

        [Header("文件接收处")] public FoldFileReceiveUI FoldFileReceiveUI;

        [Header("简陋电脑UI")] public RectTransform DesktopRectTrans;

        private void Awake()
        {
            if (!DoctorSpeakController)
            {
                DoctorSpeakController = FindObjectOfType<DoctorSpeakController>(true);
            }

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

        public IEnumerator StartStoryCoroutine()
        {
            yield return new WaitForEndOfFrame();
            const string doctorText1 = "博士：接下来需要为会议做准备，我需要你的帮助。";
            yield return DoctorSpeakController.SpeakAndWait(doctorText1, true);
            const string doctorText2 = "博士：请帮我准备好会议所需的文件";
            yield return DoctorSpeakController.SpeakAndWait(doctorText2, true);

            // 打开电脑
            DesktopRectTrans.DOScale(Vector3.one, 1f).SetId(this);
            const string doctorText3 = "博士：文件应该都已经在桌面上了 请查看一下";
            yield return DoctorSpeakController.SpeakAndWait(doctorText3, true);

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
                    DoctorSpeakController.Speak(doctorText4, true);
                }
            }

            DesktopRectTrans.DOScale(Vector3.zero, 1f).SetId(this);
            const string doctorText5 = "博士：非常感谢你的帮助";
            yield return DoctorSpeakController.SpeakAndWait(doctorText5, true);

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