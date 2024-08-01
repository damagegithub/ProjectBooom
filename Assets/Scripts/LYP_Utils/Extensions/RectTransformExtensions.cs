using UnityEngine;

namespace LYP_Utils.Extensions
{
    public static class RectTransformExtensions
    {
        /// <summary>
        ///     计算屏幕坐标到 RectTransform 的本地坐标
        /// </summary>
        public static bool ScreenPointToLocalPointInRectangle(
            this RectTransform rect,
            Vector2            screenPoint,
            Canvas             canvas,
            out Vector2        localPoint
        )
        {
            localPoint = default(Vector2);
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                return false;
            }
            else if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return RectTransformUtility
                   .ScreenPointToLocalPointInRectangle(rect, screenPoint, null, out localPoint);
            }
            else
            {
                Camera camera = canvas.worldCamera;
                if (camera == null)
                {
                    return false;
                }

                return RectTransformUtility
                   .ScreenPointToLocalPointInRectangle(rect, screenPoint, camera, out localPoint);
            }
        }
    }
}