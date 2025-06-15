using UnityEngine;

namespace CoreEngine.DI
{
    public abstract class DependencyInstaller : MonoBehaviour
    {
        public abstract void InstallBindings(DependencyContainer container);
    }
}
