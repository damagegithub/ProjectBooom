using System.Collections;
using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.UI;
using Controllers;
using DG.Tweening;
using PBDialogueSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    ///     用于第一个场景的脚本
    /// </summary>
    public class _1_Scene : MonoBehaviour
    {
        public  PlayerController PlayerController;
        private float            _initMoveSpeed;

        public StoryController StoryController;

        [Header("AVG控制器")]
        public DialogueController DialogueController;

        [Header("全屏遮挡画布")]
        public CanvasGroup BlackCanvasGroup;

        [Header("博士对话脚本")]
        public DoctorSpeakController DoctorSpeakController;

        [Header("苏醒动画时间")]
        public float StartAnimationTime = 2.0f;

        [Header("学习移动左按键")]
        public CanvasGroup LeftButton;

        [Header("学习移动右按键")]
        public CanvasGroup RightButton;

        [Header("开始场景对话")]
        public string DoctorText0;

        [Header("学习移动对话")]
        public string DoctorText1;

        [Header("完成场景对话")]
        public string DoctorText2;

        [Header("场景开场对话ID")]
        public int LevelStartDialogIndex;
        [Header("场景结束对话ID")]
        public int LevelEndDialogIndex;

        [Header("进行中的对话ID")]
        public int CurrentDialogIndex = -1;

        public void DialogFinish(int dialogIndex)
        {
            if (CurrentDialogIndex == dialogIndex)
            {
                CurrentDialogIndex = -1;
            }
        }

        private void Awake()
        {
            if (!DoctorSpeakController)
            {
                DoctorSpeakController = FindObjectOfType<DoctorSpeakController>(true);
            }

            if (!DialogueController)
            {
                DialogueController = FindObjectOfType<DialogueController>();
            }

            DialogueController.OnOneConversationEnd += DialogFinish;

            BlackCanvasGroup.alpha = 1;
            LeftButton.alpha = 0;
            RightButton.alpha = 0;
            _initMoveSpeed = PlayerController.maxSpeed;
            PlayerController.maxSpeed = 0;
            BlackCanvasGroup.alpha = 1;
        }

        /// <summary>
        ///     场景开始的对话
        /// </summary>
        public void LevelBeginAvgDialog()
        {
            StoryController.SetDebugText("场景开始AVG对话");
            StartCoroutine(StartAVGSystemCoroutine(LevelStartDialogIndex));
        }

        /// <summary>
        ///     场景结束的对话
        /// </summary>
        public void LevelEndAvgDialog()
        {
            StoryController.SetDebugText("场景结束AVG对话");
            StartCoroutine(StartAVGSystemCoroutine(LevelEndDialogIndex, true));
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

        /// <summary>
        ///     教学
        /// </summary>
        public void LevelTutorial()
        {
            StartCoroutine(LevelTutorialCoroutine());
        }

        private IEnumerator LevelTutorialCoroutine()
        {
            StoryController.SetDebugText("博士等待对话");
            DoctorSpeakController.SpeakWithoutFade(DoctorText0, true);
            // 等待第一次移动
            PlayerController.maxSpeed = 1;
            while (!InputWarp.LeftMoveDown() && !InputWarp.RightMoveDown())
            {
                // 等待左右移动
                yield return new WaitForNextFrameUnit();
            }

            DOTween.To(() => PlayerController.maxSpeed,
                       x => PlayerController.maxSpeed = x,
                       _initMoveSpeed,
                       10.0f)
                   .SetId(this);
            StoryController.SetDebugText("等待触发器");
            DoctorSpeakController.SpeakWithoutFade(DoctorText1);
            // 等待某个变量被触发
            while (0f == GlobalVariable.GetVarValue("Level1_已到达教学地点"))
            {
                yield return new WaitForNextFrameUnit();
            }

            yield return DoctorSpeakController.SpeakAndWait(DoctorText2);

            // 如果还在DOTween 就结束
            if (DOTween.IsTweening(this))
            {
                DOTween.Kill(this);
            }

            PlayerController.maxSpeed = _initMoveSpeed;
            // 到结束AVG
            StoryController.TryFinishCurrentStory();
        }
    }
}