using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = DebugEngine.Debug;

namespace GameEngine
{
    [RequireComponent(typeof(Rigidbody))]
    public class Gravity : MonoBehaviour
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public Vector3 targetVector;
        [HideInInspector] public float force = 9.81f;
        [HideInInspector][Range(0,1)]public float scale = 1f;
        [HideInInspector] public UpdateMethod updateMethod = UpdateMethod.FixedUpdate;
        
        public Vector3 direction { internal set; get; }

        private Rigidbody m_rigidbody;
        private Vector3 m_acceleration, m_vector;
        private float m_scaledForce;

        protected void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_vector = Vector3.zero;
            m_scaledForce = 0.0f;
        }

        protected void Update()
        {
            if (updateMethod == UpdateMethod.Update)
            {
                CalculateGravity();
            }
        }

        protected void FixedUpdate()
        {
            if (updateMethod == UpdateMethod.FixedUpdate)
            {
                CalculateGravity();
            }
        }

        protected void LateUpdate()
        {
            if (updateMethod == UpdateMethod.LateUpdate)
            {
                CalculateGravity();
            }
        }

        private void CalculateGravity()
        {
            if (m_rigidbody.useGravity)
            {
                m_vector = (target) ?
                       target.position - transform.position :
                       targetVector - transform.position;

                direction = m_vector.normalized;
                m_scaledForce = force * Mathf.Clamp01(scale);
                m_acceleration = direction * m_scaledForce;
                m_rigidbody.AddForce(m_acceleration, ForceMode.Acceleration);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (target)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, direction);
            }
        }
    }
}