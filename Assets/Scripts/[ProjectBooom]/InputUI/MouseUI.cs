using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.RealPlayerAction;
using LYP_Utils;
using LYP_Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.InputUI
{
    public class MouseUI : Singleton<MouseUI>
    {
        [SerializeField]
        [Header("鼠标HUD UI Canvas")]
        private Canvas _mouseCanvas;
        private RectTransform _mouseCanvasRect;
        [SerializeField]
        [Header("鼠标UI图标")]
        private Image _imageMouse;
        private Transform _imageMouseTrans;
        [SerializeField]
        [Header("鼠标UI颜色遮罩")]
        private Color _colorMask;
        [SerializeField]
        [Header("鼠标UI常规图片")]
        private Sprite _spriteMouseNormal;
        [SerializeField]
        [Header("鼠标UI抓取图片")]
        private Sprite _spriteMouseDrag;

        private bool _oldUseSystemCursor;
        [Header("使用系统鼠标")]
        public bool UseSystemCursor;

        private Camera _camera;

        private bool Valid()
        {
            bool result = true;

            if (!_mouseCanvas)
            {
                DebugHelper.LogWarning($"{name} 没有设置_mouseCanvas");
                result = false;
            }

            if (!_imageMouse)
            {
                DebugHelper.LogWarning($"{name} 没有设置_imageMouse");
                result = false;
            }

            if (!_spriteMouseNormal)
            {
                DebugHelper.LogWarning($"{name} 没有设置_spriteMouseNormal");
                result = false;
            }

            if (!_spriteMouseDrag)
            {
                DebugHelper.LogWarning($"{name} 没有设置_spriteMouseDrag");
                result = false;
            }

            return result;
        }

        public void UpdateMouseState()
        {
            if (!Valid())
            {
                return;
            }

            if (_oldUseSystemCursor != UseSystemCursor)
            {
                _oldUseSystemCursor = UseSystemCursor;
                Cursor.visible = UseSystemCursor;
                _mouseCanvas.gameObject.SetActive(!UseSystemCursor);
            }

            // 如果是系统鼠标
            if (UseSystemCursor)
            {
                return;
            }

            Vector2 mousePos = InputWarp.MousePosition();

            bool mouseInsideCanvas = RectTransformUtility
               .RectangleContainsScreenPoint(_mouseCanvasRect, mousePos);
            if (!mouseInsideCanvas)
            {
                return;
            }

            bool canLocalPoint = _mouseCanvasRect
               .ScreenPointToLocalPointInRectangle(mousePos, _mouseCanvas, out Vector2 localPoint);
            if (!canLocalPoint)
            {
                return;
            }

            _imageMouseTrans.localPosition = localPoint;
            _imageMouse.color = _colorMask;

            if (RuntimeUnimportantData.DraggingItem
             || RuntimeUnimportantData.FocusedMouseAction is ItemMouseAction)
            {
                if (_imageMouse.sprite != _spriteMouseDrag)
                {
                    _imageMouse.sprite = _spriteMouseDrag;
                }
            }
            else
            {
                if (_imageMouse.sprite != _spriteMouseNormal)
                {
                    _imageMouse.sprite = _spriteMouseNormal;
                }
            }
        }

        private void Start()
        {
            if (!Valid())
            {
                return;
            }

            _mouseCanvasRect = _mouseCanvas.GetComponent<RectTransform>();
            _imageMouseTrans = _imageMouse.transform;

            _oldUseSystemCursor = !UseSystemCursor; // 强制不一致
        }

        private void Update()
        {
            UpdateMouseState();
        }
    }
}