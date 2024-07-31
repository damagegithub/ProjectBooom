using _ProjectBooom_.DataStruct;
using _ProjectBooom_.Input;
using _ProjectBooom_.PuzzleMono.UI;
using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    /// <summary>
    ///     接近后可触发物品信息
    /// </summary>
    public class ItemNearestAction : NearestAction
    {
        [Header("玩家接近时的提示图标")]
        public SpriteRenderer SR_NotifyIcon;

        protected override void OnTriggerChanged()
        {
            SR_NotifyIcon.gameObject.SetActive(IsTriggered);
        }

        [SerializeField]
        [Header("物品信息")]
        public ItemInfo ItemInfo;

        private void OnEnable()
        {
            if (!SR_NotifyIcon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置SR_NotifyIcon");
                return;
            }

            SR_NotifyIcon.gameObject.SetActive(IsTriggered);
            InputWarp.OnActionKeyDown += ShowItem;
        }

        private void OnDisable()
        {
            if (!SR_NotifyIcon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置SR_NotifyIcon");
                return;
            }

            IsTriggered = false;
            InputWarp.OnActionKeyDown -= ShowItem;
        }

        public void ShowItem()
        {
            if (!IsTriggered)
            {
                return;
            }

            ItemPanel.Instance.SetItemInfo(ItemInfo);
        }
    }
}