using DG.Tweening;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    /// 走道
    /// </summary>
    public class _10_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        /// <summary>
        /// 开场动画
        ///  </summary>
        public void StartInitAnimation()
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
                .AppendInterval(1f)
                .OnComplete(() => { StoryController.TryFinishCurrentStory(); })
                .SetId(this);
        }
    }
}