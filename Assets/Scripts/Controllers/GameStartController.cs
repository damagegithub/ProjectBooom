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
    public Button ClearPlayerPrefsButton;

    void Start()
    {
        Level7Check();
        Level9Check();
        Level11Check();

        GameStartButton.onClick.AddListener(() =>
        {
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

            if (currentLevel != 0)
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

        ClearPlayerPrefsButton.onClick.AddListener(() => { PlayerPrefs.DeleteAll(); });


        LevelDebug7Button.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("CurrentLevel", 7);
            SceneManager.LoadScene("DebugScene_Level7");
            SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
        });
    }


    private void Level7Check()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", -1) != 7)
        {
            return;
        }

        if (PlayerPrefs.GetInt("Level7MetaCreated", -1) == 1)
        {
            bool Has02 = MetaGameUtil.CheckPlayerDesktopHasFile("GameInfo", "02.txt");
            bool Has01 = MetaGameUtil.CheckPlayerDesktopHasFile("GameInfo", "01.txt");
            if (!Has02 && Has01)
            {
                //正确 跳转到8a
                PlayerPrefs.SetInt("CurrentLevel", 8);
                // SceneManager.LoadScene("SelectCharacterScene");
                // SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else if (Has02 && Has01)
            {
                var startGameTimes = PlayerPrefs.GetInt("Level7MetaDoNothing", 1);
                SceneManager.LoadScene("_7.培养室");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else
            {
                PlayerPrefs.SetInt("CurrentLevel", -8);
                SceneManager.LoadScene("_8B");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
        }
    }

    private void Level9Check()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", -1) != 9) return;
        if (PlayerPrefs.GetInt("Level9MetaCreated", -1) == 1)
        {
            var FloderName = "PB_Meta";
            bool HasXXX = MetaGameUtil.CheckPlayerDesktopHasFile(FloderName, "lian.xxx");
            bool HasTxt = MetaGameUtil.CheckPlayerDesktopHasFile(FloderName, "lian.txt");
            bool Has01 = MetaGameUtil.CheckPlayerDesktopHasFile(FloderName, "01.txt");
            if (!HasXXX && HasTxt && Has01)
            {
                //正确 可以选人
                PlayerPrefs.SetInt("CurrentLevel", 10);
                if (PlayerPrefs.GetInt("Level10Finished", -1) == 1)
                {
                    //经历过第10关, 直接再去9
                    PlayerPrefs.SetInt("CurrentLevel", 9);
                    SceneManager.LoadScene("_9");
                    SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
                }
            }
            else if (!HasXXX && !HasTxt && Has01)
            {
                //跳转到level11
                PlayerPrefs.SetInt("Level9MetaToLevel11", 1);
                PlayerPrefs.SetInt("CurrentLevel", 11);
                SceneManager.LoadScene("_11");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else if (HasXXX && !HasTxt && Has01)
            {
                SceneManager.LoadScene("_9");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else
            {
                PlayerPrefs.SetInt("CurrentLevel", 8);
            }
        }
    }

    private void Level11Check()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", -1) == 11)
        {
            var FloderName = "PB_Meta";
            bool HasDoc = MetaGameUtil.CheckPlayerDesktopHasFile(FloderName, "doc.txt");
            bool Has01 = MetaGameUtil.CheckPlayerDesktopHasFile(FloderName, "01.txt");
            if (!HasDoc && Has01)
            {
                PlayerPrefs.SetInt("Level11MetaDeleteDoc", 1);
                SceneManager.LoadScene("_11");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else
            {
                SceneManager.LoadScene("_11");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
        }
    }

    void Update() { }
}