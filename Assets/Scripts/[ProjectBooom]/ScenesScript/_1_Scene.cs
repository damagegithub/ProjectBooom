using System.Collections;
using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.UI;
using Controllers;
using DG.Tweening;
using PBDialogueSystem;
using Unity.VisualScripting;
using UnityEngine;

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
        ///  场景开始的对话
        /// </summary>
        public void LevelBeginAvgDialog()
        {
            StoryController.SetDebugText("场景开始AVG对话");
            StartCoroutine(StartAVGSystemCoroutine(LevelStartDialogIndex));
        }

        /// <summary>
        ///  场景结束的对话
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
        ///  教学
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

        // /// <summary>
        // ///     剧情1 苏醒
        // /// </summary>
        // public void StartAnimation()
        // {
        //     StartCoroutine(StartAnimationCoroutine());
        // }
        //
        // public IEnumerator StartAnimationCoroutine()
        // {
        //     yield return DOTween
        //                 .Sequence()
        //                 .Append(BlackCanvasGroup.DOFade(0f, 1.0f))
        //                  //  // 播放苏醒动画
        //                  // .AppendCallback(() =>
        //                  //  {
        //                  //      // if (StartAnimator)
        //                  //      // {
        //                  //      //     StartAnimator.enabled = true;
        //                  //      //     StartAnimator.SetTrigger("Start");
        //                  //      // }
        //                  //
        //                  //      StoryController.SetDebugText("播放苏醒动画");
        //                  //  })
        //                  //  // 等待动画播放完毕
        //                  // .AppendInterval(StartAnimationTime)
        //                  //  // 关闭动画
        //                  // .AppendCallback(() =>
        //                  //  {
        //                  //      if (StartAnimator)
        //                  //      {
        //                  //          StartAnimator.enabled = false;
        //                  //      }
        //                  //  })
        //                  // 显示博士对话框
        //                 .AppendCallback(() =>
        //                  {
        //                      StoryController.SetDebugText("播放博士对话");
        //                      DoctorSpeakController.Speak(DoctorText0);
        //                  })
        //                 .SetId(this)
        //                 .WaitForCompletion();
        //
        //     StoryController.SetDebugText("等待对话文字播放完毕");
        //     while (DoctorSpeakController.IsSpeaking)
        //     {
        //         yield return new WaitForEndOfFrame();
        //     }
        //
        //     StoryController.TryFinishCurrentStory();
        // }

        // /// <summary>
        // ///     剧情2 学习移动
        // /// </summary>
        // public void LearnMovement()
        // {
        //     StartCoroutine(LearnMovementCoroutine());
        // }
        //
        // public IEnumerator LearnMovementCoroutine()
        // {
        //     StoryController.SetDebugText("学习移动");
        //     yield return DoctorSpeakController.SpeakAndWait(DoctorText1);
        //     yield return DOTween
        //                 .To(() => PlayerController.maxSpeed,
        //                     x => PlayerController.maxSpeed = x,
        //                     _initMoveSpeed,
        //                     5.0f)
        //                 .SetId(this)
        //                 .WaitForCompletion();
        //
        //     LeftButton.alpha = 1;
        //     RightButton.alpha = 1;
        //     StoryController.SetDebugText("等待左移动或右移动按下");
        //     bool leftButtonClicked = false;
        //     bool rightButtonClicked = false;
        //     while (leftButtonClicked == false || rightButtonClicked == false)
        //     {
        //         if (InputWarp.LeftMoveDown())
        //         {
        //             leftButtonClicked = true;
        //             LeftButton.DOFade(0f, 0.5f).SetId(this);
        //         }
        //
        //         if (InputWarp.RightMoveDown())
        //         {
        //             rightButtonClicked = true;
        //             RightButton.DOFade(0f, 0.5f).SetId(this);
        //         }
        //
        //         yield return new WaitForNextFrameUnit();
        //     }
        //
        //     StoryController.SetDebugText("等待对话结束和动画结束");
        //     while (DoctorSpeakController.IsSpeaking || DOTween.IsTweening(this))
        //     {
        //         // 等待博士对话结束
        //         yield return new WaitForNextFrameUnit();
        //     }
        //
        //     StoryController.TryFinishCurrentStory();
        // }

        // /// <summary>
        // ///     剧情3 结束当前场景
        // /// </summary>
        // public void FinishAnimation()
        // {
        //     StartCoroutine(FinishAnimationCoroutine());
        // }
        //
        // public IEnumerator FinishAnimationCoroutine()
        // {
        //     StoryController.SetDebugText("结束场景动画");
        //     yield return DoctorSpeakController.SpeakAndWait(DoctorText2);
        //
        //     yield return BlackCanvasGroup
        //                 .DOFade(1f, 1.0f)
        //                 .OnComplete(() => { StoryController.SetDebugText("结束当前场景"); })
        //                 .SetId(this)
        //                 .WaitForCompletion();
        //
        //
        //     StoryController.TryFinishCurrentStory();
        // }
    }
}