using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    [System.Serializable]
    public class SRigidbody
    {
        public float mass;
        public float drag;
        public float angularDrag;
        public bool useGravity;
        public bool isKinematic;
        public bool automaticCenterOfMass;
        public Vector3 centerOfMass;
        public Vector3 ínertiaTensor;
        public Quaternion inertiaTensorRotation;
        public RigidbodyInterpolation interpolation;
        public CollisionDetectionMode collisionDetectionMode;
        public float maxLinearVelocity;
        public float maxAngularVelocity;

        public SRigidbody() { 
        
        }

        public SRigidbody(
            float mass, 
            float drag, 
            float angularDrag, 
            bool useGravity, 
            bool isKinematic, 
            bool automaticCenterOfMass, 
            Vector3 centerOfMass, 
            Vector3 ínertiaTensor, 
            Quaternion inertiaTensorRotation, 
            RigidbodyInterpolation interpolation, 
            CollisionDetectionMode collisionDetectionMode, 
            float maxLinearVelocity, 
            float maxAngularVelocity
        ) 
        {
            this.mass = mass;
            this.drag = drag;
            this.angularDrag = angularDrag;
            this.useGravity = useGravity;
            this.isKinematic = isKinematic;
            this.automaticCenterOfMass = automaticCenterOfMass;
            this.centerOfMass = centerOfMass;
            this.ínertiaTensor = ínertiaTensor;
            this.inertiaTensorRotation = inertiaTensorRotation;
            this.interpolation = interpolation;
            this.collisionDetectionMode = collisionDetectionMode;
            this.maxLinearVelocity = maxLinearVelocity;
            this.maxAngularVelocity = maxAngularVelocity;
        }

        public SRigidbody(Rigidbody rigidbody)
        {
            if (rigidbody)
            {
                this.mass = rigidbody.mass;
                this.drag = rigidbody.drag;
                this.angularDrag = rigidbody.angularDrag;
                this.useGravity = rigidbody.useGravity;
                this.isKinematic = rigidbody.isKinematic;
                this.automaticCenterOfMass = rigidbody.automaticCenterOfMass;
                this.centerOfMass = rigidbody.centerOfMass;
                this.ínertiaTensor = rigidbody.inertiaTensor;
                this.inertiaTensorRotation = rigidbody.inertiaTensorRotation;
                this.interpolation = rigidbody.interpolation;
                this.collisionDetectionMode = rigidbody.collisionDetectionMode;
                this.maxLinearVelocity = rigidbody.maxLinearVelocity;
                this.maxAngularVelocity = rigidbody.maxAngularVelocity;
            }

        }
    }
}
