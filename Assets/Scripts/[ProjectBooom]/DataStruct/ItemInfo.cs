using System;
using UnityEngine;

namespace _ProjectBooom_.DataStruct
{
    /// <summary>
    ///     物品信息
    /// </summary>
    [Serializable]
    public class ItemInfo
    {
        [SerializeField] public string Name;
        [SerializeField] public int    ID;
        [SerializeField] public Sprite Icon;
        [SerializeField] public string Description;
    }
}