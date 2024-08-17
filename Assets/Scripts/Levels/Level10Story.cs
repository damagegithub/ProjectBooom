using System.Collections;
using DG.Tweening;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;

namespace Levels
{
    public class Level10Story : MonoBehaviour
    {
        public DialogueController dialogueController;

        // Start is called before the first frame update
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

            Invoke(nameof(StartConversation), 2f);
        }

        private void StartConversation()
        {

            dialogueController.StartConversation(1002);
            dialogueController.OnOneConversationEnd += (int id) =>
            {
                Debug.Log("Conversation ended " + id);
                PlayerPrefs.SetInt("Level10Finished", 1);
                StartCoroutine(ExecuteMetaAfterDelay(2));
            };
        }
    
        IEnumerator ExecuteMetaAfterDelay(float delay)
        {
            // 等待指定的时间
            yield return new WaitForSeconds(delay);
        
            MetaGameUtil.CloseGame();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
