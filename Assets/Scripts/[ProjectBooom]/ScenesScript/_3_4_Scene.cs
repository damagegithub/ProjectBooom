using System.Collections;
using _ProjectBooom_.PuzzleMono.UI;
using DG.Tweening;
using PBDialogueSystem;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    public class _3_4_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        [Header("全屏遮挡画布")] public CanvasGroup BlackCanvasGroup;
        
        [Header("AVG控制器")]
        public DialogueController DialogueController;

        [Header("场景开场对话ID")]
        public int LevelStartDialogIndex;
        
        [Header("进行中的对话ID")]
        public int CurrentDialogIndex = -1;
        
        private void Awake()
        {
            if (!DialogueController)
            {
                DialogueController = FindObjectOfType<DialogueController>(true);
            }

            DialogueController.OnOneConversationEnd += DialogFinish;
            BlackCanvasGroup.alpha = 1;
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
        public void LevelBeginAvgDialog()
        {
            StoryController.SetDebugText("场景开始AVG对话");
            StartCoroutine(StartAVGSystemCoroutine(LevelStartDialogIndex));
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

            StoryController.TryFinishCurrentStory();
        }
    }
}