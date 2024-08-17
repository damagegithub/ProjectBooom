using System.Collections;
using DG.Tweening;
using PBDialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public class Level8A2Story : MonoBehaviour
    {
        public DialogueController dialogueController;

        
        
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
            
            Invoke(nameof(StartConversation802), 2f);
        }
        

        private void StartConversation802()
        {

            dialogueController.StartConversation(802);
            dialogueController.OnOneConversationEnd += (int id) =>
            {
             
                Invoke(nameof(FinishLevel8), 2f);
            };
        }

        private void FinishLevel8()
        {
            PlayerPrefs.SetInt("CurrentLevel", 9);
            SceneManager.LoadScene("SelectCharacterScene");
            SceneManager.UnloadSceneAsync("_8A2");
        }


        // Update is called once per frame
        void Update()
        {
        
        }
    }
}