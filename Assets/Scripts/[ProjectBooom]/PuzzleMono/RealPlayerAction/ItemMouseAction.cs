using _ProjectBooom_.DataStruct;
using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using DG.Tweening;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.RealPlayerAction
{
    /// <summary>
    ///     场景中的可抓取物品信息
    /// </summary>
    public class ItemMouseAction : MouseAction
    {
        public Vector3 OriginPosition;

        public Vector3 DragOriginMousePosition;

        public Vector3 DragMouseOffset;

        public ItemInfo ItemInfo;

        private bool _isDragging;

        private Camera _mainCamera;

        private void Awake()
        {
            OriginPosition = transform.position;
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
                DragOriginMousePosition = _mainCamera.ScreenToWorldPoint(InputWarp.MousePosition());
                _isDragging = true;
                RuntimeUnimportantData.BeginDragItem(this);
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

            if (_isDragging)
            {
                _isDragging = false;
                DragMouseOffset = Vector3.zero;
                RuntimeUnimportantData.EndDragItem(this);
                transform.DOMove(OriginPosition, 0.5f)
                         .SetEase(Ease.OutElastic)
                         .SetId(this);
            }
        }

        private void OnMouseEnter()
        {
            RuntimeUnimportantData.FocusMouseAction(this);
        }

        private void OnMouseExit()
        {
            RuntimeUnimportantData.UnfocusMouseAction(this);
        }
    }
}