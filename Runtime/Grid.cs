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
        public Vector3 Length;
        public Vector3 Size;
        public Vector3 Space;
        public Vector3 Position;

        public Action<Vector3> OnGenerate;
        public bool IsGenerated { get { return GridContainer != null && GridContainer.Length > 0; } }
        public Vector3 this[int x, int y, int z]
        {
            set { GridContainer[x, y, z] = value; }
            get { return GridContainer[x, y, z]; }
        }

        private Vector3[,,] GridContainer;
        private Vector3 GridContainerLength = Vector3.zero;
        private List<Vector3> GridContainerList = new List<Vector3>();

        #region Grid
        public Vector3[,,] Generate()
        {
            if (GridContainerLength != Length)
            {
                GridContainerLength = Length;
                GridContainer = new Vector3[(int)Length.x, (int)Length.y, (int)Length.z];
                Debug.Log(String.Format("Grid Generated: [{0}-{1}-{2}]", Length.x, Length.y, Length.z).Color(Color.green));
            }

            if (GridContainer == null) return null;

            Vector3 Point = Vector3.zero;
            float[] val = new float[6];

            GridContainerList.Clear();
            for (int x = 0; x < Length.x; x++)
            {
                for (int y = 0; y < Length.y; y++)
                {
                    for (int z = 0; z < Length.z; z++)
                    {
                        val[0] = Position.x - (Size.x * (Length.x / 2.0f)) - (Size.x / 2.0f) - Space.x;
                        val[1] = Position.y - (Size.y * (Length.y / 2.0f)) - (Size.y / 2.0f) - Space.y;
                        val[2] = Position.z - (Size.z * (Length.z / 2.0f)) - (Size.z / 2.0f) - Space.z;

                        val[3] = Size.x + x * (Size.x + Space.x) + val[0];
                        val[4] = Size.y + y * (Size.y + Space.y) + val[1];
                        val[5] = Size.z + z * (Size.z + Space.z) + val[2];

                        Point = new Vector3(val[3], val[4], val[5]);
                        GridContainer[x, y, z] = Point;
                        GridContainerList.Add(Point);

                        if (OnGenerate != null)
                        {
                            OnGenerate.Invoke(Point);
                        }
                    }
                }
            }

            return GridContainer;
        }

        public void Collision(LayerMask Mask, bool FromCenter = false)
        {
            Vector3 Origin;

            if (FromCenter)
            {
                GridInfo[] Data = GridCastPoint(Position, Mask);
                for (int i = 0; i < Data.Length; i++)
                {
                    RemoveTo(GridContainer[Data[i].x, Data[i].y, Data[i].z]);
                }
            }
            else
            {
                for (int x = 0; x < Length.x; x++)
                {
                    for (int y = 0; y < Length.y; y++)
                    {
                        for (int z = 0; z < Length.z; z++)
                        {
                            Origin = GridContainer[x, y, z];
                            if (GridCast(Origin, Mask))
                            {
                                RemoveTo(Origin);
                            }
                        }
                    }
                }
            }

        }

        public bool Contains(Vector3 Point, float Threshold = 0.05f)
        {
            float Distance = Mathf.Infinity;

            for (int x = 0; x < Length.x; x++)
            {
                for (int y = 0; y < Length.y; y++)
                {
                    for (int z = 0; z < Length.z; z++)
                    {
                        Distance = Vector3.Distance(GridContainer[x, y, z], Point);

                        if (Distance < Math.Abs(Threshold))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public Vector3Int FindIndex(Vector3 Point)
        {
            for (int x = 0; x < Length.x; x++)
            {
                for (int y = 0; y < Length.y; y++)
                {
                    for (int z = 0; z < Length.z; z++)
                    {
                        if (GridContainer[x,y,z] == Point)
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
            GridContainer = new Vector3[0, 0, 0];
            GridContainerList.Clear();
            System.GC.Collect();
            return GridContainer == null;
        }

        public Vector3 Nearest(Vector3 Point)
        {
            if (GridContainer == null)
                return Point;

            bool Valid;
            Vector3 Distance, MinDistance = Vector3.one * Mathf.Infinity, NearPoint = Point;
            float Start = Vector3.Distance(Position, Point);
            float End = Vector3.Distance(Position, Position + Vector3.Scale(Length, Size) / 2f);

            if (Start > End) return Point;

            for (int x = 0; x < Length.x; x++)
            {
                for (int y = 0; y < Length.y; y++)
                {
                    for (int z = 0; z < Length.z; z++)
                    {
                        Distance = GridContainer[x, y, z] - Point;
                        Valid = Distance.magnitude < MinDistance.magnitude;
                        if (Valid && GridContainerList.Contains(GridContainer[x, y, z]))
                        {
                            MinDistance = Distance;
                            NearPoint = GridContainer[x, y, z];
                        }
                    }
                }
            }

            return NearPoint;
        }

        public Vector3 Longest(Vector3 Point)
        {
            Vector3 Distance, MaxDistance = Vector3.zero, LongPoint = Point;

            for (int x = 0; x < Length.x; x++)
            {
                for (int y = 0; y < Length.y; y++)
                {
                    for (int z = 0; z < Length.z; z++)
                    {
                        Distance = GridContainer[x, y, z] - Point;
                        if (Distance.magnitude > MaxDistance.magnitude)
                        {
                            MaxDistance = Distance;
                            LongPoint = GridContainer[x, y, z];
                        }
                    }
                }
            }

            return LongPoint;
        }

        public bool IsInRange(Vector3 Point)
        {
            float Start = Vector3.Distance(Position, Point);
            float End = Vector3.Distance(Position, Position + Vector3.Scale(Length, Size) / 2f);
            return Start < End;
        }

        public Vector3[,,] GetGrid()
        {
            return GridContainer;
        }
        #endregion

        #region Dynamic
        public void AddTo(Vector3 Item)
        {
            Vector3[] Container = GridContainer.Cast<Vector3>().ToArray();
            if (Array.Exists(Container, e => e == Item) && !GridContainerList.Contains(Item))
            {
                Debug.Log("Added " + Item);
                GridContainerList.Add(Item);
            }
        }

        public bool RemoveTo(Vector3 Item)
        {
            Vector3[] Container = GridContainer.Cast<Vector3>().ToArray();
            if (Array.Exists(Container, e => e == Item) && GridContainerList.Contains(Item))
            {
                Debug.Log("Removed " + Item);
                return GridContainerList.Remove(Item);
            }

            return false;
        }

        public bool ContainsTo(Vector3 Point, float Threshold = 0.05f)
        {
            return GridContainerList.Contains(Point) && Contains(Point, Threshold);
        }

        public void ResetTo()
        {
            GridContainerList.Clear();
            for (int x = 0; x < Length.x; x++)
            {
                for (int y = 0; y < Length.y; y++)
                {
                    for (int z = 0; z < Length.z; z++)
                    {
                        GridContainerList.Add(GridContainer[x, y, z]);
                    }
                }
            }
        }
        #endregion

        #region Gizmos
        public void GizmosDrawGrid(Vector3 Point, Color Color, Color Selected)
        {
            if (GridContainer != null)
            {
                bool Valid;

                for (int x = 0; x < Length.x; x++)
                {
                    for (int y = 0; y < Length.y; y++)
                    {
                        for (int z = 0; z < Length.z; z++)
                        {
                            Position = GridContainer[x, y, z];
                            Valid = GridContainerList.Contains(GridContainer[x, y, z]);

                            //Interator
                            Gizmos.color = Valid ? Color : Color.red;
                            Gizmos.DrawWireCube(GridContainer[x, y, z], Size);

                            //Nearest
                            Gizmos.color = Selected;
                            Gizmos.DrawCube(Nearest(Point), Size);
                        }
                    }
                }
            }
        }

        public void GizmosDrawBounds()
        {
            float x, y, z;
            x = Size.x * Length.x + Space.x * Length.x;
            y = Size.y * Length.y + Space.y * Length.y;
            z = Size.z * Length.z + Space.z * Length.z; 

            Gizmos.DrawWireCube(Position, new Vector3(x,y,z));
        }

        #endregion

        #region Private
        private bool GridCast(Vector3 Point, LayerMask Mask)
        {
            bool Valid = false;
            Ray[] Directions = new Ray[6];
            Directions[0] = new Ray(Point, Vector3.up);
            Directions[1] = new Ray(Point, Vector3.down);
            Directions[2] = new Ray(Point, Vector3.left);
            Directions[3] = new Ray(Point, Vector3.right);
            Directions[4] = new Ray(Point, Vector3.forward);
            Directions[5] = new Ray(Point, Vector3.back);
            RaycastHit Hit;

            float[] Distance = new float[] {
            Size.y/2, Size.y/2,
            Size.x/2, Size.x/2,
            Size.z/2, Size.z/2,
        };

            for (int i = 0; i < Directions.Length; i++)
            {
                if (i == 0)
                {
                    Valid = Physics.Raycast(Directions[i], out Hit, Distance[i], Mask);
                }
                else
                {
                    Valid |= Physics.Raycast(Directions[i], out Hit, Distance[i], Mask);
                }
            }

            return Valid;
        }

        private GridInfo[] GridCastPoint(Vector3 Point, LayerMask Mask)
        {
            int TotalLength = (int)Length.x * (int)Length.y * (int)Length.z;
            GridInfo[] Result = new GridInfo[TotalLength];

            bool isHit;
            RaycastHit Hit;
            Vector3 Direction;
            float Distance;

            int i = 0;
            for (int x = 0; x < Length.x; x++)
            {
                for (int y = 0; y < Length.y; y++)
                {
                    for (int z = 0; z < Length.z; z++)
                    {
                        Direction = GridContainer[x, y, z] - Point;
                        Distance = Vector3.Distance(Point, GridContainer[x, y, z]);
                        isHit = Physics.Raycast(Point, Direction.normalized, out Hit, Distance, Mask);
                        Result[i] = new GridInfo(x, y, z, Hit, isHit, Hit.point);
                    }
                }
            }

            return Result;
        }
        #endregion

    }

    internal struct GridInfo
    {
        public GridInfo(int x, int y, int z, RaycastHit Hit, bool IsHit, Vector3 Point)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.Hit = Hit;
            this.IsHit = IsHit;
            this.Point = Point;
        }

        public int x, y, z;
        public Vector3 Point;
        public RaycastHit Hit;
        public bool IsHit;
    }
}
/*
    float lx = Location.x - (Size.x * (Length.x / 2.0f)) - (Size.x / 2.0f) - Space.x;
    float ly = Location.y - (Size.y * (Length.y / 2.0f)) - (Size.y / 2.0f) - Space.y;
    float lz = Location.z - (Size.z * (Length.z / 2.0f)) - (Size.z / 2.0f) - Space.z;

    float wx = Size.x + x * (Size.x + Space.x) + lx;
    float wy = Size.y + y * (Size.y + Space.y) + ly;
    float wz = Size.z + z * (Size.z + Space.z) + lz;
*/