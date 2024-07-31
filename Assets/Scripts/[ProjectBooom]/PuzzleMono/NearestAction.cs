using System;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono
{
    /// <summary>
    ///     接近后用于接近后触发的行为
    /// </summary>
    public class NearestAction : MonoBehaviour
    {
        /// <summary>
        ///     触发过滤层
        /// </summary>
        [SerializeField] protected LayerMask TriggerLayer;

        /// <summary>
        ///     当前是否已触发
        /// </summary>
        public bool IsTriggered;

        /// <summary>
        ///     触发状态改变时
        /// </summary>
        public Action<bool> OnTriggerChangedAction;

        /// <summary>
        ///     触发状态改变时
        /// </summary>
        protected virtual void OnTriggerChanged() { }

        private void OnTriggerEnter(Collider other)
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

        private void OnTriggerExit(Collider other)
        {
            if (!IsTriggered)
            {
                return;
            }

            if (TriggerLayer != (TriggerLayer | (1 << other.gameObject.layer)))
            {
                return;
            }

            IsTriggered = false;
            OnTriggerChanged();
            OnTriggerChangedAction?.Invoke(IsTriggered);
        }
    }
}