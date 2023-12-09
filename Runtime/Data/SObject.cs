using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Data
{
    [System.Serializable]
    public class SObject
    {
        public int id;
        public string name;

        public STransform transform;
        public SRigidbody rigidbody;
        public SComponent[] component;
    }
}
