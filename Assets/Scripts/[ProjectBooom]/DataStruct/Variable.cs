using System;
using System.Collections.Generic;
using UnityEngine;

namespace _ProjectBooom_.DataStruct
{
    /// <summary>
    ///     变量 用于监听与触发
    /// </summary>
    [Serializable]
    public class Variable
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public int ID;
        [SerializeField]
        private float _value;

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                for (int i = 0; i < _actions.Count; i++)
                {
                    _actions[i]?.Invoke(_value);
                }
            }
        }
        private List<Action<float>> _actions = new();

        public void RegisterAction(in Action<float> action)
        {
            _actions.Add(action);
        }

        public void UnregisterAction(in Action<float> action)
        {
            _actions.Remove(action);
        }
    }
}