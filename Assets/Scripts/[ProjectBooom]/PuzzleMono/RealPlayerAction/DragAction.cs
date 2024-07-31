using System;
using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.RealPlayerAction
{
    /// <summary>
    ///     现实玩家拖拽行为
    /// </summary>
    public class DragAction : MonoBehaviour
    {
        private Vector3 _originPosition;

        private void Awake()
        {
            _originPosition = transform.position;
        }

        private void OnMouseDrag()
        {
            throw new NotImplementedException();
        }
    }
}