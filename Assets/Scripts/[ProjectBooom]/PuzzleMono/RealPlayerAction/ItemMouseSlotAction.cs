using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.RealPlayerAction
{
    /// <summary>
    ///     代表这是一个可以放置物品的槽位
    /// </summary>
    public class ItemMouseSlotAction : MouseAction
    {
        public bool IsEmpty => !SlotItem;

        [Header("运行时当前槽位物品")]
        public ItemMouseAction SlotItem;

        [Header("槽位名称")]
        public string SlotName;

        [Header("需要正确放置的物品名称")]
        public string SuccessItemName;
    }
}