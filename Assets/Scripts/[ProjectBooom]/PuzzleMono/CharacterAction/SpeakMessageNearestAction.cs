using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    public class SpeakMessageNearestAction : NearestAction
    {
        /// <summary>
        ///   是否可以中断当前对话并播放
        /// </summary>
        [Header("是否可以中断当前对话并播放")]
        [SerializeField]
        public bool IsBreakable = false;

        /// <summary>
        ///   对话内容
        /// </summary>
        [Header("对话内容")]
        [SerializeField]
        public string Message;
    }
}