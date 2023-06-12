using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace GameEngine
{
    public class ValueGeneric { }

    [Serializable]
    public class Value<T> : ValueGeneric
    {
        public T value { 
            set {
                old = _value;
                _value = value;
                OnChange.Invoke(_value);
            } 
            get { return _value; }
        }

        public T cache { get { return old; } }

        private T _value, old;


        public Action<T> OnChange;
        public float OnChangeDelay = 1;
    }
}
