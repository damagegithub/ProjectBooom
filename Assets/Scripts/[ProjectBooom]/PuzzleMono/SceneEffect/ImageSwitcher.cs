using _ProjectBooom_.ObservableData;
using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.SceneEffect
{
    /// <summary>
    ///     打开或关闭开关后会切换的图片
    /// </summary>
    public class ImageSwitcher : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField]
        [Header("开关开启时的图片")]
        private Sprite _switchOn;
        [SerializeField]
        [Header("开关关闭时的图片")]
        private Sprite _switchOff;

        [Header("开关名称")]
        public string SwitchName;

        [Header("开关状态")]
        public bool SwitchState;

        private void OnEnable()
        {
            if (!_spriteRenderer)
            {
                Debug.LogWarning($"{gameObject.name} 没有设置_spriteRenderer");
                return;
            }

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
            _spriteRenderer.sprite = SwitchState ? _switchOn : _switchOff;
        }
    }
}