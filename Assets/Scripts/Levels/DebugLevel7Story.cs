using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;

public class DebugLevel7Story : MonoBehaviour
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
        if (PlayerPrefs.GetInt("Level7MetaCreated", -1) == -1)
        {
            //第一次进level7 meta流程
            Invoke(nameof(StartConversation), 0.1f);
        }else if (PlayerPrefs.GetInt("Level7MetaCreated", -1) == 1)
        {
            //meta操作不正确
            Invoke(nameof(StartConversationRepeat), 0.1f);
        }
        yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();
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
        dialogueController.StartConversation(702);
        dialogueController.OnOneConversationEnd += (int id) =>
        {
            Debug.Log("Conversation ended "+ id);
            ShowDesktop.ShowDesktopFunc();
            Invoke(nameof(EndGame), 2f);
        };
    }
    
    private void StartConversation703()
    {
        Debug.Log("StartConversation703");
        dialogueController.StartConversation(703);
        dialogueController.OnOneConversationEnd += (int id) =>
        {
            Debug.Log("Conversation ended "+ id);
            ShowDesktop.ShowDesktopFunc();
            Invoke(nameof(EndGame), 2f);
        };
    }
    
    private void StartConversation()
    {
        dialogueController.StartConversation(701);
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
        MetaGameUtil.CreateFolderOnDesktop();
        MetaGameUtil.CreateFileOnDesktop("01.txt", "01本体");
        MetaGameUtil.CreateFileOnDesktop("02.txt", "02本体");
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
