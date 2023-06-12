using UnityEngine;

namespace GameEngine.Translation
{
    [System.Serializable]
    public class Transform
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public Transform()
        {
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
            Scale = Vector3.one;
        }

        public Transform(UnityEngine.Transform NewTransform)
        {
            Position = NewTransform.position;
            Rotation = NewTransform.rotation;
            Scale = NewTransform.localScale;
        }

        public override string ToString()
        {
            return "[Position: " + Position + " Rotation: " + Rotation.eulerAngles + " Scale: " + Scale + "]";
        }
    }
}

