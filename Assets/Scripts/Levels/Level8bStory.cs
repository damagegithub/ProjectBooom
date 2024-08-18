using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;

public class Level8bStory : MonoBehaviour
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

        Invoke(nameof(StartConversation), 2f);
    }
    
    
    private void StartConversation()
    {
        dialogueController.StartConversation(803);
        dialogueController.OnOneConversationEnd += (int id) =>
        {
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
        MetaGameUtil.CreateFolderOnDesktop();
        MetaGameUtil.CreateFileOnDesktop("01.txt", "01本体");
        MetaGameUtil.CreateFileOnDesktop("02.txt", "02本体");
        ShowDesktop.ShowDesktopFunc();
        PlayerPrefs.SetInt("CurrentLevel", 7);
        Invoke(nameof(EndGame), 2f);
    }

    void EndGame()
    {
        MetaGameUtil.CloseGame();
    }

    void Update()
    {
        
    }
}
