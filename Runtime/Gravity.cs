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
            if (m_rigidbody.useGravity && updateMethod == UpdateMethod.Update)
            {
                m_vector = (target) ?
                    target.position - transform.position :
                    targetVector - transform.position;

                direction = m_vector.normalized;
                m_scaledForce = force * Mathf.Clamp01(scale);
                m_acceleration = m_rigidbody.mass * (direction * m_scaledForce);
                m_rigidbody.AddForce(m_acceleration, ForceMode.Force);
            }
        }

        protected void FixedUpdate()
        {
            if (m_rigidbody.useGravity && updateMethod == UpdateMethod.FixedUpdate)
            {
                m_vector = (target) ? 
                    target.position - transform.position :
                    targetVector - transform.position;

                direction = m_vector.normalized;
                m_scaledForce = force * Mathf.Clamp01(scale);
                m_acceleration = m_rigidbody.mass * (direction * m_scaledForce);
                m_rigidbody.AddForce(m_acceleration, ForceMode.Force);
            }
        }

        protected void LateUpdate()
        {
            if (m_rigidbody.useGravity && updateMethod == UpdateMethod.LateUpdate)
            {
                m_vector = (target) ?
                    target.position - transform.position :
                    targetVector - transform.position;

                direction = m_vector.normalized;
                m_scaledForce = force * Mathf.Clamp01(scale);
                m_acceleration = m_rigidbody.mass * (direction * m_scaledForce);
                m_rigidbody.AddForce(m_acceleration, ForceMode.Force);
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