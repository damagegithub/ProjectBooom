using _ProjectBooom_.ObservableData;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    /// <summary>
    ///  修改变量触发器
    /// </summary>
    public class SetVarNearestAction : NearestAction
    {
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/创建修改变量事件", false, 10)]
        public static void CreateSpeakMessageAction(UnityEditor.MenuCommand menuCommand)
        {
            GameObject go = new("修改变量事件");
            SetVarNearestAction sna = go.AddComponent<SetVarNearestAction>();
            sna.IsOnce = true;
            sna.TriggerLayer = LayerMask.GetMask("Player");
            sna.VarName = "填写需要改变的变量";
            sna.VarValue = 1f;
            BoxCollider2D bc2d = go.AddComponent<BoxCollider2D>();
            bc2d.isTrigger = true;
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif
        
        public string VarName;
        public float  VarValue;

        private void OnEnable()
        {
            // 开局设置为0
            GlobalVariable.SetVarValue(VarName, 0f);
        }

        public override void DoAction()
        {
            if (!IsTriggered)
            {
                base.DoAction();
                GlobalVariable.SetVarValue(VarName, VarValue);
            }
        }
    }
}