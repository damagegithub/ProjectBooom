using System.Collections;
using _ProjectBooom_.DataStruct;
using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using DG.Tweening;
using LYP_Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.RealPlayerAction
{
    /// <summary>
    ///     现实玩家在场景中的可抓取物品信息
    /// </summary>
    public class ItemMouseAction : MouseAction
    {
        /// <summary>
        ///     物品一开始在场景中的位置
        /// </summary>
        public Vector3 SceneOriginPosition;

        /// <summary>
        ///     物品被拖动前的起点位置
        /// </summary>
        public Vector3 OriginPosition;

        /// <summary>
        ///     物品被拖动前的鼠标起点位置
        /// </summary>
        public Vector3 DragOriginMousePosition;

        /// <summary>
        ///     拖动中的鼠标相对起点位置的偏移量
        /// </summary>
        public Vector3 DragMouseOffset;

        /// <summary>
        ///     物品信息
        /// </summary>
        public ItemInfo ItemInfo;

        /// <summary>
        ///     主摄像机 用于获取鼠标在世界空间的位置
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        ///     如果被拖动的物品放在物品槽中 则记录该物品槽
        /// </summary>
        private ItemMouseSlotAction _itemMouseSlotAction;

        /// <summary>
        ///     是否在拖动中
        /// </summary>
        private bool _isDragging;

        /// <summary>
        ///     物品图标是否跟随鼠标
        /// </summary>
        private bool _isFollowMouse;

        [SerializeField]
        [Header("物品图片")]
        private Transform _Trans_ItemImage;

        private void Awake()
        {
            SceneOriginPosition = OriginPosition = transform.position;
            _mainCamera = Camera.main;
        }

        private void OnMouseDown()
        {
            if (DOTween.IsTweening(this))
            {
                return;
            }

            if (!_isDragging)
            {
                if (_itemMouseSlotAction)
                {
                    _itemMouseSlotAction.SlotItem = null;
                    _itemMouseSlotAction = null;
                }

                DragOriginMousePosition = _mainCamera.ScreenToWorldPoint(InputWarp.MousePosition());
                _isDragging = true;
                RuntimeUnimportantData.BeginDragItem(this);
                StartCoroutine(DetectItemCatch());
            }
        }

        private void OnMouseDrag()
        {
            if (DOTween.IsTweening(this))
            {
                return;
            }

            if (_isDragging)
            {
                Vector3 mousePosWS = _mainCamera.ScreenToWorldPoint(InputWarp.MousePosition());
                DragMouseOffset = mousePosWS - DragOriginMousePosition;
                transform.position = OriginPosition + DragMouseOffset;
            }
        }

        private void OnMouseUp()
        {
            if (DOTween.IsTweening(this))
            {
                return;
            }

            // 物品拖动结束但是没有放置在物品槽中
            if (_isDragging && !RuntimeUnimportantData.ItemMouseSlotAction)
            {
                _isDragging = false;
                DragMouseOffset = Vector3.zero;
                RuntimeUnimportantData.EndDragItem(this);
                OriginPosition = SceneOriginPosition;
                transform.DOMove(OriginPosition, 0.5f)
                         .SetEase(Ease.OutElastic)
                         .SetId(this);
            }
            // 物品拖动结束并且放置在物品槽中
            else if (_isDragging && RuntimeUnimportantData.ItemMouseSlotAction)
            {
                _isDragging = false;
                DragMouseOffset = Vector3.zero;
                RuntimeUnimportantData.EndDragItem(this);
                _itemMouseSlotAction = RuntimeUnimportantData.ItemMouseSlotAction;
                transform.position = _itemMouseSlotAction.transform.position;
                _Trans_ItemImage.localPosition = Vector3.zero;
                _itemMouseSlotAction.SlotItem = this;
                OriginPosition = RuntimeUnimportantData.ItemMouseSlotAction.transform.position;
            }
        }

        /// <summary>
        ///     切换物品图标鼠标跟随(动效)
        /// </summary>
        private void SwitchMouseFollowState(bool isFollowMouse)
        {
            _isFollowMouse = isFollowMouse;
            if (DOTween.IsTweening(_Trans_ItemImage))
            {
                return;
            }

            StartCoroutine(UpdateItemImageFollowMouse());
        }

        /// <summary>
        ///     物品图标跟随鼠标的携程(动效)
        /// </summary>
        private IEnumerator UpdateItemImageFollowMouse()
        {
            yield return new WaitForNextFrameUnit();
            if (_isFollowMouse)
            {
                while (_isFollowMouse)
                {
                    yield return new WaitForNextFrameUnit();
                    Vector3 localPos;
                    if (RuntimeUnimportantData.ItemMouseSlotAction && _isDragging)
                    {
                        localPos = RuntimeUnimportantData.ItemMouseSlotAction.transform.position;
                        DebugHelper.Log($"RuntimeUnimportantData.ItemMouseSlotAction && _isDragging");
                    }
                    else
                    {
                        localPos = _mainCamera.ScreenToWorldPoint(InputWarp.MousePosition());
                    }

                    localPos.z = 0;

                    _Trans_ItemImage.position = Vector3.Lerp(
                        _Trans_ItemImage.position,
                        localPos,
                        Time.unscaledDeltaTime * 10
                    );
                }
            }
            else
            {
                while (_Trans_ItemImage.localPosition != Vector3.zero && !_isFollowMouse)
                {
                    yield return new WaitForNextFrameUnit();
                    _Trans_ItemImage.localPosition = Vector3.Lerp(
                        _Trans_ItemImage.localPosition,
                        Vector3.zero,
                        Time.unscaledDeltaTime * 10
                    );
                }
            }
        }

        [SerializeField]
        [Header("物品槽位的LayerMask")]
        private LayerMask _itemCatchLayerMask;

        /// <summary>
        ///     检测鼠标是否在物品槽中
        /// </summary>
        private IEnumerator DetectItemCatch()
        {
            yield return new WaitForNextFrameUnit();
            while (_isDragging)
            {
                yield return new WaitForNextFrameUnit();
                Vector3 point = _mainCamera.ScreenToWorldPoint(InputWarp.MousePosition());
                Collider2D hit2D = Physics2D.OverlapPoint(point, _itemCatchLayerMask);
                if (hit2D)
                {
                    ItemMouseSlotAction itemMouseSlot = hit2D.GetComponent<ItemMouseSlotAction>();
                    if (itemMouseSlot)
                    {
                        RuntimeUnimportantData.EnterItemMouseSlotAction(itemMouseSlot);
                    }
                    else
                    {
                        RuntimeUnimportantData.ExitItemMouseSlotAction(itemMouseSlot);
                    }
                }
                else
                {
                    if (RuntimeUnimportantData.ItemMouseSlotAction)
                    {
                        RuntimeUnimportantData.ExitItemMouseSlotAction(RuntimeUnimportantData.ItemMouseSlotAction);
                    }
                }
            }
        }

        private void OnMouseEnter()
        {
            RuntimeUnimportantData.FocusMouseAction(this);
            SwitchMouseFollowState(true);
        }

        private void OnMouseExit()
        {
            RuntimeUnimportantData.UnfocusMouseAction(this);
            SwitchMouseFollowState(false);
        }
    }
}