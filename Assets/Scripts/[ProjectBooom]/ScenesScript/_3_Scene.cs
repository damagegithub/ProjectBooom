using System.Collections;
using System.Collections.Generic;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.UI;
using _ProjectBooom_.PuzzleMono.UI._3;
using Cyan;
using DG.Tweening;
using PBDialogueSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace _ProjectBooom_.ScenesScript
{
    /// <summary>
    ///     走道会议室
    /// </summary>
    public class _3_Scene : MonoBehaviour
    {
        public StoryController StoryController;

        [Header("全屏遮挡画布")] public CanvasGroup BlackCanvasGroup;

        [Header("博士对话脚本")] public DoctorSpeakController DoctorSpeakController;

        [Header("AVG控制器")]
        public DialogueController DialogueController;

        [Header("场景开场对话ID")]
        public int LevelStartDialogIndex;

        [Header("场景结束对话ID")]
        public int LevelEndDialogIndex;

        [Header("场景结束对话ID2")]
        public int LevelEndDialogIndex2;

        [Header("进行中的对话ID")]
        public int CurrentDialogIndex = -1;

        [Header("文件接收处")]
        public FoldFileReceiveUI FoldFileReceiveUI;

        [Header("简陋电脑UI")]
        public RectTransform DesktopRectTrans;

        [Header("电脑画布")]
        public CanvasGroup DesktopCanvasGroup;

        [Header("打开电脑前的博士对话")]
        public List<string> PreActionTexts;

        [Header("打开电脑时的博士对话")]
        public List<string> InActionTexts;

        [Header("需要删除的文件数量")]
        public int DeleteFileCount = 2;

        [Header("删除完成通知UI")]
        public CanvasGroup DeleteComputedCanvasGroup;

        [Header("删除完成等待时间")]
        public float DeleteComputedWaitSeconds = 3f;

        [Header("会议室场景名")]
        public string ConferenceRoomScene;

        [Header("默认渲染管线")]
        public Renderer2DData Renderer2DData;
        [Header("相机噪音Blit")]
        public string CameraNoiseBlit = "CameraNoise";
        [Header("相机色散Blit")]
        public string CameraColorDispersionBlit = "CameraColorDispersion";

        private  ScriptableRendererFeature _cameraNoiseData;
        private  ScriptableRendererFeature _cameraColorDispersionData;

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
            DesktopRectTrans.localScale = Vector3.zero;
            DesktopCanvasGroup.alpha = 1;
            DesktopCanvasGroup.interactable = true;
            DesktopCanvasGroup.blocksRaycasts = true;
            DesktopCanvasGroup.gameObject.SetActive(true);
            

            _cameraNoiseData = Renderer2DData
                              .rendererFeatures
                              .Find(srf => srf.name.Equals(CameraNoiseBlit));
            _cameraColorDispersionData = Renderer2DData
                                        .rendererFeatures
                                        .Find(srf => srf.name.Equals(CameraColorDispersionBlit));
            _cameraNoiseData.SetActive(false);
            _cameraColorDispersionData.SetActive(false);
        }

        /// <summary>
        ///     场景开始的对话
        /// </summary>
        public void LevelBeginAvgDialog()
        {
            StoryController.SetDebugText("场景开始AVG对话");
            StartCoroutine(StartAVGSystemCoroutine(LevelStartDialogIndex));
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

        public void StartComputeAction()
        {
            StartCoroutine(StartComputeActionCoroutine());
        }

        public IEnumerator StartComputeActionCoroutine()
        {
            DeleteComputedCanvasGroup.alpha = 0;
            yield return new WaitForEndOfFrame();
            yield return DoctorSpeakController
               .SpeakAndWait("博士：接下来需要为会议做准备，我需要你的帮助。", true);
            yield return DoctorSpeakController
               .SpeakAndWait("博士：请帮我删除会议文件夹中的文件", true);
            DoctorSpeakController.SpeakWithoutFade("博士：先去启动电脑吧。");

            // 等待触发
            while (Mathf.Approximately(0f, GlobalVariable.GetVarValue("打开电脑")))
            {
                yield return new WaitForEndOfFrame();
            }

            // 打开电脑
            yield return DOTween.Sequence()
                                .Append(DesktopCanvasGroup.DOFade(0f, 0f))
                                .Join(DesktopRectTrans.DOScale(Vector3.one, 0f))
                                .Append(DesktopCanvasGroup.DOFade(1f, 1f))
                                .JoinCallback(() =>
                                 {
                                     _cameraNoiseData.SetActive(true);
                                     _cameraColorDispersionData.SetActive(true);
                                 })
                                .SetEase(Ease.Linear)
                                .SetId(this)
                                .WaitForCompletion();

            yield return DoctorSpeakController
               .SpeakAndWait("博士：桌上的代理人数据库，冗余的文件就在其中。", true);

            float waitTime = 5f;
            // 等待完成
            while (FoldFileReceiveUI.FileUIs.Count < DeleteFileCount)
            {
                yield return new WaitForEndOfFrame();
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    waitTime = 5f;
                    DoctorSpeakController
                       .Speak("博士：这是两份完全相同的文件，选择其一拖到回收站删除即可。", true);
                }
            }

            // 删除完成
            DeleteComputedCanvasGroup.alpha = 1;
            // 对话结束等待3秒返回
            yield return new WaitForSeconds(DeleteComputedWaitSeconds);
            // DesktopRectTrans.DOScale(Vector3.zero, 1f).SetId(this);
            yield return DOTween.Sequence()
                                .Append(DesktopCanvasGroup.DOFade(0f, 1f))
                                .JoinCallback(() =>
                                 {
                                     _cameraNoiseData.SetActive(false);
                                     _cameraColorDispersionData.SetActive(false);
                                 })
                                .SetEase(Ease.Linear)
                                .SetId(this)
                                .WaitForCompletion();
            // 302 对话
            yield return StartAVGSystemCoroutine(LevelEndDialogIndex, false, false);
            DoctorSpeakController.SpeakWithoutFade("继续向左进入会议室");

            // 进入会议室 等待触发
            while (Mathf.Approximately(0f, GlobalVariable.GetVarValue("进入会议室")))
            {
                yield return new WaitForEndOfFrame();
            }

            while (DOTween.IsTweening(this))
            {
                yield return new WaitForEndOfFrame();
            }

            SceneManager.LoadScene(ConferenceRoomScene);
        }
    }
}