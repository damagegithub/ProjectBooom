using DG.Tweening;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    /// 培养室
    /// </summary>
    public class _7_Scene : MonoBehaviour
    {
        public StoryController StoryController;


        [Header("全屏遮挡画布")] public CanvasGroup BlackCanvasGroup;

        private void Awake()
        {
            BlackCanvasGroup.alpha = 1;
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