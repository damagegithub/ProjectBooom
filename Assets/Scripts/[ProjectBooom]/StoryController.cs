using System.Collections.Generic;
using System.IO;
using _ProjectBooom_.DataStruct;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _ProjectBooom_
{
    /// <summary>
    ///     用于简单控制剧情流程的类
    /// </summary>
    public class StoryController : MonoBehaviour
    {
        public Dictionary<string, int> SceneTable = new()
        {
            { "_1.培养室", 1 },
            { "_2.走道", 2 },
            // { "_3.走道_会议室", 3 },
            { "_3_4.会议室", 3 },
            { "_4.会议室", 4 },
            { "_5.培养室", 5 },
            { "_6.小巷", 6 },
            { "_7.培养室", 7 },
            { "_8_伪.黑幕", -8 },
            { "_8_真.培养室", 8 },
            { "_9.小巷", 9 },
            { "_10.走道", 10 },
            { "_11_隐藏.真莲房间", 11 },
        };

        [Header("调试文本")] public Text DebugText;

        [SerializeField] [Header("当前场景故事流程")] public List<StoryInfo> StoryInfos        = new();
        [Header("当前场景故事索引")]                  public int             CurrentStoryIndex = 0;
        [Header("下一个场景的名称")]                  public string          NextSceneName;

        public void SetDebugText(string debugText)
        {
            if (DebugText)
            {
                DebugText.text = $"{GetCurrentStoryInfo().StoryName}->{debugText}";
            }
            Debug.Log(debugText);
        }

        private void Start()
        {
            CurrentStoryIndex = 0;

            foreach (StoryInfo storyInfo in StoryInfos)
            {
                storyInfo.StoryController = this;
                storyInfo.IsBegin = false;
                storyInfo.IsFinished = false;
            }

            #if DEBUG
            if (StoryInfos.Count == 0)
            {
                DebugText.text = $"当前场景没有剧情->{SceneManager.GetActiveScene().name}";
                return;
            }
            #endif
            TryStartCurrentStory();
        }

        /// <summary>
        ///     尝试开始当前剧情
        /// </summary>
        private void TryStartCurrentStory()
        {
            StoryInfo story = GetCurrentStoryInfo();
            if (story != null)
            {
                story.IsBegin = true;
                story.BeginAction?.Invoke();
                if (DebugText)
                {
                    DebugText.text = $"{story.StoryName}->{story.StoryDescription}";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(NextSceneName) || !IsSceneNameExist(NextSceneName))
                {
                    if (DebugText)
                    {
                        DebugText.text = $"没有下一个场景的跳转->{SceneManager.GetActiveScene().name}";
                    }
                    return;
                }

                // 设置场景结束时的索引 用于记录当前场景
                string currentSceneName = SceneManager.GetActiveScene().name;
                if (SceneTable.TryGetValue(currentSceneName, out int sceneIndex))
                {
                    PlayerPrefs.SetInt("CurrentLevel", sceneIndex + 1);
                    Debug.Log($"set current level: {sceneIndex}");
                    // SceneManager.LoadScene(0); // 回到选人界面
                    SceneManager.LoadScene("SelectCharacterScene");
                    SceneManager.UnloadSceneAsync(currentSceneName);
                }
            }
        }

        public static bool IsSceneNameExist(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string sceneNameInBuildSetting =
                    Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (sceneNameInBuildSetting.Equals(sceneName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     结束当前剧情
        /// </summary>
        public void TryFinishCurrentStory()
        {
            StoryInfo story = GetCurrentStoryInfo();
            if (story != null)
            {
                story.IsFinished = true;
                story.EndAction?.Invoke();
                CurrentStoryIndex += 1;
                TryStartCurrentStory();
            }
            else
            {
                // 当前场景已经结束
            }
        }

        /// <summary>
        ///     获取当前故事剧情
        /// </summary>
        private StoryInfo GetCurrentStoryInfo()
        {
            // 如果当前故事索引大于等于故事信息的数量，则返回null
            if (CurrentStoryIndex >= StoryInfos.Count)
            {
                return null;
            }
            else
            {
                return StoryInfos[CurrentStoryIndex];
            }
        }
    }
}