using UnityEngine;

namespace GameEngine.Translation
{
    [System.Serializable]
    public class Transform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public Transform()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;
        }

        public Transform(UnityEngine.Transform NewTransform)
        {
            position = NewTransform.position;
            rotation = NewTransform.rotation;
            scale = NewTransform.localScale;
        }

        public override string ToString()
        {
            return "[Position: " + position + " Rotation: " + rotation.eulerAngles + " Scale: " + scale + "]";
        }
    }
}

