using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine
{
    public class UpdateManager : MonoBehaviour
    {
        private static UpdateManager s_instance;

        private static List<Action> s_update = new List<Action>();
        private static List<Action> s_fixedUpdate = new List<Action>();
        private static List<Action> s_lateUpdate = new List<Action>();
        
        public static void Subscribe(Action callback)
        {
            Subscribe(callback, UpdateMethod.Update);
        }

        public static void Subscribe(Action callback, UpdateMethod method)
        {
            switch (method)
            {
                case UpdateMethod.Update: s_update.Add(callback); break;
                case UpdateMethod.FixedUpdate: s_fixedUpdate.Add(callback); break;
                case UpdateMethod.LateUpdate: s_lateUpdate.Add(callback); break;
                default: break;
            }
        }

        public static void Unsubscribe(Action callback, UpdateMethod method)
        {
            switch (method)
            {
                case UpdateMethod.Update: s_update.Remove(callback); break;
                case UpdateMethod.FixedUpdate: s_fixedUpdate.Remove(callback); break;
                case UpdateMethod.LateUpdate: s_lateUpdate.Remove(callback); break;
                default: break;
            }
        }

        #region UNITY

        private void Awake()
        {
            if(s_instance != null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            foreach(var behavior in s_update)
            {
                behavior.Invoke();
            }
        }

        private void FixedUpdate()
        {
            foreach (var behavior in s_fixedUpdate)
            {
                behavior.Invoke();
            }
        }

        private void LateUpdate() 
        {
            foreach (var behavior in s_lateUpdate)
            {
                behavior.Invoke();
            }
        }

        #endregion
    

    }
}
