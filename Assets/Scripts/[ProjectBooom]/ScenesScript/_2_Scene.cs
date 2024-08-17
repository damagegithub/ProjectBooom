using System.Collections;
using _ProjectBooom_.DataStruct;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.UI;
using _ProjectBooom_.PuzzleMono.UI._2;
using Controllers;
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
        public PlayerController PlayerController;

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

        [Header("对话ID")]
        public int DialogID0 = 201;
        public int DialogID1 = 202;
        public int DialogID2 = 203;
        public int DialogID3 = 204;
        public int DialogID4 = 205;
        public int DialogID5 = 206;

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
            if (!PlayerController)
            {
                PlayerController = FindObjectOfType<PlayerController>(true);
            }

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
        
        public void SingleStory()
        {
            StartCoroutine(SingleStoryCoroutine());
        }
        
        private IEnumerator SingleStoryCoroutine()
        {
            StoryController.SetDebugText("等待场景开始AVG对话");
            yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();
            // 控制移动
            yield return new WaitUntil(() => Mathf.Approximately(1f, GlobalVariable.GetVarValue("场景2开启对话")));
            // 停止移动并开启对话
            PlayerController.maxSpeed = 0;
            yield return StartAVGSystemCoroutine(DialogID0,false,false);
            
            // 测试验证码
            StoryController.SetDebugText($"当前第1个验证码");
            CaptchaControl.Show(CaptchaInfos[0]);
            yield return new WaitUntil(() => CaptchaControl.IsAnswered);
            yield return StartAVGSystemCoroutine(DialogID1,false,false);
            StoryController.SetDebugText($"当前第2个验证码");
            CaptchaControl.Show(CaptchaInfos[1]);
            yield return new WaitUntil(() => CaptchaControl.IsAnswered);
            yield return StartAVGSystemCoroutine(DialogID2,false,false);
            StoryController.SetDebugText($"当前第3个验证码");
            CaptchaControl.Show(CaptchaInfos[2]);
            yield return new WaitUntil(() => CaptchaControl.IsAnswered);
            yield return StartAVGSystemCoroutine(DialogID3,false,false);
            StoryController.SetDebugText($"当前第4个验证码");
            CaptchaControl.Show(CaptchaInfos[3]);
            yield return new WaitUntil(() => CaptchaControl.IsAnswered);
            yield return StartAVGSystemCoroutine(DialogID4,false,false);

            yield return StartAVGSystemCoroutine(DialogID5,true);
        }

        private IEnumerator StartAVGSystemCoroutine(int dialogIndex, bool fadeEnd = false, bool autoStory = true)
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

            if (autoStory)
            {
                StoryController.TryFinishCurrentStory();
            }
        }

    }
}