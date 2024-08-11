using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    
    public Button GameStartButton;
    public Button LevelDebug7Button;
    void Start()
    {
        GameStartButton.onClick.AddListener(() =>
        {
            var CurrentLevel = PlayerPrefs.GetInt("CurrentLevel",0);
            
            if(CurrentLevel != 0 || PlayerPrefs.GetInt("LevelDebug7FileCreated" ,-1) ==1)
            {
                //完成关卡1之后, 进入选人界面
                SceneManager.LoadScene("SelectCharacterScene");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else
            {
                //完成关卡1之前, 没有选人界面
                SceneManager.LoadScene("_1.培养室");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
               
            }
        });


        LevelDebug7Button.onClick.AddListener(() =>
        {
                SceneManager.LoadScene("DebugScene_Level7");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
        });
    }

    void Update()
    {
        
    }
}
