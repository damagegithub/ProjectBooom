using System.Collections.Generic;
using PBDialogueSystem;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    /// <summary>
    ///     接近后可触发对话系统
    /// </summary>
    public class DialogSystemNearestAction : NearestAction
    {
        private StoryController _storyController;
        /// <summary>
        ///     触发条件变量名 (不为空则表示条件触发)
        /// </summary>
        [Header("触发条件变量名 (不为空则表示条件触发)")]
        [SerializeField]
        public string TriggerVarName;

        /// <summary>
        ///     AVG系统控制器
        /// </summary>
        [Header("AVG系统控制器")]
        [SerializeField]
        public DialogueController DialogueController;

        [Header("对话系统ID")]
        [SerializeField]
        public int DialoguetConversationID;

        private int       IsDialoggingID          = -1;
        private List<int> FinishedConversationIDs = new();

        private void Awake()
        {
            IsDialoggingID = -1;
            FinishedConversationIDs.Clear();
            if (!_storyController)
            {
                _storyController = FindObjectOfType<StoryController>(true);
            }

            if (!DialogueController)
            {
                DialogueController = FindObjectOfType<DialogueController>(true);
            }
        }

        private void OnEnable()
        {
            DialogueController.OnOneConversationEnd += FinishDialog;
        }

        private void OnDisable()
        {
            DialogueController.OnOneConversationEnd -= FinishDialog;
        }

        public override void DoAction()
        {
            if (!string.IsNullOrWhiteSpace(TriggerVarName))
            {
                // 如果条件变量不满足则不触发
                if (PlayerPrefs.GetInt(TriggerVarName) > 0)
                {
                    return;
                }
            }

            if (!IsTriggered)
            {
                base.DoAction();
                ShowDialog();
            }
        }

        /// <summary>
        ///     显示对话
        /// </summary>
        public void ShowDialog()
        {
            if (DialogueController)
            {
                if (IsDialoggingID == DialoguetConversationID)
                {
                    _storyController?.SetDebugText($"当前对话ID正在进行中 {DialoguetConversationID}");
                    return;
                }

                if (FinishedConversationIDs.Contains(DialoguetConversationID))
                {
                    _storyController?.SetDebugText($"当前对话ID可能曾经已经对话过 {DialoguetConversationID}");
                    return;
                }

                DialogueController.StartConversation(DialoguetConversationID);
            }
        }

        /// <summary>
        ///     结束对话
        /// </summary>
        public void FinishDialog(int dialogId)
        {
            IsDialoggingID = -1;
            FinishedConversationIDs.Add(dialogId);
        }
    }
}