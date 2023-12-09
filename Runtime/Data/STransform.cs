using UnityEngine;

namespace GameEngine.Data
{
    [System.Serializable]
    public class STransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public STransform()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;
        }

        public STransform(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }

        public override string ToString()
        {
            return "[Position: " + position + " Rotation: " + rotation.eulerAngles + " Scale: " + scale + "]";
        }
    }
}

