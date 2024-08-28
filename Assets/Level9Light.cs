using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening; // 添加DoTween的命名空间

public class Level9Light : MonoBehaviour
{
    public Light2D light;
    public float duration = 2f;
    public float MinIntensity = 1f;
    public float MaxIntensity = 11f;

    // Start is called before the first frame update
    void Start()
    {
        // 灯光强度从1-11循环变化的dotween动画
        light.intensity = MinIntensity;                                                // 初始化灯光强度
        DOTween.To(() => light.intensity, x => light.intensity = x, MaxIntensity, duration) // 从1到11，持续2秒
               .SetEase(Ease.Linear)                                      // 设置缓动类型
               .SetLoops(-1, LoopType.Yoyo);                                 // 无限循环，来回变化
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}