using System;
using LYP_Utils;
using UnityEngine;

namespace _ProjectBooom_.Input
{
    /// <summary>
    ///     玩家输入封装
    /// </summary>
    public class InputWarp : Singleton<InputWarp>
    {
        private void Update()
        {
            if (ActionKeyDown())
            {
                OnActionKeyDown?.Invoke();
            }

            if (ActionKeyUp())
            {
                OnActionKeyUp?.Invoke();
            }
        }

        public static Action OnActionKeyDown;
        public static Action OnActionKeyUp;

        public static bool ActionKeyDown()
        {
            #if ENABLE_LEGACY_INPUT_MANAGER
            return UnityEngine.Input.GetKeyDown(KeyCode.F);
            #elif ENABLE_INPUT_SYSTEM
            throw new System.NotImplementedException();
            #endif
        }

        public static bool ActionKeyUp()
        {
            #if ENABLE_LEGACY_INPUT_MANAGER
            return UnityEngine.Input.GetKeyUp(KeyCode.F);
            #elif ENABLE_INPUT_SYSTEM
            throw new System.NotImplementedException();
            #endif
        }
    }
}