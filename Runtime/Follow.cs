using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace CoreEngine
{
    public class Follow : MonoBehaviour
    {
        [Header("Properties")]
        [Tooltip("the target transform to be followed.")]
        public Transform target;
        [Tooltip("In which type of loop the motion transformation has to be executed.")]
        public UpdateMethod loop;
        [Tooltip("The type of interpolation to be applied to this object during the motion. (translation)")]
        public InterpolationType interpolation;
        [Tooltip("smooth value of the calculation formula.")]
        public float smooth = 1.0f;
        [Tooltip("[Enable/Disable] the delta time usage in the formula.")]
        public bool deltaTime = true;
        [Tooltip("Blend between the target position and the clamp disntaces. (0 = Unclamp / 1 = Clamp)")]
        [Range(0.0f, 1.0f)] public float blendDistance;
        [Tooltip("The minimun distance between this transform and the target transform")]
        public float minDistance = 0.0f;
        [Tooltip("The maximun distance between this transform and the target transform (distances above make object stop moving)")]
        public float maxDistance = Mathf.Infinity;
        [Tooltip("offset vector of the current target position (zero represents the target position)")]
        public Vector3 offset = Vector3.zero;
        [Tooltip("How much need the transform speed up to be detect motion.")]
        public float velocityThreshold = 0.1f;
        [Tooltip("Enable/Disable Debug console info and transform gizmos")]
        public bool enableDebug;

        [Header("Events")]
        [SerializeField] public UnityEvent onFollow = new UnityEvent();

        //Public Fields
        public Vector3 velocity { internal set; get; }
        public Vector3 startPosition { internal set; get; }

        //Internal
        protected Vector3 distance, previusPosition;
        protected Vector3 targetPosition;

        protected virtual void Awake()
        {
            if (GetComponentInChildren<Rigidbody>()) {
                Debug.LogWarning("The follow object cant contains a physics component.", gameObject);
                Debug.LogWarning("Deactivating script", gameObject);
                enabled = false;
            }
        }

        protected virtual void Start()
        {
            if (!target)
            {
                Debug.LogError("The target value on follow object cannot be null.", gameObject);
                Debug.LogError("Deactivating script", gameObject);
                enabled = false;
            }
            
            startPosition = transform.position;
        }

        protected virtual void Update()
        {
            if (loop == UpdateMethod.Update && target)
            {
                velocity = (transform.position - previusPosition) / Time.deltaTime;
                previusPosition = transform.position;
                if (velocity.magnitude > velocityThreshold) { onFollow.Invoke(); }
                float Delta = deltaTime ? Time.deltaTime * smooth : smooth;
                distance = target.position - transform.position;
                Vector3 Direction = transform.TransformDirection(distance);
                Vector3 Position = target.position + Direction.normalized * -Mathf.Abs(minDistance);
                Position = (distance.magnitude < maxDistance) ? Position + offset : transform.position;
                
                targetPosition = Vector3.Lerp(target.position + offset, Position, blendDistance);
                switch (interpolation)
                {
                    case InterpolationType.Linear:
                        transform.position = Vector3.Lerp(transform.position, targetPosition, Delta);
                        break;
                    case InterpolationType.Spherical:
                        transform.position = Vector3.Slerp(transform.position, targetPosition, Delta);
                        break;
                    default: transform.position = targetPosition; break;
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (loop == UpdateMethod.FixedUpdate && target)
            {
                velocity = (transform.position - previusPosition) / Time.fixedDeltaTime;
                previusPosition = transform.position;
                if (velocity.magnitude > velocityThreshold) { onFollow.Invoke(); }
                float Delta = deltaTime ? Time.fixedDeltaTime * smooth : smooth;
                distance = target.position - transform.position;
                Vector3 Direction = transform.TransformDirection(distance);
                Vector3 Position = target.position + Direction.normalized * -Mathf.Abs(minDistance);
                Position = (distance.magnitude < maxDistance) ? Position + offset : transform.position;

                targetPosition = Vector3.Lerp(target.position + offset, Position, blendDistance);
                switch (interpolation)
                {
                    case InterpolationType.Linear:
                        transform.position = Vector3.Lerp(transform.position, targetPosition, Delta);
                        break;
                    case InterpolationType.Spherical:
                        transform.position = Vector3.Slerp(transform.position, targetPosition, Delta);
                        break;
                    default: transform.position = targetPosition; break;
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if (loop == UpdateMethod.LateUpdate && target)
            {
                velocity = (transform.position - previusPosition) / Time.deltaTime;
                previusPosition = transform.position;
                if (velocity.magnitude > velocityThreshold) { onFollow.Invoke(); }
                float Delta = deltaTime ? Time.deltaTime * smooth : smooth;
                distance = target.position - transform.position;
                Vector3 Direction = transform.TransformDirection(distance);
                Vector3 Position = target.position + Direction.normalized * -Mathf.Abs(minDistance);
                Position = (distance.magnitude < maxDistance) ? Position + offset : transform.position;

                targetPosition = Vector3.Lerp(target.position + offset, Position, blendDistance);
                switch (interpolation)
                {
                    case InterpolationType.Linear:
                        transform.position = Vector3.Lerp(transform.position, targetPosition, Delta);
                        break;
                    case InterpolationType.Spherical:
                        transform.position = Vector3.Slerp(transform.position, targetPosition, Delta);
                        break;
                    default: transform.position = targetPosition; break;
                }
            }
        }

        //Gizmos
        private void OnDrawGizmos()
        {
            if (enableDebug && target)
            {
                Gizmos.color = (distance.magnitude > maxDistance && blendDistance > 0) 
                    ? Color.red : Color.green;
                Gizmos.DrawLine(transform.position, target.position);
                Gizmos.DrawWireSphere(target.position, 0.1f);
            }
        }
    }

}
