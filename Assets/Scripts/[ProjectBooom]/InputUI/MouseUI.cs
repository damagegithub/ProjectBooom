using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.RealPlayerAction;
using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.InputUI
{
    public class MouseUI : Singleton<MouseUI>
    {
        [SerializeField]
        [Header("鼠标UI常规图片")]
        private Texture2D _mouseNormal;
        [SerializeField]
        [Header("鼠标UI抓取图片")]
        private Texture2D _mouseDrag;

        [Header("使用系统鼠标")]
        public bool UseSystemCursor;

        private Texture2D _oldCursor;

        private bool Valid()
        {
            bool result = true;

            if (!_mouseNormal)
            {
                DebugHelper.LogWarning($"{name} 没有设置_spriteMouseNormal");
                result = false;
            }

            if (!_mouseDrag)
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

            // 如果是系统鼠标
            if (UseSystemCursor)
            {
                DebugHelper.Log($"set system cursor");
                SetCursorIcon(null);
            }
            else
            {
                if (RuntimeUnimportantData.DraggingItem || RuntimeUnimportantData.FocusedMouseAction is ItemMouseAction)
                {
                    DebugHelper.Log($"set drag cursor");
                    SetCursorIcon(_mouseDrag);
                }
                else
                {
                    DebugHelper.Log($"set normal cursor");
                    SetCursorIcon(_mouseNormal);
                }
            }
        }

        private void SetCursorIcon(Texture2D newCursor)
        {
            if (_oldCursor != newCursor)
            {
                _oldCursor = newCursor;
                if (_oldCursor)
                {
                    Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.ForceSoftware);
                }
                else
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        private void Update()
        {
            UpdateMouseState();
        }
    }
}