using System.Collections;
using System.Collections.Generic;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;

public class DebugLevel7Story : MonoBehaviour
{
    public DialogueController dialogueController;


    void Start()
    {
        if (PlayerPrefs.GetInt("Level7MetaCreated", -1) == -1)
        {
            //第一次进level7 meta流程
            Invoke(nameof(StartConversation), 2f);
        }else if (PlayerPrefs.GetInt("Level7MetaCreated", -1) == 1)
        {
            //meta操作不正确
            Invoke(nameof(StartConversationRepeat), 1f);
        }
       
    }

    private void StartConversationRepeat()
    {
        var startGameTimes = PlayerPrefs.GetInt("Level7MetaDoNothing", 1);
        PlayerPrefs.SetInt("Level7MetaDoNothing", startGameTimes + 1);
        if (startGameTimes < 3)
        {
            StartConversation702();
        }
        else
        {
            StartConversation703();
        }
    }
    
    private void StartConversation702()
    {
        Debug.Log("StartConversation702");
        dialogueController.StartConversation(101);//todo 702
        dialogueController.OnOneConversationEnd += (int id) =>
        {
            Debug.Log("Conversation ended "+ id);
            Invoke(nameof(EndGame), 2f);
        };
    }
    
    private void StartConversation703()
    {
        Debug.Log("StartConversation703");
        dialogueController.StartConversation(101);//todo 703
        dialogueController.OnOneConversationEnd += (int id) =>
        {
            Debug.Log("Conversation ended "+ id);
            Invoke(nameof(EndGame), 2f);
        };
    }
    
    private void StartConversation()
    {
        dialogueController.StartConversation(101);//todo 701
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
        PlayerPrefs.SetInt("Level7MetaCreated",1);
        MetaGameUtil.CloseGame();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
