using _ProjectBooom_.ObservableData;
using DG.Tweening;
using LYP_Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _ProjectBooom_.PuzzleMono.SceneEffect
{
    /// <summary>
    ///     光源切换开关
    /// </summary>
    public class LightSwitcher : MonoBehaviour
    {
        private Light2D _light2D;
        private float   _originalIntensity;

        [Header("光源切换时间")]
        public float LightSwitchTime = 0.5f;

        [Header("开关名称")]
        public string SwitchName;

        [Header("开关状态")]
        public bool SwitchState;

        private void Awake()
        {
            _light2D = GetComponent<Light2D>();
            if (!_light2D)
            {
                Debug.LogWarning($"{gameObject.name} 没有设置_light2D");
                return;
            }

            _originalIntensity = _light2D.intensity;
        }

        private void OnEnable()
        {
            if (!GlobalVariable.ExistVar(SwitchName))
            {
                DebugHelper.LogWarning($"{gameObject.name} 可能不存在开关名称 {SwitchName}");
            }

            SwitchChange(GlobalVariable.GetVarValue(SwitchName));
            GlobalVariable.AddVarListener(SwitchName, SwitchChange);
        }

        private void OnDisable()
        {
            GlobalVariable.RemoveVarListener(SwitchName, SwitchChange);
        }

        private void SwitchChange(float value)
        {
            SwitchState = Mathf.Approximately(1F, value);
            float targetIntensity = SwitchState ? _originalIntensity : 0;
            if (DOTween.IsTweening(this))
            {
                DOTween.Kill(this);
            }

            DOTween.To(() => _light2D.intensity, x => _light2D.intensity = x, targetIntensity, LightSwitchTime)
                   .SetEase(Ease.InExpo)
                   .SetId(this);
        }
    }
}