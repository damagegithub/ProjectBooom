using System.Collections;
using _ProjectBooom_.DataStruct;
using _ProjectBooom_.PuzzleMono.UI;
using _ProjectBooom_.PuzzleMono.UI._2;
using DG.Tweening;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    ///     走道
    /// </summary>
    public class _2_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        public CaptchaControl CaptchaControl;

        [Header("全屏遮挡画布")] public CanvasGroup BlackCanvasGroup;

        [Header("博士对话脚本")] public DoctorSpeakController DoctorSpeakController;

        [SerializeField] [Header("验证码信息")] public CaptchaInfo[] CaptchaInfos;

        private void Awake()
        {
            if (!DoctorSpeakController)
            {
                DoctorSpeakController = FindObjectOfType<DoctorSpeakController>(true);
            }

            BlackCanvasGroup.alpha = 1;
        }

        /// <summary>
        ///     开场动画
        /// </summary>
        public void StartInitAnimation()
        {
            DOTween.Sequence()
                   .Append(BlackCanvasGroup.DOFade(0f, 1f))
                   .OnComplete(() => { StoryController.TryFinishCurrentStory(); })
                   .SetId(this);
        }

        /// <summary>
        ///     开始验证码测试
        /// </summary>
        public void StartCaptchaTest()
        {
            StartCoroutine(StartCaptchaTestCoroutine());
        }

        public IEnumerator StartCaptchaTestCoroutine()
        {
            const string doctorText = "博士：你好，我是博士，你需要通过这些验证码来证明你的身份。";
            yield return DoctorSpeakController.SpeakAndWait(doctorText);
            for (int i = 0; i < CaptchaInfos.Length; i++)
            {
                StoryController.SetDebugText($"当前第{i}个验证码");
                CaptchaControl.Show(CaptchaInfos[i]);
                while (!CaptchaControl.IsAnswered)
                {
                    yield return new WaitForEndOfFrame();
                }

                // DO: 插入对话
                string doctorText2 = CaptchaControl.IsCorrect ? "博士：似乎不错" : "博士：有点不太对劲";
                yield return DoctorSpeakController.SpeakAndWait(doctorText2);
            }

            // DO: 插入对话
            const string doctorText3 = "博士：先去会议室吧";
            yield return DoctorSpeakController.SpeakAndWait(doctorText3);
            StoryController.TryFinishCurrentStory();
        }

        /// <summary>
        ///     结束动画
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