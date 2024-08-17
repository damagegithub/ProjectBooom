using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MetaGameUtils;
using PBDialogueSystem;
using UnityEngine;
public class Level9Story : MonoBehaviour
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
        if (PlayerPrefs.GetInt("Level9MetaCreated", -1) == 1)
        {
            Debug.Log("StartConversation---902");
            dialogueController.StartConversation(902);
        }
        else
        {
            dialogueController.StartConversation(901);
        }
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
        
        MetaGame();
    }

    
    void MetaGame()
    {
        var FloderName = "PB_Meta";
        MetaGameUtil.CreateFolderOnDesktop(FloderName);
        MetaGameUtil.CreateFileOnDesktop(FloderName, "01.txt", "01本体");
        MetaGameUtil.CreateFileOnDesktop(FloderName, "lian.xxx", "lian?????");
        ShowDesktop.ShowDesktopFunc();
        PlayerPrefs.SetInt("Level9MetaCreated",1);
        Invoke(nameof(EndGame), 2f);
    }
    
    void EndGame()
    {
        MetaGameUtil.CloseGame();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
