using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine.Event
{

    public static class EventManager
    {
        private interface ICallback
        {
            public void Trigger(object callbackEvent);
            public bool Compare(object callbackEvent);
        }
        
        private class EventCallback : Dictionary<Type, List<ICallback>> { }

        private class GenericCallback<T> : ICallback where T : IBaseEvent
        {

            private readonly Action<T> m_callback;

            public GenericCallback(Action<T> callback)
            {
                this.m_callback = callback;
            }

            public void Trigger(object callbackEvent)
            {
                m_callback?.Invoke((T)callbackEvent);
            }

            public bool Compare(object callbackEvent)
            {
                return m_callback == (Action<T>)callbackEvent;
            }
        }

        private static readonly EventCallback s_callbacks = new EventCallback();

        #region SUBSCRIBE
        public static void Subscribe<T>(Action<T> callback) where T : IBaseEvent
        {
            var callbackType = typeof(T);

            if (!s_callbacks.ContainsKey(callbackType))
            {
                s_callbacks.Add(callbackType, new List<ICallback>());
            }

            var callbackList = s_callbacks[callbackType];

            var genericCallback = new GenericCallback<T>(callback);

            callbackList.Add(genericCallback);
        }

        public static void UnSubscribe<T>(Action<T> callback) where T : IBaseEvent
        {
            var callbackType = typeof(T);

            if (!s_callbacks.ContainsKey(callbackType))
            {
                return;
            }

            var callbackList = s_callbacks[callbackType];

            var current = callbackList.Find(x => x.Compare(callback));

            if (current != null)
            {
                callbackList.Remove(current);
            }
        }
        #endregion

        #region TRIGGER
        public static void Trigger<T>(T eventObject) where T : IBaseEvent
        {
            var callbackType = typeof(T);

            if (!s_callbacks.ContainsKey(callbackType))
            {
                Debug.Log($"No s_callbacks found for typeof {callbackType}");
                return;
            }

            /// create a safe array before call.
            var tempCallbacks = new ICallback[s_callbacks[callbackType].Count];
            s_callbacks[callbackType].CopyTo(tempCallbacks);

            for(int i = 0; i < tempCallbacks.Length; i++)
            {
                tempCallbacks[i].Trigger(eventObject);
            }

            /*foreach (var callback in tempCallbacks)
            {
                callback.Trigger(eventObject);
            }*/
        }
        #endregion
    }
}
