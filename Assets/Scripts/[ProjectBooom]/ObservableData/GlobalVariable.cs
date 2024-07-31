using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _ProjectBooom_.DataStruct;
using LYP_Utils;
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
            Path.Combine(Application.streamingAssetsPath, "SaveData/GlobalVariable.savedata");

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

            // 在游戏启动时加载全局变量
            LoadFrom(SaveFilePath);

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

            IEnumerable<string> lines = _varDict.Values.Select(v => $"{v.Name},{v.Value}");
            File.WriteAllText(filePath, string.Join('\n', lines));
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

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                IReadOnlyList<string> parts = LCsv.ParseIgnoreQuotation(lines[i]);
                if (parts.Count < 2)
                {
                    continue;
                }

                if (float.TryParse(parts[1], out float value))
                {
                    DebugHelper.Log($"加载变量覆盖 {parts[0]} {value}");
                    SetVarValue(parts[0], value);
                }
            }
        }

        /// <summary>
        ///     游戏退出时保存全局变量
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveTo(SaveFilePath);
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