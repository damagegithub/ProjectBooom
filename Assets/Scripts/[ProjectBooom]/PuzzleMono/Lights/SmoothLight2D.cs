using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _ProjectBooom_.PuzzleMono.Lights
{
    /// <summary>
    ///  用于灯光的平滑过渡
    /// </summary>
    public class SmoothLight2D : MonoBehaviour
    {
        [Header("2D灯光组件")]
        public Light2D Light2D;

        [Header("原始强度")]
        public float OriginIntensity;

        [Header("渐变时间")]
        public float FadeDuration = 1f;

        [Header("是否在开始时渐变")]
        public bool FadeOnAwake = true;

        private void Awake()
        {
            if (!Light2D)
            {
                Light2D = GetComponent<Light2D>();
            }

            OriginIntensity = Light2D.intensity;
            if (FadeOnAwake)
            {
                Light2D.intensity = 0;
                Turn(true);
            }
        }

        public void Turn(bool isOn)
        {
            if (DOTween.IsTweening(this))
            {
                DOTween.Kill(this);
            }

            var targetIntensity = isOn ? OriginIntensity : 0;
            DOTween.To(() => Light2D.intensity,
                       x => Light2D.intensity = x,
                       targetIntensity,
                       FadeDuration)
                   .SetEase(Ease.InOutExpo)
                   .SetId(this);
        }
    }
}