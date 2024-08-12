using _ProjectBooom_.PuzzleMono.UI;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    public class SpeakMessageNearestAction : NearestAction
    {
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/创建对话事件", false, 10)]
        public static void CreateSpeakMessageAction(UnityEditor.MenuCommand menuCommand)
        {
            GameObject go = new("对话事件");
            SpeakMessageNearestAction sna = go.AddComponent<SpeakMessageNearestAction>();
            sna.IsOnce = true;
            sna.TriggerLayer = LayerMask.GetMask("Player");
            sna.IsBreakable = true;
            sna.Message = "填写博士需要说的话";
            BoxCollider2D bc2d = go.AddComponent<BoxCollider2D>();
            bc2d.isTrigger = true;
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        #endif

        /// <summary>
        ///   是否可以中断当前对话并播放
        /// </summary>
        [Header("是否可以中断当前对话并播放")]
        [SerializeField]
        public bool IsBreakable;

        /// <summary>
        ///   对话内容
        /// </summary>
        [Header("对话内容")] [SerializeField] public string Message;

        public override void DoAction()
        {
            if (!IsTriggered)
            {
                base.DoAction();
                Speak();
            }
        }

        /// <summary>
        ///  播放对话
        /// </summary>
        private void Speak()
        {
            DoctorSpeakController dsc = FindObjectOfType<DoctorSpeakController>(true);
            if (!dsc)
            {
                return;
            }

            dsc.Speak(Message, IsBreakable);
        }
    }
}