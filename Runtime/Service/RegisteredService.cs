using UnityEngine;

namespace CoreEngine
{
    public abstract class RegisteredService : MonoBehaviour, IServiceRegister
    {
        public abstract void Register();
        public abstract void Unregister();

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }
    }
}
