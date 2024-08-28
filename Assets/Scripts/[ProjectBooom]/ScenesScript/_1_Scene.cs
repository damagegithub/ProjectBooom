using System.Collections;
using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.UI;
using Controllers;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using PBDialogueSystem;
using TMPro;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    ///     用于第一个场景的脚本
    /// </summary>
    public class _1_Scene : MonoBehaviour
    {
        public PlayerController PlayerController;

        private float _initMoveSpeed;

        public StoryController StoryController;

        [Header("AVG控制器")]
        public DialogueController DialogueController;

        [Header("全屏遮挡画布")]
        public CanvasGroup BlackCanvasGroup;

        [Header("博士对话脚本")]
        public DoctorSpeakController DoctorSpeakController;

        [Header("苏醒动画时间")]
        public float StartAnimationTime = 2.0f;

        [Header("按键提示")]
        public SpriteRenderer TipTMP;

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
            _initMoveSpeed = PlayerController.maxSpeed;
            PlayerController.maxSpeed = 0;
            BlackCanvasGroup.alpha = 1;
            TipTMP.DOFade(0f,0f);
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
            CurrentDialogIndex = dialogIndex;
            DialogueController.StartConversation(dialogIndex);
            yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();

            yield return new WaitUntil(() => CurrentDialogIndex != dialogIndex);

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
            // 高亮按键提示
            TweenerCore<Color, Color, ColorOptions> tipDOTweeen = TipTMP.DOFade(1f, 1.0f);
            // 等待第一次移动
            PlayerController.maxSpeed = 1;
            yield return new WaitUntil(() => InputWarp.LeftMoveDown() || InputWarp.RightMoveDown());

            DOTween.To(() => PlayerController.maxSpeed,
                       x => PlayerController.maxSpeed = x,
                       _initMoveSpeed,
                       10.0f)
                   .SetId(this);
            StoryController.SetDebugText("等待触发器");
            DoctorSpeakController.SpeakWithoutFade(DoctorText1);
            // 等待某个变量被触发
            yield return new WaitUntil(() => GlobalVariable.GetVarValue("Level1_已到达教学地点") != 0f);

            tipDOTweeen.Kill();
            TipTMP.DOFade(0f, 1.0f);
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