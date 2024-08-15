using System.Collections;
using Controllers;
using DG.Tweening;
using PBDialogueSystem;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    /// 会议室
    /// </summary>
    public class _4_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        [Header("全屏遮挡画布")] public CanvasGroup BlackCanvasGroup;

        [Header("AVG控制器")]
        public DialogueController DialogueController;

        [Header("对话ID")]
        public int DialogIndex0;
        public int DialogIndex1;

        [Header("进行中的对话ID")]
        public int CurrentDialogIndex = -1;

        [Header("玩家控制器 02")]
        public PlayerController PlayerController02;

        [Header("玩家02需要移动到的目标点")]
        public float TargetPosX;

        private void Awake()
        {
            if (!DialogueController)
            {
                DialogueController = FindObjectOfType<DialogueController>(true);
            }

            DialogueController.OnOneConversationEnd += DialogFinish;
            BlackCanvasGroup.alpha = 1;

            PlayerController02.IsScriptControl = true;
        }

        public void DialogFinish(int dialogIndex)
        {
            if (CurrentDialogIndex == dialogIndex)
            {
                CurrentDialogIndex = -1;
            }
        }

        /// <summary>
        ///     场景开始的对话
        /// </summary>
        public void SingleStory()
        {
            StartCoroutine(SingleStoryCoroutine());
        }

        private IEnumerator SingleStoryCoroutine()
        {
            StoryController.SetDebugText("场景开始AVG对话");
            yield return StartAVGSystemCoroutine(DialogIndex0);
            // 控制移动
            var dir = TargetPosX - PlayerController02.transform.position.x;
            var speed = PlayerController02.maxSpeed * Mathf.Sign(dir);
            PlayerController02.ScriptSpeed = new Vector2(speed, 0);

            float newDir = dir;
            while (Mathf.Approximately(Mathf.Sign(newDir), Mathf.Sign(dir)))
            {
                yield return new WaitForEndOfFrame();
                newDir = TargetPosX - PlayerController02.transform.position.x;
            }

            PlayerController02.ScriptSpeed = Vector2.zero;
            var playerPos = PlayerController02.transform.position;
            playerPos.x = TargetPosX;
            PlayerController02.transform.position = playerPos;

            yield return StartAVGSystemCoroutine(DialogIndex1, true);
        }

        private IEnumerator StartAVGSystemCoroutine(int dialogIndex, bool fadeEnd = false)
        {
            yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();

            CurrentDialogIndex = dialogIndex;
            DialogueController.StartConversation(dialogIndex);

            while (CurrentDialogIndex == dialogIndex)
            {
                yield return new WaitForEndOfFrame();
            }

            if (fadeEnd)
            {
                yield return BlackCanvasGroup.DOFade(1f, 1.0f).SetId(this).WaitForCompletion();
            }
        }
    }
}