using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    [System.Serializable]
    public class LODAdjustment 
    {
        public float minSquareDistance;
        public float maxSquareDistance;
        public ShadowResolution resolution;
        public ShadowQuality quality;
        public LightShadows lightShadows;
        public bool enabled;
        [Range(0,1)]public float strength;
    }
}
