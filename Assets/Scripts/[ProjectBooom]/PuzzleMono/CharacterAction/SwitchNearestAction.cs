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
        [Header("开关名称")]
        public string SwitchName;

        [Header("开关状态")]
        public bool SwitchState;

        private void OnEnable()
        {
            if (!GlobalVariable.ExistVar(SwitchName))
            {
                DebugHelper.LogWarning($"{gameObject.name} 可能不存在开关名称 {SwitchName}");
            }

            SwitchState = Mathf.Approximately(1F, GlobalVariable.GetVarValue(SwitchName));
            GlobalVariable.AddVarListener(SwitchName, OnStateChange);
        }

        private void OnDisable()
        {
            GlobalVariable.RemoveVarListener(SwitchName, OnStateChange);
        }

        private void OnStateChange(float value)
        {
            SwitchState = Mathf.Approximately(1F, value);
        }

        public override void DoAction()
        {
            ChangeState();
        }

        private void ChangeState()
        {
            SwitchState = !SwitchState;
            GlobalVariable.SetVarValue(SwitchName, SwitchState ? 1F : 0F);
        }
    }
}