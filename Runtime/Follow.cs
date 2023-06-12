using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Debug = DebugEngine.Debug;

namespace GameEngine
{
    public class Follow : MonoBehaviour
    {
        [Header("Properties")]
        [Tooltip("the target transform to be followed.")]
        public Transform Target;
        [Tooltip("In which type of loop the motion transformation has to be executed.")]
        public UpdateMethod Loop;
        [Tooltip("The type of interpolation to be applied to this object during the motion. (translation)")]
        public InterpolationType Interpolation;
        [Tooltip("Smooth value of the calculation formula.")]
        public float Smooth = 1.0f;
        [Tooltip("[Enable/Disable] the delta time usage in the formula.")]
        public bool DeltaTime = true;
        [Tooltip("Blend between the target position and the clamp disntaces. (0 = Unclamp / 1 = Clamp)")]
        [Range(0.0f, 1.0f)] public float BlendDistance;
        [Tooltip("The minimun distance between this transform and the target transform")]
        public float MinDistance = 0.0f;
        [Tooltip("The maximun distance between this transform and the target transform (distances above make object stop moving)")]
        public float MaxDistance = Mathf.Infinity;
        [Tooltip("Offset vector of the current target position (zero represents the target position)")]
        public Vector3 Offset = Vector3.zero;
        [Tooltip("How much need the transform speed up to be detect motion.")]
        public float VelocityThreshold = 0.1f;
        [Tooltip("Enable/Disable Debug console info and transform gizmos")]
        public bool EnableDebug;

        [Header("Events")]
        [SerializeField] public UnityEvent OnFollow = new UnityEvent();

        //Public Fields
        public Vector3 Velocity { internal set; get; }
        public Vector3 StartPosition { internal set; get; }

        //Internal
        protected Vector3 Distance, PreviusPosition;
        protected Vector3 TargetPosition;

        protected virtual void Awake()
        {
            if (GetComponentInChildren<Rigidbody>()) {
                Debug.LogWarning("The follow object cant contains a physics component.", gameObject, EnableDebug);
                Debug.LogWarning("Deactivating script", gameObject, EnableDebug);
                enabled = false;
            }
        }

        protected virtual void Start()
        {
            if (!Target)
            {
                Debug.LogError("The Target value on follow object cannot be null.", gameObject, EnableDebug);
                Debug.LogError("Deactivating script", gameObject, EnableDebug);
                enabled = false;
            }
            
            StartPosition = transform.position;
        }

        protected virtual void Update()
        {
            if (Loop == UpdateMethod.Update && Target)
            {
                Velocity = (transform.position - PreviusPosition) / Time.deltaTime;
                PreviusPosition = transform.position;
                if (Velocity.magnitude > VelocityThreshold) { OnFollow.Invoke(); }
                float Delta = DeltaTime ? Time.deltaTime * Smooth : Smooth;
                Distance = Target.position - transform.position;
                Vector3 Direction = transform.TransformDirection(Distance);
                Vector3 Position = Target.position + Direction.normalized * -Mathf.Abs(MinDistance);
                Position = (Distance.magnitude < MaxDistance) ? Position + Offset : transform.position;
                
                TargetPosition = Vector3.Lerp(Target.position + Offset, Position, BlendDistance);
                switch (Interpolation)
                {
                    case InterpolationType.Linear:
                        transform.position = Vector3.Lerp(transform.position, TargetPosition, Delta);
                        break;
                    case InterpolationType.Spherical:
                        transform.position = Vector3.Slerp(transform.position, TargetPosition, Delta);
                        break;
                    default: transform.position = TargetPosition; break;
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (Loop == UpdateMethod.FixedUpdate && Target)
            {
                Velocity = (transform.position - PreviusPosition) / Time.fixedDeltaTime;
                PreviusPosition = transform.position;
                if (Velocity.magnitude > VelocityThreshold) { OnFollow.Invoke(); }
                float Delta = DeltaTime ? Time.fixedDeltaTime * Smooth : Smooth;
                Distance = Target.position - transform.position;
                Vector3 Direction = transform.TransformDirection(Distance);
                Vector3 Position = Target.position + Direction.normalized * -Mathf.Abs(MinDistance);
                Position = (Distance.magnitude < MaxDistance) ? Position + Offset : transform.position;

                TargetPosition = Vector3.Lerp(Target.position + Offset, Position, BlendDistance);
                switch (Interpolation)
                {
                    case InterpolationType.Linear:
                        transform.position = Vector3.Lerp(transform.position, TargetPosition, Delta);
                        break;
                    case InterpolationType.Spherical:
                        transform.position = Vector3.Slerp(transform.position, TargetPosition, Delta);
                        break;
                    default: transform.position = TargetPosition; break;
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if (Loop == UpdateMethod.LateUpdate && Target)
            {
                Velocity = (transform.position - PreviusPosition) / Time.deltaTime;
                PreviusPosition = transform.position;
                if (Velocity.magnitude > VelocityThreshold) { OnFollow.Invoke(); }
                float Delta = DeltaTime ? Time.deltaTime * Smooth : Smooth;
                Distance = Target.position - transform.position;
                Vector3 Direction = transform.TransformDirection(Distance);
                Vector3 Position = Target.position + Direction.normalized * -Mathf.Abs(MinDistance);
                Position = (Distance.magnitude < MaxDistance) ? Position + Offset : transform.position;

                TargetPosition = Vector3.Lerp(Target.position + Offset, Position, BlendDistance);
                switch (Interpolation)
                {
                    case InterpolationType.Linear:
                        transform.position = Vector3.Lerp(transform.position, TargetPosition, Delta);
                        break;
                    case InterpolationType.Spherical:
                        transform.position = Vector3.Slerp(transform.position, TargetPosition, Delta);
                        break;
                    default: transform.position = TargetPosition; break;
                }
            }
        }

        //Gizmos
        private void OnDrawGizmos()
        {
            if (EnableDebug && Target)
            {
                Gizmos.color = (Distance.magnitude > MaxDistance && BlendDistance > 0) 
                    ? Color.red : Color.green;
                Gizmos.DrawLine(transform.position, Target.position);
                Gizmos.DrawWireSphere(Target.position, 0.1f);
            }
        }
    }

}
