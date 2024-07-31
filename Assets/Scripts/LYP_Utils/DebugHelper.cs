using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LYP_Utils
{
    public static class DebugHelper
    {
        /// <summary>
        ///     Debug.Log方法 编译时会被移除
        /// </summary>
        public static void Log(object message, Object context = null)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (context == null)
            {
                Debug.Log(message);
            }
            else
            {
                Debug.Log(message, context);
            }
            #endif
        }

        /// <summary>
        ///     Debug.LogWarning方法 编译时会被移除
        /// </summary>
        public static void LogWarning(object message, Object context = null)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (context == null)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.LogWarning(message, context);
            }
            #endif
        }

        /// <summary>
        ///     Debug.LogError方法 编译时会被移除
        /// </summary>
        public static void LogError(object message, Object context = null)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (context == null)
            {
                Debug.LogError(message);
            }
            else
            {
                Debug.LogError(message, context);
            }
            #endif
        }

        /// <summary>
        ///     Debug.LogException方法 编译时会被移除
        /// </summary>
        public static void LogException(Exception exception, Object context = null)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (context == null)
            {
                Debug.LogException(exception);
            }
            else
            {
                Debug.LogException(exception, context);
            }
            #endif
        }
    }
}