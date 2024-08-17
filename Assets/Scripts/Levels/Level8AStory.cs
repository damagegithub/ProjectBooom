using System.Collections;
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
            yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();
            Invoke(nameof(StartConversation801), 2f);
        }

        private void StartConversation801()
        {

            dialogueController.StartConversation(101);//todo 801
            dialogueController.OnOneConversationEnd += (int id) =>
            {
                Debug.Log("Conversation ended " + id);
            
                //todo 加解密小游戏
                Invoke(nameof(StartConversation802), 2f);
            };
        }
    
        private void StartConversation802()
        {
            dialogueController.ClearOnOneConversationEnd(); 
            dialogueController.StartConversation(101);//todo 802
            dialogueController.OnOneConversationEnd += (int id) =>
            {
                Debug.Log("Conversation ended " + id);
                dialogueController.ClearOnOneConversationEnd(); 
                // StartCoroutine(ExecuteMetaAfterDelay(2));
            };
        }
    

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}