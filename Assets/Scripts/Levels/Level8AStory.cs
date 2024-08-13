using PBDialogueSystem;
using UnityEngine;

namespace Levels
{
    public class Level8AStory : MonoBehaviour
    {
        public DialogueController dialogueController;

        // Start is called before the first frame update
        void Start()
        {
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