using System;
using System.Collections;
using _ProjectBooom_.Input;
using Controllers;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    /// 用于第一个场景的脚本
    /// </summary>
    public class _1_Scene : MonoBehaviour
    {
        public PlayerController PlayerController;
        private float _initMoveSpeed;

        public StoryController StoryController;

        [Header("苏醒动画")] public Animator StartAnimator;

        [Header("博士对话框")] public CanvasGroup DoctorCanvasGroup;

        [Header("博士对话文字")] public TextMeshProUGUI TMP_DoctorText;

        [Header("苏醒动画时间")] public float StartAnimationTime = 2.0f;


        [Header("学习移动左按键")] public CanvasGroup LeftButton;

        [Header("学习移动右按键")] public CanvasGroup RightButton;

        [Header("苏醒博士对话框时间")] public float DoctorTextFadeTime0 = 5.0f;
        [Header("开始场景对话")] public string DoctorText0;

        [Header("学习移动博士对话框时间")] public float DoctorTextFadeTime1 = 5.0f;
        [Header("学习移动对话")] public string DoctorText1;

        [Header("完成场景博士对话框时间")] public float DoctorTextFadeTime2 = 5.0f;
        [Header("完成场景对话")] public string DoctorText2;

        private void Init()
        {
            LeftButton.alpha = 0;
            RightButton.alpha = 0;
            _initMoveSpeed = PlayerController.maxSpeed;
            PlayerController.maxSpeed = 0;
            DoctorCanvasGroup.alpha = 0;
        }

        /// <summary>
        /// 剧情1 苏醒
        /// </summary>
        public void StartAnimation()
        {
            Init();
            DOTween.Sequence()
                // 播放苏醒动画
                .AppendCallback(() =>
                {
                    if (StartAnimator)
                    {
                        StartAnimator.enabled = true;
                        StartAnimator.SetTrigger("Start");
                    }

                    StoryController.SetDebugText("播放苏醒动画");
                })
                // 等待动画播放完毕
                .AppendInterval(StartAnimationTime)
                // 关闭动画
                .AppendCallback(() =>
                {
                    if (StartAnimator)
                    {
                        StartAnimator.enabled = false;
                    }
                })
                // 显示博士对话框
                .AppendCallback(() =>
                {
                    StoryController.SetDebugText("播放博士对话");
                    TMP_DoctorText.maxVisibleCharacters = 0;
                    TMP_DoctorText.text = DoctorText0;
                })
                .Join(DoctorCanvasGroup.DOFade(1f, 0.5f))
                // 显示博士对话文字
                .Join(DOTween.To(
                    () => TMP_DoctorText.maxVisibleCharacters,
                    x => TMP_DoctorText.maxVisibleCharacters = x,
                    DoctorText0.Length,
                    DoctorTextFadeTime0
                ))
                .AppendInterval(0.5f)
                // 等待对话文字播放完毕
                .OnComplete(() =>
                {
                    StoryController.SetDebugText("等待对话文字播放完毕");
                    DoctorCanvasGroup.DOFade(0f, 0.5f);
                    StoryController.TryFinishCurrentStory();
                })
                .SetId(this);
        }

        /// <summary>
        ///  剧情2 学习移动
        /// </summary>
        public void LearnMovement()
        {
            StartCoroutine(LearnMovementCoroutine());
        }

        public IEnumerator LearnMovementCoroutine()
        {
            StoryController.SetDebugText("学习移动");
            TMP_DoctorText.text = DoctorText1;
            TMP_DoctorText.maxVisibleCharacters = 0;
            while (DOTween.IsTweening(this))
            {
                // 等待上个动画播放完毕
                yield return new WaitForNextFrameUnit();
            }

            DOTween.Sequence()
                .Append(DoctorCanvasGroup.DOFade(1f, 0.5f))
                .Join(DOTween.To(
                    () => TMP_DoctorText.maxVisibleCharacters,
                    x => TMP_DoctorText.maxVisibleCharacters = x,
                    DoctorText1.Length,
                    DoctorTextFadeTime1
                ))
                .Join(LeftButton.DOFade(1f, 0.5f))
                .Join(RightButton.DOFade(1f, 0.5f))
                .Append(DoctorCanvasGroup.DOFade(0f, 0.5f))
                // 恢复移动速度
                .Join(DOTween.To(
                    () => PlayerController.maxSpeed,
                    x => PlayerController.maxSpeed = x,
                    _initMoveSpeed,
                    5.0f
                ))
                .SetId(this);

            bool leftButtonClicked = false;
            bool rightButtonClicked = false;
            while (leftButtonClicked == false || rightButtonClicked == false)
            {
                if (InputWarp.LeftMoveDown())
                {
                    leftButtonClicked = true;
                    LeftButton.DOFade(0f, 0.5f).SetId(this);
                }

                if (InputWarp.RightMoveDown())
                {
                    rightButtonClicked = true;
                    RightButton.DOFade(0f, 0.5f).SetId(this);
                }

                yield return new WaitForNextFrameUnit();
            }

            while (DOTween.IsTweening(this))
            {
                // 等待博士对话结束
                yield return new WaitForNextFrameUnit();
            }

            StoryController.TryFinishCurrentStory();
        }

        /// <summary>
        ///  剧情3 结束当前场景
        /// </summary>
        public void FinishAnimation()
        {
            StartCoroutine(FinishAnimationCoroutine());
        }
        
        public IEnumerator FinishAnimationCoroutine()
        {
            StoryController.SetDebugText("结束场景动画");
            TMP_DoctorText.text = DoctorText2;
            TMP_DoctorText.maxVisibleCharacters = 0;
            DOTween.Sequence()
                .Append(DOTween.To(
                    () => TMP_DoctorText.maxVisibleCharacters,
                    x => TMP_DoctorText.maxVisibleCharacters = x,
                    DoctorText2.Length,
                    DoctorTextFadeTime2
                ))
                .Join(DoctorCanvasGroup.DOFade(1f, 0.5f))
                .AppendInterval(0.5f)
                .OnComplete(() =>
                {
                    StoryController.SetDebugText("结束当前场景");
                })
                .SetId(this);
            
            while (DOTween.IsTweening(this))
            {
                // 等待博士对话结束
                yield return new WaitForNextFrameUnit();
            }

            StoryController.TryFinishCurrentStory();
        }
    }
}