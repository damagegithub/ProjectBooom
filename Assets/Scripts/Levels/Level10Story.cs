using System.Collections;
using System.Collections.Generic;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level10Story : MonoBehaviour
{
    public DialogueController dialogueController;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartConversation), 2f);
    }

    private void StartConversation()
    {

        dialogueController.StartConversation(101);//todo 1002
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
