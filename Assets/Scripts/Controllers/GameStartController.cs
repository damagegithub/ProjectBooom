using System.Collections;
using System.Collections.Generic;
using MetaGameUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public Button GameStartButton;
    public Button LevelDebug7Button;

    void Start()
    {
        
        
        Level7Check();
        
        GameStartButton.onClick.AddListener(() =>
        {
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

            if (currentLevel != 0 || PlayerPrefs.GetInt("LevelDebug7FileCreated", -1) == 1)
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


    private void Level7Check()
    {
        if (PlayerPrefs.GetInt("Level7MetaCreated", -1) == 1)
        {
            bool Has02 = MetaGameUtil.CheckPlayerDesktopHasFile("GameInfo", "02.txt");
            bool Has01 = MetaGameUtil.CheckPlayerDesktopHasFile("GameInfo", "01.txt");
            if (!Has02 && Has01)
            {
                //正确 跳转到8a
                SceneManager.LoadScene("_8_真.培养室");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else if (Has02 && Has01)
            {
                var startGameTimes = PlayerPrefs.GetInt("Level7MetaDoNothing", 1);
                SceneManager.LoadScene("_7.培养室");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else
            {
                PlayerPrefs.SetInt("CurrentLevel", 8);
                SceneManager.LoadScene("_8_伪.黑幕");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
        }
    }

    void Update()
    {
    }
}