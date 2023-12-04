using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Debug = DebugEngine.Debug;

namespace GameEngine
{
    [System.Serializable]
    public class Grid
    {
        //Properties
        public Vector3 length;
        public Vector3 size;
        public Vector3 space;
        public Vector3 position;

        public Action<Vector3> onGenerate;
        public bool isGenerated { get { return m_gridContainer != null && m_gridContainer.Length > 0; } }
        public Vector3 this[int x, int y, int z]
        {
            set { m_gridContainer[x, y, z] = value; }
            get { return m_gridContainer[x, y, z]; }
        }

        private Vector3[,,] m_gridContainer;
        private Vector3 m_gridContainerLength = Vector3.zero;
        private List<Vector3> m_gridContainerList = new List<Vector3>();

        #region Grid
        public Vector3[,,] Generate()
        {
            if (m_gridContainerLength != length)
            {
                m_gridContainerLength = length;
                m_gridContainer = new Vector3[(int)length.x, (int)length.y, (int)length.z];
                Debug.Log(String.Format("Grid Generated: [{0}-{1}-{2}]", length.x, length.y, length.z).Color(Color.green));
            }

            if (m_gridContainer == null) return null;

            Vector3 point = Vector3.zero;
            float[] val = new float[6];

            m_gridContainerList.Clear();
            for (int x = 0; x < length.x; x++)
            {
                for (int y = 0; y < length.y; y++)
                {
                    for (int z = 0; z < length.z; z++)
                    {
                        val[0] = position.x - (size.x * (length.x / 2.0f)) - (size.x / 2.0f) - space.x;
                        val[1] = position.y - (size.y * (length.y / 2.0f)) - (size.y / 2.0f) - space.y;
                        val[2] = position.z - (size.z * (length.z / 2.0f)) - (size.z / 2.0f) - space.z;

                        val[3] = size.x + x * (size.x + space.x) + val[0];
                        val[4] = size.y + y * (size.y + space.y) + val[1];
                        val[5] = size.z + z * (size.z + space.z) + val[2];

                        point = new Vector3(val[3], val[4], val[5]);
                        m_gridContainer[x, y, z] = point;
                        m_gridContainerList.Add(point);

                        if (onGenerate != null)
                        {
                            onGenerate.Invoke(point);
                        }
                    }
                }
            }

            return m_gridContainer;
        }

        public void Collision(LayerMask mask, bool fromCenter = false)
        {
            Vector3 origin;

            if (fromCenter)
            {
                GridInfo[] Data = GridCastPoint(position, mask);
                for (int i = 0; i < Data.Length; i++)
                {
                    RemoveTo(m_gridContainer[Data[i].x, Data[i].y, Data[i].z]);
                }
            }
            else
            {
                for (int x = 0; x < length.x; x++)
                {
                    for (int y = 0; y < length.y; y++)
                    {
                        for (int z = 0; z < length.z; z++)
                        {
                            origin = m_gridContainer[x, y, z];
                            if (GridCast(origin, mask))
                            {
                                RemoveTo(origin);
                            }
                        }
                    }
                }
            }

        }

        public bool Contains(Vector3 point, float threshold = 0.05f)
        {
            float distance = Mathf.Infinity;

            for (int x = 0; x < length.x; x++)
            {
                for (int y = 0; y < length.y; y++)
                {
                    for (int z = 0; z < length.z; z++)
                    {
                        distance = Vector3.Distance(m_gridContainer[x, y, z], point);

                        if (distance < Math.Abs(threshold))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public Vector3Int FindIndex(Vector3 point)
        {
            for (int x = 0; x < length.x; x++)
            {
                for (int y = 0; y < length.y; y++)
                {
                    for (int z = 0; z < length.z; z++)
                    {
                        if (m_gridContainer[x,y,z] == point)
                        {
                            return new Vector3Int(x, y, z);
                        }
                    }
                }
            }

            return new Vector3Int(-1, -1, -1);
        }

        public bool Clear()
        {
            m_gridContainer = new Vector3[0, 0, 0];
            m_gridContainerList.Clear();
            System.GC.Collect();
            return m_gridContainer == null;
        }

        public Vector3 Nearest(Vector3 point)
        {
            if (m_gridContainer == null)
                return point;

            bool valid;
            Vector3 distance, minDistance = Vector3.one * Mathf.Infinity, nearPoint = point;
            float start = Vector3.Distance(position, point);
            float end = Vector3.Distance(position, position + Vector3.Scale(length, size) / 2f);

            if (start > end) return point;

            for (int x = 0; x < length.x; x++)
            {
                for (int y = 0; y < length.y; y++)
                {
                    for (int z = 0; z < length.z; z++)
                    {
                        distance = m_gridContainer[x, y, z] - point;
                        valid = distance.magnitude < minDistance.magnitude;
                        if (valid && m_gridContainerList.Contains(m_gridContainer[x, y, z]))
                        {
                            minDistance = distance;
                            nearPoint = m_gridContainer[x, y, z];
                        }
                    }
                }
            }

            return nearPoint;
        }

        public Vector3 Longest(Vector3 point)
        {
            Vector3 distance, maxDistance = Vector3.zero, longPoint = point;

            for (int x = 0; x < length.x; x++)
            {
                for (int y = 0; y < length.y; y++)
                {
                    for (int z = 0; z < length.z; z++)
                    {
                        distance = m_gridContainer[x, y, z] - point;
                        if (distance.magnitude > maxDistance.magnitude)
                        {
                            maxDistance = distance;
                            longPoint = m_gridContainer[x, y, z];
                        }
                    }
                }
            }

            return longPoint;
        }

        public bool IsInRange(Vector3 point)
        {
            float start = Vector3.Distance(position, point);
            float end = Vector3.Distance(position, position + Vector3.Scale(length, size) / 2f);
            return start < end;
        }

        public Vector3[,,] GetGrid()
        {
            return m_gridContainer;
        }
        #endregion

        #region Dynamic
        public void AddTo(Vector3 item)
        {
            Vector3[] container = m_gridContainer.Cast<Vector3>().ToArray();
            if (Array.Exists(container, e => e == item) && !m_gridContainerList.Contains(item))
            {
                Debug.Log("Added " + item);
                m_gridContainerList.Add(item);
            }
        }

        public bool RemoveTo(Vector3 item)
        {
            Vector3[] container = m_gridContainer.Cast<Vector3>().ToArray();
            if (Array.Exists(container, e => e == item) && m_gridContainerList.Contains(item))
            {
                Debug.Log("Removed " + item);
                return m_gridContainerList.Remove(item);
            }

            return false;
        }

        public bool ContainsTo(Vector3 point, float threshold = 0.05f)
        {
            return m_gridContainerList.Contains(point) && Contains(point, threshold);
        }

        public void ResetTo()
        {
            m_gridContainerList.Clear();
            for (int x = 0; x < length.x; x++)
            {
                for (int y = 0; y < length.y; y++)
                {
                    for (int z = 0; z < length.z; z++)
                    {
                        m_gridContainerList.Add(m_gridContainer[x, y, z]);
                    }
                }
            }
        }
        #endregion

        #region Gizmos
        public void GizmosDrawGrid(Vector3 point, Color Color, Color Selected)
        {
            if (m_gridContainer != null)
            {
                bool valid;

                for (int x = 0; x < length.x; x++)
                {
                    for (int y = 0; y < length.y; y++)
                    {
                        for (int z = 0; z < length.z; z++)
                        {
                            position = m_gridContainer[x, y, z];
                            valid = m_gridContainerList.Contains(m_gridContainer[x, y, z]);

                            //Interator
                            Gizmos.color = valid ? Color : Color.red;
                            Gizmos.DrawWireCube(m_gridContainer[x, y, z], size);

                            //Nearest
                            Gizmos.color = Selected;
                            Gizmos.DrawCube(Nearest(point), size);
                        }
                    }
                }
            }
        }

        public void GizmosDrawBounds()
        {
            float x, y, z;
            x = size.x * length.x + space.x * length.x;
            y = size.y * length.y + space.y * length.y;
            z = size.z * length.z + space.z * length.z; 

            Gizmos.DrawWireCube(position, new Vector3(x,y,z));
        }

        #endregion

        #region Private
        private bool GridCast(Vector3 point, LayerMask mask)
        {
            bool valid = false;
            Ray[] directions = new Ray[6];
            directions[0] = new Ray(point, Vector3.up);
            directions[1] = new Ray(point, Vector3.down);
            directions[2] = new Ray(point, Vector3.left);
            directions[3] = new Ray(point, Vector3.right);
            directions[4] = new Ray(point, Vector3.forward);
            directions[5] = new Ray(point, Vector3.back);
            RaycastHit hit;

            float[] distance = new float[] {
            size.y/2, size.y/2,
            size.x/2, size.x/2,
            size.z/2, size.z/2,
        };

            for (int i = 0; i < directions.Length; i++)
            {
                if (i == 0)
                {
                    valid = Physics.Raycast(directions[i], out hit, distance[i], mask);
                }
                else
                {
                    valid |= Physics.Raycast(directions[i], out hit, distance[i], mask);
                }
            }

            return valid;
        }

        private GridInfo[] GridCastPoint(Vector3 point, LayerMask mask)
        {
            int totalLength = (int)length.x * (int)length.y * (int)length.z;
            GridInfo[] Result = new GridInfo[totalLength];

            bool isHit;
            RaycastHit hit;
            Vector3 direction;
            float distance;

            int i = 0;
            for (int x = 0; x < length.x; x++)
            {
                for (int y = 0; y < length.y; y++)
                {
                    for (int z = 0; z < length.z; z++)
                    {
                        direction = m_gridContainer[x, y, z] - point;
                        distance = Vector3.Distance(point, m_gridContainer[x, y, z]);
                        isHit = Physics.Raycast(point, direction.normalized, out hit, distance, mask);
                        Result[i] = new GridInfo(x, y, z, hit, isHit, hit.point);
                    }
                }
            }

            return Result;
        }
        #endregion

    }

    internal struct GridInfo
    {
        public GridInfo(int x, int y, int z, RaycastHit hit, bool isHit, Vector3 point)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.hit = hit;
            this.isHit = isHit;
            this.point = point;
        }

        public int x, y, z;
        public Vector3 point;
        public RaycastHit hit;
        public bool isHit;
    }
}
