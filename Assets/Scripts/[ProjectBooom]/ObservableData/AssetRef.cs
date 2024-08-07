using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.ObservableData
{
    /// <summary>
    ///     场景资源引用
    /// </summary>
    public class AssetRef : Singleton<AssetRef>
    {
        [SerializeField]
        [Header("鼠标UI常规图片")]
        public Texture2D MouseNormal;

        [SerializeField]
        [Header("鼠标UI抓取图片")]
        public Texture2D MouseDrag;

        [SerializeField]
        [Header("玩家角色交互物品提示图标")]
        public Sprite ItemTipIcon;

        [SerializeField]
        [Header("玩家角色交互开关提示图标")]
        public Sprite SwitchTipIcon;
    }
}