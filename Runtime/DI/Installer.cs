using UnityEngine;


namespace CoreEngine.DI
{
    public class Installer : MonoBehaviour
    {
        [Tooltip("Arrastra aqu� tu MonoBehaviourInjector si est� en otro GameObject. Si no, buscar� uno en la escena.")]
        public Injector customInjector;

        void Awake()
        {
            if (customInjector == null)
            {
                customInjector = FindObjectOfType<Injector>();
                if (customInjector == null)
                {
                    Debug.LogError("MiniInject: MonoBehaviourInjector no encontrado en la escena. Aseg�rate de tener uno.");
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

            // Luego de registrar todo, realiza la inyecci�n
            customInjector.InjectAllMonoBehaviours();
        }
    }
}