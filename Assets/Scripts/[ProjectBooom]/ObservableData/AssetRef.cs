using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.ObservableData
{
    /// <summary>
    ///     场景资源引用
    /// </summary>
    public class AssetRef : Singleton<AssetRef>
    {
        private void OnEnable()
        {
            DontDestroyOnLoad(GlobalManager);
            
            int normalX = MouseNormal.width;
            int normalY = MouseNormal.height;
            MouseNormal = Resize(MouseNormal, (int)(normalX * MouseScale), (int)(normalY * MouseScale));

            int dragX = MouseDrag.width;
            int dragY = MouseDrag.height;
            MouseDrag = Resize(MouseDrag, (int)(dragX * MouseScale), (int)(dragY * MouseScale));
        }

        private Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            RenderTexture rt = new(targetX, targetY, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);
            Texture2D result = new(targetX, targetY);
            result.name = texture2D.name;
            result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
            result.Apply();
            return result;
        }

        [SerializeField]
        [Header("鼠标UI缩放比例")]
        public float MouseScale = 0.8f;

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