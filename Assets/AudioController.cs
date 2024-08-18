using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public float       fadeInDuration = 2.0f;  // 淡入时间
    public float       fadeOutDuration = 2.0f;  // 淡入时间

    public void FadefInBGM()
    {
        audioSource.volume = 0;  // 初始音量为 0
        StartCoroutine(FadeIn());
    }
    
    public void FadefOutBGM()
    {
        audioSource.volume = 1;  // 初始音量为 0
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        audioSource.Play(); 
        float startVolume = 0;
        float targetVolume = 1.0f;  // 目标音量

        float currentTime = 0;

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeInDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;  // 确保最终音量达到目标值
    }
    
    private System.Collections.IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        float currentTime = 0;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeOutDuration);
            yield return null;
        }

        audioSource.volume = 0; // 确保音量最终归零
        audioSource.Stop();     // 停止音频播放
    }
    
  
}
