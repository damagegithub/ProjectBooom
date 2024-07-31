using _ProjectBooom_.DataStruct;
using LYP_Utils;
using TMPro;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI
{
    /// <summary>
    ///     物品信息面板
    /// </summary>
    public class ItemPlane : Singleton<ItemPlane>
    {
        public TextMeshProUGUI TMP_Name;
        public Image           IMG_Icon;
        public TextMeshProUGUI TMP_Description;
        public Button          BTN_Ok;
        public Button          BTN_Close;

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

            if (!BTN_Ok)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置BTN_Ok");
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
            if (Valid())
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