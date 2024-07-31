using System;
using System.Collections.Generic;
using _ProjectBooom_.DataStruct;
using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.ObservableData
{
    /// <summary>
    ///     可触发变量变动的全局变量
    /// </summary>
    public class GlobalVariable : Singleton<GlobalVariable>
    {
        #region Init

        [SerializeField]
        private List<Variable> _initVariable;

        protected override void Awake()
        {
            foreach (Variable variable in _initVariable)
            {
                if (!_varDict.TryAdd(variable.Name, variable))
                {
                    DebugHelper.LogWarning($"GlobalVariable: {variable.Name} 重复定义");
                }
            }

            base.Awake();
        }

        #endregion

        /// <summary>
        ///     关键字忽略大小写 存放变量值
        /// </summary>
        private static Dictionary<string, Variable> _varDict = new(StringComparer.InvariantCultureIgnoreCase);

        public static IEnumerable<string> VarNames => _varDict.Keys;

        public static bool ExistVar(in string varName) => _varDict.ContainsKey(varName);

        public static Variable GetVar(in string varName)
        {
            if (_varDict.TryGetValue(varName, out Variable varObj))
            {
                return varObj;
            }

            return null;
        }

        public static void SetVarValue(in string varName, in float value)
        {
            if (!_varDict.TryGetValue(varName, out Variable varObj))
            {
                varObj = new Variable();
                varObj.Name = varName;
                _varDict.Add(varName, varObj);
            }

            varObj.Value = value;
        }

        public static float GetVarValue(in string varName)
        {
            if (_varDict.TryGetValue(varName, out Variable varObj))
            {
                return varObj.Value;
            }

            return 0;
        }

        public static void AddVarListener(in string varName, in Action<float> action)
        {
            if (!_varDict.TryGetValue(varName, out Variable varObj))
            {
                varObj = new Variable();
                varObj.Name = varName;
                _varDict.Add(varName, varObj);
            }

            varObj.RegisterAction(action);
        }

        public static void RemoveVarListener(in string varName, in Action<float> action)
        {
            if (_varDict.TryGetValue(varName, out Variable varObj))
            {
                varObj.UnregisterAction(action);
            }
        }
    }
}