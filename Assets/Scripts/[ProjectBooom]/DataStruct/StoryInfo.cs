using System;
using UnityEngine;
using UnityEngine.Events;

namespace _ProjectBooom_.DataStruct
{
    /// <summary>
    ///  简单的剧情信息
    /// </summary>
    [Serializable]
    public class StoryInfo
    {
        [SerializeField] [Header("当前故事名称")] public string StoryName;
        [SerializeField] [Header("当前故事描述")] public string StoryDescription;

        [SerializeField] [Header("当前剧情是否已经开始")]
        public bool IsBegin;

        [SerializeField] [Header("当前剧情是否已经结束")]
        public bool IsFinished;

        [Header("在流程开始时播放的事件")] public UnityEvent BeginAction;
        [Header("在流程结束时播放的事件")] public UnityEvent EndAction;

        [NonSerialized] public StoryController StoryController;
    }
}