using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = DebugEngine.Debug;

namespace GameEngine
{
    public class FollowPhysics : Follow
    {
        protected Rigidbody m_rigidbody;

        protected override void Awake()
        {
            m_rigidbody = GetComponentInChildren<Rigidbody>();
            if(m_rigidbody != null)
            {
                Debug.Log("Follow Physics Enabled");
            }
        }
    }
}
