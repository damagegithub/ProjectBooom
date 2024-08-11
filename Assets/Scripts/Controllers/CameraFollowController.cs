using System;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    ///   摄像机跟随控制器
    /// </summary>
    public class CameraFollowController: MonoBehaviour
    {
        public Transform targetTrans;
        public Transform cameraTrans;
        public float cameraMinX;
        public float cameraMaxX;

        private void Awake()
        {
            if (!targetTrans)
            {
                var pc = FindObjectOfType<PlayerController>(true);
                if (pc)
                {
                    targetTrans = pc.transform;
                }
            }
            
            if (!cameraTrans)
            {
                cameraTrans = Camera.main.transform;
            }
        }

        private void Update()
        {
            var tartgetX = targetTrans.position.x;
            var cameraPos = cameraTrans.position;
            cameraPos.x = Mathf.Clamp(tartgetX, cameraMinX, cameraMaxX);
            cameraTrans.position = cameraPos;
        }
    }
}