using System;
using _ProjectBooom_.PuzzleMono.RealPlayerAction;

namespace _ProjectBooom_.ObservableData
{
    /// <summary>
    ///     运行时不重要数据
    /// </summary>
    public static class RuntimeUnimportantData
    {
        #region 场景中鼠标所在的物品捕获槽

        public static Action<ItemMouseSlotAction> ItemMouseSlotActionChanged;

        public static ItemMouseSlotAction ItemMouseSlotAction { get; private set; }

        public static void EnterItemMouseSlotAction(ItemMouseSlotAction itemMouseSlot)
        {
            if (ItemMouseSlotAction != itemMouseSlot)
            {
                ItemMouseSlotAction = itemMouseSlot;
                ItemMouseSlotActionChanged?.Invoke(ItemMouseSlotAction);
            }
        }

        public static void ExitItemMouseSlotAction(ItemMouseSlotAction itemMouseSlot)
        {
            if (ItemMouseSlotAction == itemMouseSlot) 
            {
                ItemMouseSlotAction = null;
                ItemMouseSlotActionChanged?.Invoke(ItemMouseSlotAction);
            }
        }

        #endregion

        #region 场景中鼠标位置有焦点的对象

        public static Action<MouseAction> FocusedMouseActionChanged;

        public static MouseAction FocusedMouseAction { get; private set; }

        public static void FocusMouseAction(MouseAction mouseAction)
        {
            if (FocusedMouseAction != mouseAction)
            {
                FocusedMouseAction = mouseAction;
                FocusedMouseActionChanged?.Invoke(FocusedMouseAction);
            }
        }

        public static void UnfocusMouseAction(MouseAction mouseAction)
        {
            if (FocusedMouseAction == mouseAction)
            {
                FocusedMouseAction = null;
                FocusedMouseActionChanged?.Invoke(FocusedMouseAction);
            }
        }

        #endregion

        #region 正在拖拽的物品

        public static Action<ItemMouseAction> DraggingItemChanged;

        public static ItemMouseAction DraggingItem { get; private set; }

        public static void BeginDragItem(ItemMouseAction item)
        {
            if (DraggingItem != item)
            {
                DraggingItem = item;
                DraggingItemChanged?.Invoke(DraggingItem);
            }
        }

        public static void EndDragItem(ItemMouseAction item)
        {
            if (DraggingItem == item)
            {
                DraggingItem = null;
                DraggingItemChanged?.Invoke(DraggingItem);
            }
        }

        #endregion
    }
}