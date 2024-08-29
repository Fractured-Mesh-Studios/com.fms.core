using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreEngine.Occlusion
{
    public class Occludee : MonoBehaviour
    {
        public Vector3 expand = Vector3.one;
        public LayerMask mask;
        public float coldown = 0.5f;
        public UpdateMethod updateMethod;

        public System.Action onEnable;
        public System.Action onDisable;

        public float maxDistance = 50f;
        public float threshold = 1f;

        public GameObject[] objects;
        public Behaviour[] behaviors;

        //private
        private bool m_isVisible = true;
        private bool m_canChange = true;

        //plane
        private bool m_planeValid = false;
        private bool m_planeLastValid = false;

        private Renderer[] m_renderer;

        private Camera m_camera;
        private RaycastHit m_hit;
        private Plane[] m_plane;
        private Bounds m_bounds;

        public bool isVisible { get { return m_isVisible; } }

        #region UNITY
        private void Awake()
        {
            m_camera = Camera.main;

            m_renderer = DetectComponents<Renderer>();
            if(m_renderer == null) 
            {
                if (!isVisible)
                {
                    Enable();
                    m_renderer = DetectComponents<Renderer>();
                    Disable();
                }
                else
                {
                    Debug.LogError("Renderers not detected on awake", gameObject);
                }
            }

            m_planeLastValid = !m_planeValid;
        }

        private void Update()
        {
            if(updateMethod == UpdateMethod.Update)
            {
                CalculateOcclusion();
            }
        }

        private void FixedUpdate()
        {
            if (updateMethod == UpdateMethod.FixedUpdate)
            {
                CalculateOcclusion();
            }
        }

        private void LateUpdate()
        {
            if (updateMethod == UpdateMethod.LateUpdate)
            {
                CalculateOcclusion();
            }
        }
        #endregion

        #region OCCLUSION
        private void CalculateOcclusion()
        {
            m_plane = GeometryUtility.CalculateFrustumPlanes(m_camera);
            m_planeValid = GeometryUtility.TestPlanesAABB(m_plane, GetBounds());

            Vector3 start = m_camera.transform.position;
            Vector3 end = transform.position;

            if (Vector3.Distance(start, end) > maxDistance)
            {
                if (m_planeValid != m_planeLastValid)
                {
                    Disable();
                }

                return;
            }

            if (m_planeValid != m_planeLastValid)
            {
                if (m_planeValid)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }

            m_planeLastValid = m_planeValid;
        }

        private bool CalculateVisibility()
        {
            #if UNITY_EDITOR
            if (Camera.current != null) 
                return false;
            #endif

            int count = 0;
            for(int i = 0;i < m_renderer.Length; i++)
            {
                count = m_renderer[i].isVisible ? 1 : 0;
            }
            return count > 0;
        }

        private bool CalculateBoundPoints(float threshold = 0.2f, float max = Mathf.Infinity)
        {
            Vector3 current;
            Vector3 start = m_camera.transform.position;
            float distance, subDistance;
            int count = 0;

            var m_boundPoint = GetBounds().GetBoundPoints();
            for (int i = 0; i < m_boundPoint.Length; i++)
            {
                //current = transform.TransformPoint(m_boundPoint[i]);
                current = m_boundPoint[i];
                distance = Vector3.Distance(start, current);

                if (Physics.Linecast(start, current, out m_hit, mask))
                {
                    //Hit Something
                    subDistance = Mathf.Abs(distance - m_hit.distance);

                    if (subDistance < threshold)
                    {
                        //Distance Valid
                        count++;
                    }
                    else
                    {
                        //Distance Invalid
                    }
                }
                else { count += (distance < max) ? 1 : 0; }
            }

            return count != 0;
        }

        private bool CalculateBlock(Vector3 start, Vector3 end, float radius)
        {
            Vector3 direction = end - start;
            float distance = Vector3.Distance(start, end);
            Ray ray = new Ray(start, direction.normalized);
            if(Physics.SphereCast(ray, radius, distance + 1f, mask))
            {
                if (Vector3.Distance(start, m_hit.point) < distance - threshold)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void Enable()
        {
            if (m_canChange && !m_isVisible)
            {
                m_isVisible = true;
                m_canChange = false;

                for (int i = 0; i < behaviors.Length; i++)
                {
                    behaviors[i].enabled = true;
                }

                for(int i = 0;i < objects.Length; i++) 
                {
                    objects[i].SetActive(true);
                }

                Invoke(nameof(OnOccludeeEnable), coldown);
            }
        }

        public virtual void Disable()
        {
            if (m_canChange && m_isVisible)
            {
                m_isVisible = false;
                m_canChange = false;

                for (int i = 0; i < behaviors.Length; i++)
                {
                    behaviors[i].enabled = false;
                }

                for (int i = 0; i < objects.Length; i++)
                {
                    objects[i].SetActive(false);
                }

                Invoke(nameof(OnOccludeeDisable), coldown);
            }
        }

        public Bounds GetBounds()
        {
            return GetBounds(expand);
        }

        public Bounds GetBounds(Vector3 expand)
        {
            if(m_renderer == null) { return new Bounds(); }
            
            Bounds current = new Bounds();
            float maxMagnitude = 0f;

            for(int i = 0; i < m_renderer.Length; i++)
            {
                float magnitude = m_renderer[i].bounds.size.magnitude;
                if(magnitude > maxMagnitude)
                {
                    maxMagnitude = magnitude;
                    current = m_renderer[i].bounds;
                }
            }

            current.Expand(expand);
            current.center = transform.position;
            return current;
        }
        #endregion

        #region CALLBACK
        private void OnOccludeeEnable()
        {
            m_canChange = true;
            onEnable?.Invoke();
        }

        private void OnOccludeeDisable()
        {
            m_canChange = true;
            onDisable?.Invoke();
        }
        #endregion

        #region DETECTION
        private T[] DetectComponents<T>() where T : Component
        {
            T[] result = GetComponents<T>();
            if (result == null) result = GetComponentsInChildren<T>();
            if (result == null) result = GetComponentsInParent<T>();
            return result;
        }
        #endregion

        #region GIZMOS
        private void OnDrawGizmosSelected()
        {
            Bounds bounds = GetBounds();
            Gizmos.color = Color.white;
            bounds.Expand(expand);
            Gizmos.DrawWireCube(bounds.center, bounds.extents);
        }
        #endregion
    }
}
