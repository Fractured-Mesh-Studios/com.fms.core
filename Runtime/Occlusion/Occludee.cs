using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Debug = DebugEngine.Debug;
using System.Linq;
using Codice.Client.BaseCommands.BranchExplorer;
using System.Diagnostics.Tracing;
using PlasticPipe.PlasticProtocol.Messages;
using static Codice.CM.Common.CmCallContext;

namespace CoreEngine.Occlusion
{
    public class Occludee : MonoBehaviour
    {
        public bool detectRenderer = true;
        public Vector3 expand = Vector3.one;
        public LayerMask mask;
        public float coldown = 0.5f;
        public float tick = 0.01f;

        public GameObject[] objects;
        public Behaviour[] behaviors;

        //private
        private bool m_isVisible = true;
        private bool m_canChange = true;
        private bool m_planeValid = false;
        private bool m_planeCache = false;

        private Renderer[] m_renderer;
        private Collider[] m_collider;
        private Camera m_camera;
        private RaycastHit m_hit;
        private Plane[] m_plane;

        public bool isVisible { get { return m_isVisible; } }

        void Awake()
        {
            m_camera = Camera.main;
            m_renderer = DetectComponents<Renderer>();
            m_collider = DetectComponents<Collider>();

            if(m_renderer == null) { Debug.LogError("Renderers not detected on awake", gameObject); }
            if(m_collider == null) { Debug.LogError("Colliders not detected on awakke", gameObject); }
        }

        private void Update()
        {
            Bounds bounds = GetBounds();

            m_plane = GeometryUtility.CalculateFrustumPlanes(m_camera);
            m_planeValid = GeometryUtility.TestPlanesAABB(m_plane, GetBounds());

            Vector3 start = m_camera.transform.position;
            Vector3 end = transform.position;

            if (m_planeValid)
            {
                if (CalculateBoundPoints(0.5f))
                {
                    Debug.Log("Visible".Color(Color.green));
                    Enable();
                } 
                else 
                {
                    Debug.Log("Invisible".Color(Color.red));
                    Disable();
                }
            }
            else
            {
                Disable();
            }

            m_planeCache = m_planeValid;
        }

        /*private void CalculateVisibility(Vector3 start, Vector3 end)
        {
            if (Physics.Linecast(start, end, out m_hit, mask))
            {
                if (m_collider.Contains(m_hit.collider))
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }
        }*/

        private bool Visibility()
        {
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
                current = transform.TransformPoint(m_boundPoint[i]);
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

            return count > 0;
        }

        public void Enable()
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

        public void Disable()
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
            return current;
        }

        public Collider[] GetColliders()
        {
            return m_collider;
        }

        #region CALLBACK
        private void OnOccludeeEnable()
        {
            m_canChange = true; 
        }

        private void OnOccludeeDisable()
        {
            m_canChange = true;
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
        private void DrawBoundsGizmos(float threshold)
        {
            Vector3 start = m_camera.transform.position;
            Vector3 current = Vector3.zero;
            float distance, subDistance;

            var m_boundPoint = GetBounds().GetBoundPoints();
            for (int i = 0; i < m_boundPoint.Length; i++)
            {
                Gizmos.color = Color.red;
                //current = m_boundPoint[i] + transform.position;
                current = transform.TransformPoint(m_boundPoint[i]);

                if (Physics.Linecast(start, current, out m_hit, mask))
                {
                    distance = Vector3.Distance(start, current);
                    subDistance = Mathf.Abs(distance - m_hit.distance);

                    if (subDistance < threshold)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {

                    }

                    Gizmos.DrawLine(start, m_hit.point);
                    Gizmos.DrawSphere(m_hit.point, 0.1f);
                }

                //Gizmos.DrawSphere(current, 0.1f);
                //Gizmos.DrawLine(start, current);
            }
        }

        private void OnDrawGizmos()
        {
            if (m_camera)
            {
                DrawBoundsGizmos(0.2f);
            }
            else { m_camera = Camera.main; }
            
            if(m_renderer != null && m_renderer.Length > 0)
            {
                /*Bounds bounds = GetBounds();

                foreach (var b in bounds.GetBoundPoints())
                    Gizmos.DrawSphere(b, 0.1f);

                Gizmos.color = Color.white;
                bounds.Expand(expand);
                Gizmos.DrawWireCube(bounds.center, bounds.size);*/

            }
            else
            {
                m_renderer = DetectComponents<Renderer>();
            }
        }
        #endregion
    }
}
