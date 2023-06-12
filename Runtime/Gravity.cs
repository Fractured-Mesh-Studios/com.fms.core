using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = DebugEngine.Debug;

namespace GameEngine
{
    [RequireComponent(typeof(Rigidbody))]
    public class Gravity : MonoBehaviour
    {
        [HideInInspector]public Transform Target;
        [HideInInspector]public Vector3 TargetVector;
        [HideInInspector]public float Force = 9.81f;
        [HideInInspector][Range(0,1)]public float Scale = 1;
        [HideInInspector]public UpdateMethod UpdateMethod = UpdateMethod.FixedUpdate;
        
        public Vector3 Direction { internal set; get; }

        private Rigidbody BodyComponent;
        private Vector3 Newton, Vector;
        private float ScaledForce;

        protected void Awake()
        {
            BodyComponent = GetComponent<Rigidbody>();
            Vector = Vector3.zero;
            ScaledForce = 0.0f;
        }

        protected void Update()
        {
            if (BodyComponent.useGravity && UpdateMethod == UpdateMethod.Update)
            {
                Vector = (Target) ?
                    Target.position - transform.position :
                    TargetVector - transform.position;

                Direction = Vector.normalized;
                ScaledForce = Force * Mathf.Clamp01(Scale);
                Newton = BodyComponent.mass * (Direction * ScaledForce);
                BodyComponent.AddForce(Newton, ForceMode.Force);
            }
        }

        protected void FixedUpdate()
        {
            if (BodyComponent.useGravity && UpdateMethod == UpdateMethod.FixedUpdate)
            {
                Vector = (Target) ? 
                    Target.position - transform.position :
                    TargetVector - transform.position;

                Direction = Vector.normalized;
                ScaledForce = Force * Mathf.Clamp01(Scale);
                Newton = BodyComponent.mass * (Direction * ScaledForce);
                BodyComponent.AddForce(Newton, ForceMode.Force);
            }
        }

        protected void LateUpdate()
        {
            if (BodyComponent.useGravity && UpdateMethod == UpdateMethod.LateUpdate)
            {
                Vector = (Target) ?
                    Target.position - transform.position :
                    TargetVector - transform.position;

                Direction = Vector.normalized;
                ScaledForce = Force * Mathf.Clamp01(Scale);
                Newton = BodyComponent.mass * (Direction * ScaledForce);
                BodyComponent.AddForce(Newton, ForceMode.Force);
            }
        }
    }
}