using _ProjectBooom_.DataStruct;
using _ProjectBooom_.PuzzleMono.UI._2;
using Controllers;
using DG.Tweening;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    /// 走道
    /// </summary>
    public class _2_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        public CaptchaControl CaptchaControl;

        [Header("全屏遮挡画布")] public CanvasGroup BlackCanvasGroup;


        [SerializeField] [Header("验证码信息")] public CaptchaInfo[] CaptchaInfos;

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

        /// <summary>
        ///  开始验证码测试
        /// </summary>
        public void StartCapchaTest()
        {
            DOTween.Sequence()
                .AppendInterval(1f)
                .OnComplete(() => { StoryController.TryFinishCurrentStory(); })
                .SetId(this);
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