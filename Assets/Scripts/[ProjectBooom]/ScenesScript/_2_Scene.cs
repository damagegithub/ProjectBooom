using System.Collections;
using _ProjectBooom_.DataStruct;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.UI;
using _ProjectBooom_.PuzzleMono.UI._2;
using DG.Tweening;
using PBDialogueSystem;
using UnityEngine;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    ///     走道
    /// </summary>
    public class _2_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        public CaptchaControl CaptchaControl;

        [Header("AVG控制器")]
        public DialogueController DialogueController;

        [Header("全屏遮挡画布")]
        public CanvasGroup BlackCanvasGroup;

        [Header("博士对话脚本")]
        public DoctorSpeakController DoctorSpeakController;

        [Header("验证码间隔时间")]
        public float CaptchaIntervalTime = 3.0f;

        [Header("场景开场对话ID")]
        public int LevelStartDialogIndex;

        [Header("场景结束对话ID")]
        public int LevelEndDialogIndex;

        [Header("进行中的对话ID")]
        public int CurrentDialogIndex = -1;

        [SerializeField]
        [Header("验证码信息")]
        public CaptchaInfo[] CaptchaInfos;

        public void DialogFinish(int dialogIndex)
        {
            if (CurrentDialogIndex == dialogIndex)
            {
                CurrentDialogIndex = -1;
            }
        }

        private void Awake()
        {
            if (!DialogueController)
            {
                DialogueController = FindObjectOfType<DialogueController>(true);
            }

            if (!DoctorSpeakController)
            {
                DoctorSpeakController = FindObjectOfType<DoctorSpeakController>(true);
            }

            DialogueController.OnOneConversationEnd += DialogFinish;

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
        ///     开始验证码测试
        /// </summary>
        public void StartCaptchaTest()
        {
            StartCoroutine(StartCaptchaTestCoroutine());
        }

        public IEnumerator StartCaptchaTestCoroutine()
        {
            // 等待触发验证码
            while (Mathf.Approximately(0f, GlobalVariable.GetVarValue("开启验证码")))
            {
                yield return new WaitForEndOfFrame();
            }
            
            // 等待指定的时间
            for (int i = 0; i < CaptchaInfos.Length; i++)
            {
                StoryController.SetDebugText($"当前第{i+1}个验证码");
                CaptchaControl.Show(CaptchaInfos[i]);
                while (!CaptchaControl.IsAnswered)
                {
                    yield return new WaitForEndOfFrame();
                }

                // 等待指定的时间
                yield return new WaitForSeconds(CaptchaIntervalTime);
            }

            // 进入下个片段
            StoryController.TryFinishCurrentStory();
        }
    }
}