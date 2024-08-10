using System;
using System.Collections.Generic;
using _ProjectBooom_.PuzzleMono.CharacterAction;
using _ProjectBooom_.PuzzleMono.RealPlayerAction;
using UnityEngine;

namespace _ProjectBooom_.ObservableData
{
    /// <summary>
    ///     运行时不重要数据
    /// </summary>
    public static class RuntimeUnimportantData
    {
        #region 场景中可交互的对象

        public static Action ActionObjectChanged;

        public static readonly List<NearestAction> ActionObjects = new();

        public static void EnterActionObject(NearestAction actionObject)
        {
            ActionObjects.RemoveAll(na => !na);
            
            if (!ActionObjects.Contains(actionObject))
            {
                ActionObjects.Add(actionObject);
                ActionObjectChanged?.Invoke();
            }
        }

        public static void ExitActionObject(NearestAction actionObject)
        {
            ActionObjects.RemoveAll(na => !na);
            
            if (ActionObjects.Contains(actionObject))
            {
                ActionObjects.Remove(actionObject);
                ActionObjectChanged?.Invoke();
            }
        }

        /// <summary>
        ///     获取最接近的可交互对象
        /// </summary>
        public static NearestAction GetNearestActionObject(Vector3 position)
        {
            ActionObjects.RemoveAll(na => !na);
            
            if (ActionObjects.Count == 0)
            {
                return null;
            }

            NearestAction nearestAction = ActionObjects[0];
            float minDistance = Vector3.Distance(nearestAction.transform.position, position);

            for (int i = 1; i < ActionObjects.Count; i++)
            {
                float distance = Vector3.Distance(ActionObjects[i].transform.position, position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestAction = ActionObjects[i];
                }
            }

            return nearestAction;
        }

        #endregion

        #region 场景中鼠标所在的物品捕获槽

        public static Action<ItemMouseSlotAction> ItemMouseSlotActionChanged;

        private static ItemMouseSlotAction _itemMouseSlotAction;
        public static ItemMouseSlotAction ItemMouseSlotAction 
        {
            get => _itemMouseSlotAction ? _itemMouseSlotAction : null;
            private set => _itemMouseSlotAction = value;
        }

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

        private static MouseAction _focusedMouseAction;

        public static MouseAction FocusedMouseAction
        {
            get => _focusedMouseAction ? _focusedMouseAction : null;
            private set => _focusedMouseAction = value;
        }

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

        private static ItemMouseAction _draggingItem;

        public static ItemMouseAction DraggingItem
        {
            get => _draggingItem ? _draggingItem : null;
            private set => _draggingItem = value;
        }

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