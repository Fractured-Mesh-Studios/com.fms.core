using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public enum CollisionHandling
    {
        None,
        Ignore,
        Block,
        Overlap,
    }

    public class Collision : MonoBehaviour
    {
        public CollisionHandling CollisionHandling;
        public GameObject Target;

        private Collider[] SelfColliders;
        private Collider[] TargetColliders;

        protected void Awake()
        {
            SelfColliders = GetComponentsInChildren<Collider>();
            TargetColliders = Target.GetComponentsInChildren<Collider>();

            switch (CollisionHandling)
            {
                case CollisionHandling.Ignore:

                    for (int i = 0; i < TargetColliders.Length; i++)
                    {
                        for(int k = 0; k < SelfColliders.Length; k++)
                        {
                            Physics.IgnoreCollision(SelfColliders[k], TargetColliders[i]);
                        }
                    }
                    break;
                case CollisionHandling.Block: break;
                case CollisionHandling.Overlap: break;
            }
        }
    }
}
