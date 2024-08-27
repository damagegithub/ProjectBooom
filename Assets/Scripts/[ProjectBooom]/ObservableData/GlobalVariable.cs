using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _ProjectBooom_.DataStruct;
using LYP_Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace _ProjectBooom_.ObservableData
{
    /// <summary>
    ///     可触发变量变动的全局变量 还提供一个简单的保存与读取
    /// </summary>
    public class GlobalVariable : Singleton<GlobalVariable>
    {
        #region Init

        /// <summary>
        ///     保存的全局变量路径
        /// </summary>
        private string SaveFilePath =>
            Path.Combine(Application.streamingAssetsPath, "SaveData/GlobalVariable.json");

        /// <summary>
        ///     初始化的全局变量
        /// </summary>
        [SerializeField]
        [Header("初始化的全局变量")]
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

            // // 在游戏启动时加载全局变量
            // LoadFrom(SaveFilePath);

            base.Awake();
        }

        /// <summary>
        ///     保存
        /// </summary>
        public void SaveTo(string filePath)
        {
            DirectoryInfo parentDir = new FileInfo(filePath).Directory;
            // 如果父文件夹不存在则创建
            if (parentDir != null && !parentDir.Exists)
            {
                parentDir.Create();
            }

            Dictionary<string, float> kvMap = _varDict.ToDictionary(v => v.Key, v => GetVarValue(v.Key));
            string json = JsonConvert.SerializeObject(kvMap);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        ///     读取
        /// </summary>
        public void LoadFrom(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string json = File.ReadAllText(filePath);
            Dictionary<string, float> kvMap = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);
            foreach (KeyValuePair<string, float> kv in kvMap)
            {
                SetVarValue(kv.Key, kv.Value);
            }
        }

        /// <summary>
        ///     游戏退出时保存全局变量
        /// </summary>
        private void OnApplicationQuit()
        {
            // SaveTo(SaveFilePath);
        }

        #endregion

        /// <summary>
        ///     关键字忽略大小写 存放变量值
        /// </summary>
        private static Dictionary<string, Variable> _varDict = new(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        ///     获取存在的全部变量名
        /// </summary>
        public static IEnumerable<string> VarNames => _varDict.Keys;

        /// <summary>
        ///     测试某变量是否存在
        /// </summary>
        public static bool ExistVar(in string varName) => _varDict.ContainsKey(varName);

        /// <summary>
        ///     获取变量对象
        /// </summary>
        public static Variable GetVar(in string varName)
        {
            if (_varDict.TryGetValue(varName, out Variable varObj))
            {
                return varObj;
            }

            return null;
        }

        /// <summary>
        ///     设置变量值
        /// </summary>
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

        /// <summary>
        ///     获取变量值
        /// </summary>
        public static float GetVarValue(in string varName)
        {
            if (_varDict.TryGetValue(varName, out Variable varObj))
            {
                return varObj.Value;
            }

            return 0;
        }

        /// <summary>
        ///     添加变量赋值监听器
        /// </summary>
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

        /// <summary>
        ///     移除变量赋值监听器
        /// </summary>
        public static void RemoveVarListener(in string varName, in Action<float> action)
        {
            if (_varDict.TryGetValue(varName, out Variable varObj))
            {
                varObj.UnregisterAction(action);
            }
        }
    }
}