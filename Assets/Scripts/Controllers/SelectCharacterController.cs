using System.Collections;
using System.Collections.Generic;
using MetaGameUtils;
using PBDialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class SelectCharacterController : MonoBehaviour
{
    //选人界面控制器

    public GameObject CharacterPreab;       //预制体
    public Transform  CharacterPanelParent; //角色父panel

    public GameObject SelectShowPanel;


    private List<DialogueActor>            _actors      = new List<DialogueActor>();            //角色列表
    private Dictionary<int, List<Vector2>> PosMap       = new Dictionary<int, List<Vector2>>(); //角色位置映射表
    private Dictionary<int, List<Vector2>> SpritePosMap = new Dictionary<int, List<Vector2>>(); //角色位置映射表

    private List<int> GetCanSelectedCharacters()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", -1) == 10)
        {
            new List<int>() { 1, 2, 3 };
        }

        return new List<int>() { 1, 2 };
    }

    private List<int> GetUsableCharacters()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", -1) == 10)
        {
            new List<int>() { 1 };
        }

        return new List<int>() { 1 };
    }


    // Start is called before the first frame update
    void Start()
    {
        PosMap.Add(2, new List<Vector2>() { new Vector2(-500, 0), new Vector2(500, 0) });
        PosMap.Add(3, new List<Vector2>() { new Vector2(-400, 0), new Vector2(0, 0), new Vector2(400, 0) });
        SpritePosMap.Add(2, new List<Vector2>() { new Vector2(-2, 0), new Vector2(2, 0) });
        SpritePosMap.Add(3, new List<Vector2>() { new Vector2(-3, 0), new Vector2(0, 0), new Vector2(3, 0) });

        List<DialogueActor> Actors = CSVToJsonUtil.GetJsonData<DialogueActor>("Tables/Actors");
        foreach (var Actor in Actors)
        {
            _actors.Add(Actor);
        }


        var CanSelectedCharacters = GetCanSelectedCharacters();
        var index = 0;
        foreach (var characterID in CanSelectedCharacters)
        {
            GameObject obj = Instantiate(CharacterPreab, CharacterPanelParent);
            //设置父
            obj.transform.SetParent(CharacterPanelParent, false);
            obj.transform.localPosition = PosMap[CanSelectedCharacters.Count][index];
            foreach (DialogueActor actor in _actors)
            {
                if (actor.ActorID == characterID)
                {
                    var SpriteImage = obj.GetComponentInChildren<SpriteRenderer>();
                    SpriteImage.sprite = Resources.Load<Sprite>(actor.SelectSceneImagePath);
                    SpriteImage.transform.position = SpritePosMap[CanSelectedCharacters.Count][index];
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = actor.ActorName;
                    var light = obj.GetComponentInChildren<Light2D>();
                    light.transform.position = SpritePosMap[CanSelectedCharacters.Count][index] + new Vector2(0, 8);
                    light.gameObject.SetActive(false);
                    if (GetUsableCharacters().Contains(characterID))
                    {
                        light.gameObject.SetActive(true);
                        var Handle = obj.GetComponentInChildren<SpriteClickHandle>();
                        
                        Handle.SelectShowObj = SelectShowPanel;
                        Handle.CharacterPanelParent = CharacterPanelParent;
                        Handle.Actor = actor;
                        Handle.SelectCharacterController = this;
                    }

                    index++;
                }
            }
        }
    }

    public void JumpToGame()
    {
        SceneManager.LoadScene("_1.培养室");
        SceneManager.UnloadSceneAsync("SelectCharacterScene");
        if (PlayerPrefs.GetInt("CurrentLevel", -1) == 8)
        {
            SceneManager.LoadScene("_8A");
            SceneManager.UnloadSceneAsync("SelectCharacterScene");
        }
        else if (PlayerPrefs.GetInt("CurrentLevel", -1) == 9)
        {
            SceneManager.LoadScene("_9");
            SceneManager.UnloadSceneAsync("SelectCharacterScene");
        }
        else if (PlayerPrefs.GetInt("CurrentLevel", -1) == 10)
        {
            SceneManager.LoadScene("_10A");
            SceneManager.UnloadSceneAsync("SelectCharacterScene");
        }
        else if (PlayerPrefs.GetInt("CurrentLevel", -1) == 11)
        {
            SceneManager.LoadScene("_11");
            SceneManager.UnloadSceneAsync("SelectCharacterScene");
        }
    }


    // Update is called once per frame
    void Update() { }
}