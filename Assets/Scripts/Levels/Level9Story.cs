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
        var MetaDialog = PlayerPrefs.GetInt("levle9MetaDialog", -1);
        if (MetaDialog != -1)
        {
            Debug.Log("StartConversation---"+MetaDialog);
            PlayerPrefs.SetInt("levle9MetaDialog", -1);
            dialogueController.StartConversation(MetaDialog);
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
        MetaGameUtil.CreateFolderOnDesktop();
        MetaGameUtil.CreateFileOnDesktop("01.txt", "01本体");
        if (!MetaGameUtil.CheckPlayerDesktopHasFile("lian.txt"))
        {
            MetaGameUtil.CreateFileOnDesktop("lian.xxx", "lian?????");
        }
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
