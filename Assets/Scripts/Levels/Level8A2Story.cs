using PBDialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public class Level8A2Story : MonoBehaviour
    {
        public DialogueController dialogueController;

        // Start is called before the first frame update
        void Start()
        {
            Invoke(nameof(StartConversation803), 2f);
        
        }

        private void StartConversation803()
        {

            dialogueController.StartConversation(101);//todo 803
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