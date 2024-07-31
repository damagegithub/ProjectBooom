using System;
using UnityEngine;

namespace LYP_Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>(true);
                }

                if (!_instance)
                {
                    _instance = GlobalManager.AddComponent<T>();
                }

                if (!_instance)
                {
                    throw new Exception("Singleton instance cannot found or created");
                }

                return _instance;
            }
            protected set => _instance = value;
        }
        public static bool InstanceExists => Instance;

        protected virtual void Awake()
        {
            if (_instance == (T)this)
            {
                return;
            }

            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        #region GlobalManagerObject

        public const string GlobalManagerName = "---GlobalManager---";
        // ReSharper disable once StaticMemberInGenericType
        private static GameObject _globalManager;
        public static GameObject GlobalManager
        {
            get
            {
                if (_globalManager)
                {
                    return _globalManager;
                }

                _globalManager = GameObject.Find(GlobalManagerName);
                if (!_globalManager)
                {
                    _globalManager = new GameObject(GlobalManagerName);
                }

                DontDestroyOnLoad(_globalManager);
                return _globalManager;
            }
        }

        #endregion
    }
}