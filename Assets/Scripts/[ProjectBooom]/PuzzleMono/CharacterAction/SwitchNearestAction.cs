using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    /// <summary>
    ///     接近后可触发开关切换
    /// </summary>
    public class SwitchNearestAction : NearestAction
    {
        [Header("玩家接近时的提示图标")]
        public SpriteRenderer SR_NotifyIcon;

        protected override void OnTriggerChanged()
        {
            SR_NotifyIcon.gameObject.SetActive(IsTriggered);
        }

        [Header("开关名称")]
        public string SwitchName;

        [Header("开关状态")]
        public bool SwitchState;

        private void OnEnable()
        {
            if (!SR_NotifyIcon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置SR_NotifyIcon");
                return;
            }

            SR_NotifyIcon.gameObject.SetActive(IsTriggered);

            if (!GlobalVariable.ExistVar(SwitchName))
            {
                DebugHelper.LogWarning($"{gameObject.name} 可能不存在开关名称 {SwitchName}");
            }

            SwitchState = Mathf.Approximately(1F, GlobalVariable.GetVarValue(SwitchName));
            GlobalVariable.AddVarListener(SwitchName, OnStateChange);
            InputWarp.OnActionKeyDown += ChangeState;
        }

        private void OnDisable()
        {
            if (!SR_NotifyIcon)
            {
                DebugHelper.LogWarning($"{gameObject.name} 没有设置SR_NotifyIcon");
                return;
            }

            IsTriggered = false;
            GlobalVariable.RemoveVarListener(SwitchName, OnStateChange);
            InputWarp.OnActionKeyDown -= ChangeState;
        }

        private void OnStateChange(float value)
        {
            SwitchState = Mathf.Approximately(1F, value);
        }

        private void ChangeState()
        {
            SwitchState = !SwitchState;
            GlobalVariable.SetVarValue(SwitchName, SwitchState ? 1F : 0F);
        }
    }
}