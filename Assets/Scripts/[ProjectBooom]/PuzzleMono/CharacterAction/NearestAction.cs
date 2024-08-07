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
        ///     触发过滤层
        /// </summary>
        [SerializeField]
        [Header("可交互碰撞层")]
        protected LayerMask TriggerLayer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            RuntimeUnimportantData.EnterActionObject(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (0 == (TriggerLayer & (1 << other.gameObject.layer)))
            {
                return;
            }

            RuntimeUnimportantData.ExitActionObject(this);
        }

        public virtual void DoAction()
        {
            // ignore
        }
    }
}