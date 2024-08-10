using System;
using UnityEngine;

namespace _ProjectBooom_.DataStruct
{
    [Serializable]
    public class CaptchaInfo
    {
        [SerializeField] [Header("验证码题目")] public string Title;
        [SerializeField] [Header("验证码描述内容")] public string Content;
        [SerializeField] [Header("验证码答案")] public string[] Answers = Array.Empty<string>();
        [SerializeField] [Header("验证码正确答案")] public string CorrectAnswer;
    }
}