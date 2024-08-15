using System.Collections;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.CharacterAction;
using _ProjectBooom_.PuzzleMono.UI;
using Controllers;
using DG.Tweening;
using PBDialogueSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    /// 小巷
    /// </summary>
    public class _6_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        [Header("左上角对话")]
        public DoctorSpeakController DoctorSpeakController;

        [Header("全屏遮挡画布")]
        public CanvasGroup BlackCanvasGroup;

        [Header("AVG控制器")]
        public DialogueController DialogueController;

        [Header("对话ID")]
        public int DialogIndex0;
        public int DialogIndex1;

        [Header("进行中的对话ID")]
        public int CurrentDialogIndex = -1;

        [Header("射灯灯光")]
        public GameObject smoothLight;

        public string tip0 = "02：钥匙已经托人放在了柜子里头……";
        public string tip1 = "门是锁着的";
        public string tip2 = "电源未接通";
        public string tip3 = "门已打开, 这样路就打通了";
        public string tip4 = "电路已连接";

        public SpriteRenderer BackgroundNormal;
        public SpriteRenderer BackgroundSpotLight;
        public SpriteRenderer BackgroundSpotLightAndOpenCabinet;
        public SpriteRenderer BackgroundSpotLightAndOpenCabinetAndOpenDoor;

        public SetVarNearestAction lockAction1;
        public SetVarNearestAction lockAction2;
        public SetVarNearestAction lockAction3;

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
            
            smoothLight.SetActive(false);
            BackgroundNormal.DOFade(1f, 0f).SetId(this);
            BackgroundSpotLight.DOFade(0f, 0f).SetId(this);
            BackgroundSpotLightAndOpenCabinet.DOFade(0f, 0f).SetId(this);
            BackgroundSpotLightAndOpenCabinetAndOpenDoor.DOFade(0f, 0f).SetId(this);
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
            const string lock1 = "场景6Trigger1";
            const string lock2 = "场景6Trigger2";
            const string lock3 = "场景6Trigger3";

            DoctorSpeakController.SpeakWithoutFade(tip0,true);

            // 等待其中一个触发
            while (GlobalVariable.GetVarValue(lock1) == 0
                || GlobalVariable.GetVarValue(lock2) == 0
                || GlobalVariable.GetVarValue(lock3) == 0)
            {
                yield return new WaitForEndOfFrame();
                if (GlobalVariable.GetVarValue(lock3) != 0)
                {
                    // 通过
                    lockAction3.gameObject.SetActive(false);
                    break;
                }

                // 门
                if (GlobalVariable.GetVarValue(lock1) != 0)
                {
                    GlobalVariable.SetVarValue(lock1, 0);
                    DoctorSpeakController.SpeakWithoutFade(tip1,true);
                }
                // 柜子
                else if (GlobalVariable.GetVarValue(lock2) != 0)
                {
                    GlobalVariable.SetVarValue(lock2, 0);
                    DoctorSpeakController.SpeakWithoutFade(tip2,true);
                }
            }
            
            // 连接电源
            smoothLight.gameObject.SetActive(true);
            DoctorSpeakController.SpeakWithoutFade(tip4,true);
            BackgroundSpotLight.DOFade(1f, 1f).SetId(this);
            BackgroundNormal.DOFade(0f, 1f).SetId(this);
            
            // 等待其中一个触发
            while (GlobalVariable.GetVarValue(lock1) == 0
                || GlobalVariable.GetVarValue(lock2) == 0)
            {
                yield return new WaitForEndOfFrame();
                if (GlobalVariable.GetVarValue(lock2) != 0)
                {
                    // 通过
                    lockAction2.gameObject.SetActive(false);
                    break;
                }

                // 门
                if (GlobalVariable.GetVarValue(lock1) != 0)
                {
                    GlobalVariable.SetVarValue(lock1, 0);
                    DoctorSpeakController.SpeakWithoutFade(tip1,true);
                }
            }
            
            // 获得了钥匙
            DoctorSpeakController.Speak("获得了钥匙");
            BackgroundSpotLightAndOpenCabinet.DOFade(1f, 1f).SetId(this);
            BackgroundSpotLight.DOFade(0f, 1f).SetId(this);
            
            // 等待其中一个触发
            while (GlobalVariable.GetVarValue(lock1) == 0)
            {
                yield return new WaitForEndOfFrame();
            }
            lockAction1.gameObject.SetActive(false);
            
            // 打开了门
            DoctorSpeakController.SpeakWithoutFade(tip3);
            BackgroundSpotLightAndOpenCabinetAndOpenDoor.DOFade(1f, 1f).SetId(this);
            BackgroundSpotLightAndOpenCabinet.DOFade(0f, 1f).SetId(this);

            // 等待场景变化完成
            while (DOTween.IsTweening(this))
            {
                yield return new WaitForEndOfFrame();
            }

            yield return StartAVGSystemCoroutine(DialogIndex1, true);

            StoryController.TryFinishCurrentStory();
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