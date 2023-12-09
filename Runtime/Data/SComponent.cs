using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Data
{
    [System.Serializable]
    public struct SField
    {
        public string name;
        public object value;

        public SField(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }

    [System.Serializable]
    public class SComponent
    {
        public string name;
        public bool isComponent;
        public SField[] field;

        public SComponent(string name, bool isComponent, SField[] field)
        {
            this.name = name;
            this.isComponent = isComponent;
            this.field = field;
        }
    }
}
