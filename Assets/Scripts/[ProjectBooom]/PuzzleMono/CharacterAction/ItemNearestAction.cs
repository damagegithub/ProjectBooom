using _ProjectBooom_.DataStruct;
using _ProjectBooom_.PuzzleMono.UI;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    /// <summary>
    ///     接近后可触发物品信息
    /// </summary>
    public class ItemNearestAction : NearestAction
    {
        [SerializeField]
        [Header("物品信息")]
        public ItemInfo ItemInfo;

        public override void DoAction()
        {
            ShowItem();
        }

        public void ShowItem()
        {
            ItemPanel.Instance.SetItemInfo(ItemInfo);
        }
    }
}