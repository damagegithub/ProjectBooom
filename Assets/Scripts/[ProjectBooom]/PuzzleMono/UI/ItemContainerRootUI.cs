using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.UI
{
    /// <summary>
    ///     物品容器根节点
    /// </summary>
    public class ItemContainerRootUI : MonoBehaviour
    {
        [Header("物品容器根节点")]
        public RectTransform ContentRoot;
        [Header("物品容器预制体")]
        public GameObject ItemContainerPrefab;
        [Header("物品拖动捕获面板")]
        public GameObject ItemCatchPanel;
    }
}