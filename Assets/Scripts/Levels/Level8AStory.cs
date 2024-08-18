using System.Collections;
using _ProjectBooom_.PuzzleMono.UI;
using DG.Tweening;
using PBDialogueSystem;
using UnityEngine;

namespace Levels
{
    public class Level8AStory : MonoBehaviour
    {
        public DialogueController dialogueController;
        [Header("全屏遮挡画布")]
        public CanvasGroup BlackCanvasGroup;
        [Header("博士对话脚本")]
        public DoctorSpeakController DoctorSpeakController;
        
        private void Awake()
        {
            BlackCanvasGroup.alpha = 1;
        }
        
        void Start()
        {
           
            StartCoroutine(ScriptStart());
        }

        public IEnumerator ScriptStart()
        {
            StartConversation801();
            yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();
        }

        private void StartConversation801()
        {

            dialogueController.StartConversation(801);
            dialogueController.OnOneConversationEnd += (int id) =>
            {
                Debug.Log("Conversation ended " + id);
                DoctorSpeakController.gameObject.SetActive(true);
                DoctorSpeakController.SpeakWithoutFade("必须马上离开这里！");
                dialogueController.ClearOnOneConversationEnd(); 
            };
        }
        
    }
}