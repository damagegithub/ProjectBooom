using System.Collections;
using System.Collections.Generic;
using PBDialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class SpriteClickHandle : MonoBehaviour
{
    public Transform CharacterPanelParent;
    public GameObject SelectShowObj;
    public DialogueActor Actor;
    public SelectCharacterController SelectCharacterController;
    
    void OnMouseDown()
    {
        Debug.Log("Sprite clicked!");
        // 在这里处理点击事件，例如触发动画或改变场景
        
        CharacterPanelParent.gameObject.SetActive(false);
        SelectShowObj.SetActive(true);
        var actor = SelectShowObj.transform.Find("Actor");
        actor.GetComponent<Image>().sprite = Resources.Load<Sprite>(Actor.SelectFBImagePath);
        actor.GetComponent<Image>().SetNativeSize();
        var Name = SelectShowObj.transform.Find("Name");
        Name.transform.Find("Name2").GetComponent<Image>().sprite = Resources.Load<Sprite>(Actor.SelectWord);
        Name.transform.Find("Name1").GetComponent<Image>().sprite = Resources.Load<Sprite>(Actor.SelectWord);
        Invoke(nameof(JumpToGame), 3f);
    }

    void JumpToGame()
    {
        SelectCharacterController.JumpToGame();
    }
}
    

