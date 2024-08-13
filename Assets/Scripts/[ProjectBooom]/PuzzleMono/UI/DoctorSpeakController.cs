using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace _ProjectBooom_.PuzzleMono.UI
{
    /// <summary>
    ///     博士对话框
    /// </summary>
    public class DoctorSpeakController : MonoBehaviour
    {
        [Header("博士对话框")] public CanvasGroup DoctorCanvasGroup;

        [Header("博士对话文字")] public TextMeshProUGUI TMP_DoctorText;

        [Header("博士每个字的时间")] public float TimePerCharacter = 0.1f;

        [Serializable]
        private class SpeakMessage
        {
            public string Text;
            public bool   AutoFade = true;

            public SpeakMessage(string text) => Text = text;
        }

        private Queue<SpeakMessage> DoctorTextQueue = new();

        public bool IsSpeaking { get; private set; }

        private void OnEnable()
        {
            DoctorCanvasGroup.alpha = 0;
        }

        /// <summary>
        ///     说话
        /// </summary>
        public void Speak(string text, bool isBreakCurrentMessage = false)
        {
            if (isBreakCurrentMessage)
            {
                DOTween.Kill(this);
                DoctorTextQueue.Clear();
                DoctorCanvasGroup.DOFade(0f, 0.5f).SetId(this);
            }

            DoctorTextQueue.Enqueue(new SpeakMessage(text));
            if (!IsSpeaking)
            {
                StartCoroutine(SpeakCoroutine());
            }
        }

        public void SpeakWithoutFade(string text, bool isBreakCurrentMessage = false)
        {
            if (isBreakCurrentMessage)
            {
                DOTween.Kill(this);
                DoctorTextQueue.Clear();
                DoctorCanvasGroup.DOFade(0f, 0.5f).SetId(this);
            }

            DoctorTextQueue.Enqueue(new SpeakMessage(text)
            {
                AutoFade = false,
            });
            if (!IsSpeaking)
            {
                StartCoroutine(SpeakCoroutine());
            }
        }

        /// <summary>
        ///     说话并等待
        /// </summary>
        public IEnumerator SpeakAndWait(string text, bool isBreakCurrentMessage = false)
        {
            Speak(text, isBreakCurrentMessage);
            while (IsSpeaking)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator SpeakCoroutine()
        {
            if (!IsSpeaking)
            {
                IsSpeaking = true;
            }
            else
            {
                yield break;
            }

            // 等待动画结束
            while (DOTween.IsTweening(this))
            {
                yield return new WaitForEndOfFrame();
            }

            while (DoctorTextQueue.TryDequeue(out SpeakMessage messageData))
            {
                yield return new WaitForNextFrameUnit();
                yield return DoctorSpeak(messageData.Text, messageData.AutoFade);
            }

            IsSpeaking = false;
        }


        public IEnumerator DoctorSpeak(string text, bool autoFade = true)
        {
            TMP_DoctorText.maxVisibleCharacters = 0;
            TMP_DoctorText.text = text;
            float duration = text.Length * TimePerCharacter;

            Sequence sequence = DOTween
                               .Sequence()
                               .Append(DoctorCanvasGroup.DOFade(1f, 0.5f))
                               .Append(DOTween.To(
                                           () => TMP_DoctorText.maxVisibleCharacters,
                                           x => TMP_DoctorText.maxVisibleCharacters = x,
                                           text.Length,
                                           duration
                                       ));
            if (autoFade)
            {
                sequence = sequence.Append(DoctorCanvasGroup.DOFade(0f, 0.5f));
            }

            sequence.SetEase(Ease.Linear)
                    .SetId(this);

            // 等待动画结束
            while (DOTween.IsTweening(this))
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}