using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.UI
{
    /// <summary>
    ///  保存角色旋转时图标的缩放
    /// </summary>
    public class KeepIconScale : MonoBehaviour
    {
        private void Update()
        {
            Vector3 scale = transform.localScale;
            Vector3 lossyScale = transform.parent.localScale;

            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(lossyScale.x);
            scale.y = Mathf.Abs(scale.y) * Mathf.Sign(lossyScale.y);
            scale.z = Mathf.Abs(scale.z) * Mathf.Sign(lossyScale.z);
            
            transform.localScale = scale;
        }
    }
}