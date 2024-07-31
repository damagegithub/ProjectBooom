using _ProjectBooom_.DataStruct;
using LYP_Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI
{
    /// <summary>
    ///     物品信息面板
    /// </summary>
    public class ItemPlane : Singleton<ItemPlane>
    {
        [Header("物品信息名称")]
        public TextMeshProUGUI TMP_Name;
        [Header("物品信息图标")]
        public Image IMG_Icon;
        [Header("物品信息描述")]
        public TextMeshProUGUI TMP_Description;
        [Header("物品信息确认按钮")]
        public Button BTN_Ok;
        [Header("物品信息关闭按钮")]
        public Button BTN_Close;

        private bool Valid()
        {
            bool result = true;

            if (!TMP_Name)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置TMP_Name");
                result = false;
            }

            if (!IMG_Icon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置IMG_Icon");
                result = false;
            }

            if (!TMP_Description)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置TMP_Description");
                result = false;
            }

            if (!BTN_Close)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置BTN_Close");
                result = false;
            }

            return result;
        }

        public void SetItemInfo(in ItemInfo itemInfo)
        {
            if (!Valid())
            {
                return;
            }

            TMP_Name.text = itemInfo.Name;
            IMG_Icon.sprite = itemInfo.Icon;
            TMP_Description.text = itemInfo.Description;
            gameObject.SetActive(true);
        }
    }
}