using _ProjectBooom_.ObservableData;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    /// <summary>
    ///     玩家控制角色接近后用于接近后触发的行为
    /// </summary>
    public class NearestAction : MonoBehaviour
    {
        /// <summary>
        ///     是否需要玩家主动按键触发
        /// </summary>
        [SerializeField]
        [Header("是否需要玩家主动按键触发")]
        protected bool IsExplicit = false;

        /// <summary>
        ///     是否只触发一次
        /// </summary>
        [SerializeField]
        [Header("是否只触发一次")]
        protected bool IsOnce = false;
        [SerializeField]
        [Header("是否已经触发过(如果触发过就不再触发)")]
        protected bool IsTriggered = false;

        /// <summary>
        ///     触发过滤层
        /// </summary>
        [SerializeField]
        [Header("可交互碰撞层")]
        protected LayerMask TriggerLayer;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            // 已经触发过一次的不再触发
            if (IsTriggered)
            {
                return;
            }

            if (IsExplicit)
            {
                RuntimeUnimportantData.EnterActionObject(this);
            }
            else
            {
                DoAction();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            // 已经触发过一次的不再触发
            if (IsTriggered)
            {
                return;
            }

            if (IsExplicit)
            {
                RuntimeUnimportantData.EnterActionObject(this);
            }
            else
            {
                DoAction();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            if (IsExplicit)
            {
                RuntimeUnimportantData.ExitActionObject(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            if (IsExplicit)
            {
                RuntimeUnimportantData.ExitActionObject(this);
            }
        }

        public virtual void DoAction()
        {
            if (IsOnce && !IsTriggered)
            {
                IsTriggered = true;
                if (IsExplicit)
                {
                    RuntimeUnimportantData.ExitActionObject(this);
                }
            }
        }
    }
}