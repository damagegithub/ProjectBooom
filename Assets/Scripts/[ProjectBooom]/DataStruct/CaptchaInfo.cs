using System;
using UnityEngine;

namespace _ProjectBooom_.DataStruct
{
    [Serializable]
    public class CaptchaInfo
    {
        [Serializable]
        public class CaptchaAnswerInfo
        {
            [SerializeField]
            [Header("验证码题目文本")]
            public string Answer;
            
            [SerializeField]
            [Header("验证码题目图片")]
            public Sprite Image;
            
            [SerializeField]
            [Header("验证码题目音频")]
            public AudioClip Audio;
        }

        [SerializeField]
        [Header("验证码题目")]
        public string Title;
        
        [SerializeField]
        [Header("验证码描述内容")]
        public string Content;
        
        [SerializeField]
        [Header("验证码答案")]
        public string[] Answers = Array.Empty<string>();
        
        [SerializeField]
        [Header("验证码正确答案")]
        public string CorrectAnswer;

        [SerializeField]
        [Header("文本或图片或音频答案")]
        public CaptchaAnswerInfo[] AnswerInfos = Array.Empty<CaptchaAnswerInfo>();
    }
}