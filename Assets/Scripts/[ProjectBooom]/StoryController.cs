using System;
using System.Collections.Generic;
using _ProjectBooom_.DataStruct;
using LYP_Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _ProjectBooom_
{
    /// <summary>
    ///  用于简单控制剧情流程的类
    /// </summary>
    public class StoryController : MonoBehaviour
    {
        [Header("调试文本")] public Text DebugText;

        [SerializeField] [Header("当前场景故事流程")] public List<StoryInfo> StoryInfos = new List<StoryInfo>();
        [Header("当前场景故事索引")] public int CurrentStoryIndex = 0;
        [Header("下一个场景的名称")] public string NextSceneName;

        public void SetDebugText(string debugText)
        {
            DebugText.text = $"{GetCurrentStoryInfo().StoryName}->{debugText}";
        }

        private void Start()
        {
            CurrentStoryIndex = 0;

            foreach (var storyInfo in StoryInfos)
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
        ///  尝试开始当前剧情
        /// </summary>
        private void TryStartCurrentStory()
        {
            var story = GetCurrentStoryInfo();
            if (story != null)
            {
                story.IsBegin = true;
                story.BeginAction?.Invoke();
                DebugText.text = $"{story.StoryName}->{story.StoryDescription}";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(NextSceneName) || !IsSceneNameExist(NextSceneName))
                {
                    DebugText.text = $"没有下一个场景的跳转->{SceneManager.GetActiveScene().name}";
                    return;
                }

                // 跳转到下一个场景
                SceneManager.LoadScene(NextSceneName);
            }
        }

        public static bool IsSceneNameExist(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var sceneNameInBuildSetting =
                    System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (sceneNameInBuildSetting.Equals(sceneName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 结束当前剧情
        /// </summary>
        public void TryFinishCurrentStory()
        {
            var story = GetCurrentStoryInfo();
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
        ///  获取当前故事剧情
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