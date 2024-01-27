using System.Linq;
using UnityEngine;

namespace CoreEngine.Occlusion
{
    public class OcclusionManager : MonoBehaviour
    {
        public new Camera camera;

        public LayerMask mask;

        public string filterTag = "Occlusion";

        private Plane[] m_planes;
        private Occludee[] m_occludees;
        private Behaviour[] m_behaviours;

        private IOccludee[] m_iOccludees;

        private void Start()
        {
            Refresh();
        }

        private void Update()
        {
            Calculate();
        }

        private void Calculate()
        {
            bool valid;
            Bounds bounds;
            m_planes = GeometryUtility.CalculateFrustumPlanes(camera);
            Vector3 cameraPosition = camera.transform.position;

            //interface object
            if (m_iOccludees != null)
            {
                for (int i = 0; i < m_iOccludees.Length; i++)
                {
                    bounds = m_iOccludees[i].GetBounds();
                    valid = GeometryUtility.TestPlanesAABB(m_planes, bounds);

                    if (valid)
                        m_iOccludees[i].Enable();
                    else
                        m_iOccludees[i].Disable();
                }
            }

            valid = true;
            //occludee object
            if (m_occludees != null)
            {
                for (int i = 0; i < m_occludees.Length; i++)
                {
                    bounds = m_occludees[i].GetBounds();
                    valid = GeometryUtility.TestPlanesAABB(m_planes, bounds);
                    valid &= !Blocked(cameraPosition, bounds, m_occludees[i]);

                    if (valid)
                        m_occludees[i].Enable();
                    else
                        m_occludees[i].Disable();
                }
            }
        }

        private void Refresh()
        {
            m_occludees = FindObjectsByType<Occludee>(FindObjectsSortMode.None);
            m_behaviours = FindObjectsByType<Behaviour>(FindObjectsSortMode.None);
            m_iOccludees = m_behaviours.OfType<IOccludee>().ToArray();
        }

        private bool Blocked(Vector3 position, Bounds bounds, Occludee element)
        {
            float threshold = bounds.extents.magnitude;
            if(Intercept(position, bounds, threshold))
            {
                return true;
            }
            else
            {
                int wallHit = 0;
                Vector3[] points = bounds.GetBoundPoints();

                for (int i = 0; i < points.Length; i++)
                {
                    wallHit += Intercept(position, points[i], 0.1f) ? 1 : 0;
                }

                return wallHit == points.Length;
            }
        }

        private bool Intercept(Vector3 start, Bounds bounds, float threshold = 0.1f)
        {
            return Intercept(start, bounds.center, threshold);
        }

        private bool Intercept(Vector3 start, Vector3 end, float threshold = 0.1f)
        {
            RaycastHit hit;
            float distance = Vector3.Distance(start, end);
            if(Physics.Linecast(start, end, out hit, mask))
            {
                if(Mathf.Abs(distance - hit.distance) < threshold)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        #region GIZMOS
        private void DrawRayGizmos(Vector3 position, Bounds bounds)
        {
            Vector3[] points = bounds.GetBoundPoints();

            Vector3 direction = Vector3.zero;

            for (int i = 0; i < points.Length; i++)
            {
                direction = points[i] - position;

                Gizmos.DrawRay(position, direction);

            }

            direction = bounds.center - position;
            Gizmos.DrawRay(position, direction);

        }

        private void OnDrawGizmos()
        {
            if (m_occludees == null) return;

            bool valid = true;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            m_planes = GeometryUtility.CalculateFrustumPlanes(camera);

            for (int i = 0; i < m_occludees.Length; i++)
            {
                bounds = m_occludees[i].GetBounds();
                valid = GeometryUtility.TestPlanesAABB(m_planes, bounds);

                Gizmos.color = valid ? Color.green : Color.red;
                Gizmos.DrawWireCube(m_occludees[i].transform.position, bounds.size);
                DrawRayGizmos(camera.transform.position, bounds);
            }
        }
        #endregion
    }
}
