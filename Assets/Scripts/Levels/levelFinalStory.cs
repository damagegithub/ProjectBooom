using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MetaGameUtils;
using UnityEngine;

public class levelFinalStory : MonoBehaviour
{
    // Start is called before the first frame update
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
        Invoke(nameof(EndGame), 5f);
    }
    void EndGame()
    {
        MetaGameUtil.CloseGame();
    }
}
