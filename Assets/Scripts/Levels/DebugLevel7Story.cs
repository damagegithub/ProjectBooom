using System.Collections;
using System.Collections.Generic;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;

public class DebugLevel7Story : MonoBehaviour
{
    public DialogueController dialogueController;

    private enum DebugLevle7State
    {
        Conversation,
        CreateMetaGame
    }
    void Start()
    {
        dialogueController.StartConversation(101);
        dialogueController.OnOneConversationEnd += (int id) =>
        {
            Debug.Log("Conversation ended "+ id);
            StartCoroutine(ExecuteMetaAfterDelay(2));
        };
    }
    
    
    IEnumerator ExecuteMetaAfterDelay(float delay)
    {
        // 等待指定的时间
        yield return new WaitForSeconds(delay);
        
        // 延迟后执行代码
        MetaGame();
    }

    
    void MetaGame()
    {
        Debug.Log("MetaGame ");
        MetaGameUtil.CreateFolderOnDesktop("GameInfo");
        MetaGameUtil.CreateFileOnDesktop("GameInfo", "01.txt", "01本体");
        MetaGameUtil.CreateFileOnDesktop("GameInfo", "02.txt", "02本体");
        ShowDesktop.ShowDesktopFunc();
        Invoke(nameof(EndGame), 2f);
    }

    void EndGame()
    {
        Debug.Log("CloseGame ");
        PlayerPrefs.SetInt("LevelDebug7FileCreated",1);
        MetaGameUtil.CloseGame();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
