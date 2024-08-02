using _ProjectBooom_.DataStruct;
using _ProjectBooom_.ObservableData;
using LYP_Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.PuzzleMono.UI
{
    /// <summary>
    ///     物品信息容器 UI
    /// </summary>
    public class ItemContainerUI : MonoBehaviour
    {
        private ItemInfo _itemInfo;
        [Header("物品信息图标")]
        public Image IMG_Icon;
        [Header("物品信息名称")]
        public Text TMP_Name;

        public Button BTN_Use;
        public Color  BaseNormalColor;
        public Color  HoverNormalColor;
        public Color  BaseSelectedColor;
        public Color  HoverSelectedColor;

        private bool Valid()
        {
            bool result = true;
            if (!IMG_Icon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置IMG_Icon");
                result = false;
            }

            if (!TMP_Name)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置TMP_Name");
                result = false;
            }

            if (!BTN_Use)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置BTN_Use");
                result = false;
            }

            return result;
        }

        private void OnEnable()
        {
            if (!Valid())
            {
                return;
            }

            BTN_Use.onClick.AddListener(OnClickSelect);
            RuntimeUnimportantData.SelectedItemContainerUIChanged += UpdateBtnState;
            UpdateBtnState(RuntimeUnimportantData.SelectedItemContainerUI);
        }

        private void OnDisable()
        {
            if (!Valid())
            {
                return;
            }

            BTN_Use.onClick.RemoveListener(OnClickSelect);
            RuntimeUnimportantData.SelectedItemContainerUIChanged -= UpdateBtnState;
        }

        public void Bind(ItemInfo itemInfo)
        {
            _itemInfo = itemInfo;
            TMP_Name.text = itemInfo.Name;
            IMG_Icon.sprite = itemInfo.Icon;
        }

        public void UpdateBtnState(ItemContainerUI selectedItemContainerUI)
        {
            if (selectedItemContainerUI == this)
            {
                ColorBlock colors = BTN_Use.colors;
                colors.normalColor = HoverNormalColor;
                colors.selectedColor = HoverSelectedColor;
                BTN_Use.colors = colors;
            }
            else
            {
                ColorBlock colors = BTN_Use.colors;
                colors.normalColor = BaseNormalColor;
                colors.selectedColor = BaseSelectedColor;
                BTN_Use.colors = colors;
            }
        }

        public void OnClickSelect()
        {
            if (RuntimeUnimportantData.SelectedItemContainerUI == this)
            {
                RuntimeUnimportantData.UnselectItem(this);
            }
            else
            {
                RuntimeUnimportantData.SelectItem(this);
            }
        }
    }
}