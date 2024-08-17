using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[ExecuteInEditMode]
public class ShowWorldPosition : MonoBehaviour
{
    public Vector3 worldPosition;

    void Update()
    {
        // 更新世界位置
        worldPosition = transform.position;
    }


}
