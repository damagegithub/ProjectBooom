using _ProjectBooom_.ScenesScript;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _ProjectBooom_.PuzzleMono
{
    /// <summary>
    ///     清理后处理效果
    /// </summary>
    public class DisablePostProcess : MonoBehaviour
    {
        public Renderer2DData Renderer2DData;
        [Header("相机噪音Blit")]
        public string CameraNoiseBlit = "CameraNoise";
        [Header("相机色散Blit")]
        public string CameraColorDispersionBlit = "CameraColorDispersion";
        private ScriptableRendererFeature _cameraNoiseData;
        private ScriptableRendererFeature _cameraColorDispersionData;


        private void OnEnable()
        {
            _cameraNoiseData = Renderer2DData
                              .rendererFeatures
                              .Find(srf => srf.name.Equals(CameraNoiseBlit));
            _cameraColorDispersionData = Renderer2DData
                                        .rendererFeatures
                                        .Find(srf => srf.name.Equals(CameraColorDispersionBlit));

            bool ifIsScene3 = (bool)FindObjectOfType<_3_Scene>(true);

            if (!ifIsScene3)
            {
                _cameraNoiseData.SetActive(false);
                _cameraColorDispersionData.SetActive(false);
            }
        }
    }
}