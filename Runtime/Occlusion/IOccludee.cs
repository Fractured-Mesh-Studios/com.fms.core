using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine.Occlusion
{
    public interface IOccludee
    {
        public abstract void Enable();

        public abstract void Disable();

        public abstract Bounds GetBounds();

        public abstract GameObject GetGameObject();
    }
}
