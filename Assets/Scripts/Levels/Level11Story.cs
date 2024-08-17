using System.Collections;
using DG.Tweening;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;

namespace Levels
{
    public class Level11Story : MonoBehaviour
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
            if (PlayerPrefs.GetInt("Level11MetaCreated", -1) == 1)
            {
                dialogueController.StartConversation(1102); 
                dialogueController.OnOneConversationEnd += (int id) => { Invoke(nameof(EndGame), 2f); };
            }
            else if (PlayerPrefs.GetInt("Level11MetaDeleteDoc", -1) == 1)
            {
                dialogueController.StartConversation(1103); 
                dialogueController.OnOneConversationEnd += (int id) =>
                {
                    //todo 播放结局 然后
                    PlayerPrefs.SetInt("GameFinished", 1);
                    PlayerPrefs.SetInt("CurrentLevel", 999); 
                    Invoke(nameof(EndGame), 2f);
                };
            }
            else
            {
                dialogueController.StartConversation(1101); 
                dialogueController.OnOneConversationEnd += (int id) =>
                {
                    Debug.Log("Conversation ended " + id);
                    StartCoroutine(ExecuteMetaAfterDelay(2));
                };
            }
        }

        IEnumerator ExecuteMetaAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            MetaGame();
        }


        void MetaGame()
        {
            var FloderName = "PB_Meta";
            MetaGameUtil.CreateFolderOnDesktop(FloderName);
            MetaGameUtil.CreateFileOnDesktop(FloderName, "01.txt", "01本体");
            MetaGameUtil.CreateFileOnDesktop(FloderName, "doc.txt", "doc本体");
            ShowDesktop.ShowDesktopFunc();
            PlayerPrefs.SetInt("Level11MetaCreated", 1);
            Invoke(nameof(EndGame), 2f);
        }

        void EndGame()
        {
            MetaGameUtil.CloseGame();
        }

        // Update is called once per frame
        void Update() { }
    }
}