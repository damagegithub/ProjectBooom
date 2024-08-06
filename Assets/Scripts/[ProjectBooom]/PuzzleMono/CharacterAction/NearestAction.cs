using System;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    /// <summary>
    ///     玩家控制角色接近后用于接近后触发的行为
    /// </summary>
    public class NearestAction : MonoBehaviour
    {
        /// <summary>
        ///     触发过滤层
        /// </summary>
        [SerializeField]
        [Header("可交互碰撞层")]
        protected LayerMask TriggerLayer;

        /// <summary>
        ///     当前是否可触发
        /// </summary>
        [Header("是否可交互")]
        public bool IsTriggered;

        /// <summary>
        ///     触发状态改变时
        /// </summary>
        public Action<bool> OnTriggerChangedAction;

        /// <summary>
        ///     触发状态改变时
        /// </summary>
        protected virtual void OnTriggerChanged() { }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsTriggered)
            {
                return;
            }

            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            IsTriggered = true;
            OnTriggerChanged();
            OnTriggerChangedAction?.Invoke(IsTriggered);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsTriggered)
            {
                return;
            }

            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            IsTriggered = false;
            OnTriggerChanged();
            OnTriggerChangedAction?.Invoke(IsTriggered);
        }
    }
}