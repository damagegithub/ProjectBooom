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
    ///     场景中的可抓取物品信息
    /// </summary>
    public class ItemMouseAction : MouseAction
    {
        public Vector3 SceneOriginPosition;
        
        public Vector3 OriginPosition;

        public Vector3 DragOriginMousePosition;

        public Vector3 DragMouseOffset;

        public ItemInfo ItemInfo;

        private Camera _mainCamera;
        
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
        private LayerMask _itemCatchLayerMask;

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