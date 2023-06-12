using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = DebugEngine.Debug;

namespace GameEngine
{
    public class FollowPhysics : Follow
    {
        protected Rigidbody BodyComponent;

        protected override void Awake()
        {
            BodyComponent = GetComponentInChildren<Rigidbody>();
            if(BodyComponent != null)
            {
                Debug.Log("Follow Physics Enabled");
            }
        }
    }
}
