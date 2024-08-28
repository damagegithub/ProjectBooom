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
        StartConversation();
        yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();
    }
    
    
    private void StartConversation()
    {
        dialogueController.StartConversation(803);
        dialogueController.OnOneConversationEnd += (int id) =>
        {
            MetaGame();
        };
    }
    
    void MetaGame()
    {
        MetaGameUtil.CreateFolderOnDesktop();
        MetaGameUtil.CreateFileOnDesktop("01.txt", MetaGameUtils.GlobalString._01TxtContent);
        MetaGameUtil.CreateFileOnDesktop("02.txt", MetaGameUtils.GlobalString._02TxtContent);
        ShowDesktop.ShowDesktopFunc();
        PlayerPrefs.SetInt("CurrentLevel", 7);
        EndGame();
    }

    void EndGame()
    {
        MetaGameUtil.CloseGame();
    }

    void Update()
    {
        
    }
}
