using System.Collections;
using System.Collections.Generic;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level10AStory : MonoBehaviour
{
    public DialogueController dialogueController;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartConversation), 2f);
    }

    private void StartConversation()
    {

        dialogueController.StartConversation(101);//todo 1001
        dialogueController.OnOneConversationEnd += (int id) =>
        {
            Debug.Log("Conversation ended " + id);
            StartCoroutine(ExecuteMetaAfterDelay(2));
        };
    }
    
    IEnumerator ExecuteMetaAfterDelay(float delay)
    {
        // 等待指定的时间
        yield return new WaitForSeconds(delay);
        
        SceneManager.LoadScene("_10");
        SceneManager.UnloadSceneAsync("_10A");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
