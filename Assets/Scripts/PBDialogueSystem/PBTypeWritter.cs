using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PBDialogueSystem;
using TMPro;
using UnityEngine;

public class PBTypeWritter : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float           typingSpeed = 0.1f; // 每个字符的时间间隔
    public int            typingAudioDurations = 3; // 每个几个字符播放一次音效

    private int   _currentCharIndex;
    private Tween _typewriterTween;

    public Conversation conversation;

    public AudioSource audioSource;

    public string fullText = "";


    public void ShowFullText()
    {
        // 立即完成打字机效果的Tween并显示完整文本
        if (_typewriterTween != null && _typewriterTween.IsActive())
        {
            _typewriterTween.Kill();
        }

        // textMeshPro.text = fullText;
        textMeshPro.SetText(fullText);
        textMeshPro.maxVisibleCharacters = fullText.Length;
        OnTypingComplete();
    }

    public void StartTyping()
    {
        // 确保textMeshPro引用已分配
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        textMeshPro.text = "";
        _currentCharIndex = 0;

        // 开始打字机效果
        StartTypewriterEffect();
    }

    void StartTypewriterEffect()
    {
        _typewriterTween = DOTween.To(() => _currentCharIndex, x =>
                                      {
                                          if (x > _currentCharIndex)
                                          {
                                            if (_currentCharIndex % typingAudioDurations == 0)
                                            {
                                                audioSource.PlayOneShot(audioSource.clip);
                                            }
                                              // audioSource.Play();
                                          }
                                          
                                          _currentCharIndex = x;
                                      }, fullText.Length,
                                      fullText.Length * typingSpeed)
                                  .SetEase(Ease.Linear)
                                  .OnUpdate(UpdateText)
                                  .OnComplete(OnTypingComplete);
    }


    void UpdateText()
    {
        if (!string.Equals(textMeshPro.text, fullText, StringComparison.InvariantCultureIgnoreCase))
        {
            textMeshPro.SetText(fullText);
        }
        
        // textMeshPro.text = fullText.Substring(0, _currentCharIndex);
        textMeshPro.maxVisibleCharacters = _currentCharIndex;
        audioSource.Play();
    }

    void OnTypingComplete()
    {
        // 完成后的操作（可选）
        conversation.State = ConversationState.WaitingForNext;
    }
}