using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MetaGameUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public Button      GameStartButton;
    public Button      LevelDebug7Button;
    public Button      LevelDebug3Button;
    public Button      LevelDebug11Button;
    public Button      ClearPlayerPrefsButton;
    public CanvasGroup BlackCanvasGroup;

    private void Awake()
    {
        BlackCanvasGroup.alpha = 1;
    }

    void Start()
    {
        // PlayerPrefs.DeleteAll();
        StartCoroutine(ScriptStart());
    }

    public IEnumerator ScriptStart()
    {
        Level7Check();
        Level9Check();
        Level11Check();

        GameStartButton.onClick.AddListener(() =>
        {
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

            if (currentLevel == 1)
            {
                //完成关卡1之前, 没有选人界面
                SceneManager.LoadScene("_1.培养室");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else
            {
                //完成关卡1之后, 进入选人界面
                SceneManager.LoadScene("SelectCharacterScene");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
        });

        ClearPlayerPrefsButton.onClick.AddListener(() => { PlayerPrefs.DeleteAll(); });


        LevelDebug7Button.onClick.AddListener(() => { PlayerPrefs.SetInt("CurrentLevel", 7); });
        LevelDebug11Button.onClick.AddListener(() => { PlayerPrefs.SetInt("CurrentLevel", 11); });

        LevelDebug3Button.onClick.AddListener(() => { PlayerPrefs.SetInt("CurrentLevel", 3); });
        yield return BlackCanvasGroup.DOFade(0f, 1.0f).SetId(this).WaitForCompletion();
    }


    private void Level7Check()
    {
        //关卡7 或者 -8关
        if (PlayerPrefs.GetInt("CurrentLevel", -1) != 7 && PlayerPrefs.GetInt("Level7MetaCreated", -1) != -8)
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
                //对应案子a, 出现lian 本体
                PlayerPrefs.SetInt("CurrentLevel", 10);
                if (PlayerPrefs.GetInt("Level10Finished", -1) == 1)
                {
                    //经历过第10关, 直接再去9
                    PlayerPrefs.SetInt("CurrentLevel", 9);
                    PlayerPrefs.SetInt("levle9MetaDialog", 902);
                    SceneManager.LoadScene("_9");
                    SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
                }
            }
            else if (!HasXXX && !HasTxt && Has01)
            {
                //对应案子b, 删除了文件,进入关卡11
                //跳转到level11
                PlayerPrefs.SetInt("Level9MetaToLevel11", 1);
                PlayerPrefs.SetInt("CurrentLevel", 11);
                // SceneManager.LoadScene("_11");
                // SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else if (HasXXX && !HasTxt && Has01)
            {
                //对应C 玩家没有任何操作
                PlayerPrefs.SetInt("levle9MetaDialog", 902);
                SceneManager.LoadScene("_9");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
            else
            {
                // 对应案子d, 其他情况
                PlayerPrefs.SetInt("levle9MetaDialog", 903);
                SceneManager.LoadScene("_9");
                SceneManager.UnloadSceneAsync("_0.MainScene_开始界面");
            }
        }
    }

    private void Level11Check()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", -1) == 11)
        {
            if (PlayerPrefs.GetInt("Level11MetaCreated", -1) == 1)
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
    }

    void Update() { }
}