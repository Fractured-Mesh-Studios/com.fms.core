using UnityEngine;


namespace CoreEngine.DI
{
    public class Installer : MonoBehaviour
    {
        [Tooltip("Arrastra aquí tu MonoBehaviourInjector si está en otro GameObject. Si no, buscará uno en la escena.")]
        public Injector customInjector;

        void Awake()
        {
            if (customInjector == null)
            {
                customInjector = FindObjectOfType<Injector>();
                if (customInjector == null)
                {
                    Debug.LogError("MiniInject: MonoBehaviourInjector no encontrado en la escena. Asegúrate de tener uno.");
                    return;
                }
            }

            // Encuentra todos los instaladores en la escena
            var installers = FindObjectsByType<DependencyInstaller>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            // Ejecuta los bindings de cada instalador
            foreach (var installer in installers)
            {
                installer.InstallBindings(customInjector.Container);
            }

            // Luego de registrar todo, realiza la inyección
            customInjector.InjectAllMonoBehaviours();
        }
    }
}