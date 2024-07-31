using _ProjectBooom_.DataStruct;
using _ProjectBooom_.Input;
using _ProjectBooom_.PuzzleMono.UI;
using LYP_Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono
{
    /// <summary>
    ///     用于挂载到场景中碰撞体上，当玩家进入时触发 离开时取消触发
    /// </summary>
    public class ItemNearestAction : NearestAction
    {
        [Header("玩家接近时的提示按钮")]
        public Button BTN_NotifyIcon;

        [SerializeField]
        [Header("物品信息")]
        public ItemInfo ItemInfo;

        private void OnEnable()
        {
            if (!BTN_NotifyIcon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置BTN_NotifyIcon");
                return;
            }

            BTN_NotifyIcon.gameObject.SetActive(IsTriggered);
            BTN_NotifyIcon.onClick.AddListener(ShowItem);
            InputWarp.OnActionKeyDown += ShowItem;
        }

        private void OnDisable()
        {
            if (!BTN_NotifyIcon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置BTN_NotifyIcon");
                return;
            }

            BTN_NotifyIcon.onClick.RemoveListener(ShowItem);
            InputWarp.OnActionKeyDown -= ShowItem;
        }

        protected override void OnTriggerChanged()
        {
            BTN_NotifyIcon.gameObject.SetActive(IsTriggered);
        }

        public void ShowItem()
        {
            if (!IsTriggered)
            {
                return;
            }

            ItemPlane.Instance.SetItemInfo(ItemInfo);
        }
    }
}