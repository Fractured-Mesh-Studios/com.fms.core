// GameInstallers/MyGameInstaller.cs
using UnityEngine;

namespace CoreEngine.DI.Demo
{
    public class MyGameInstaller : DependencyInstaller
    {
        // Puedes arrastrar tus MonoBehaviours que ser�n Singletons o que ya existen en escena
        public MyGameService myGameServiceInstance; // Arrastra tu GameObject con MyGameService aqu�

        public override void InstallBindings(DependencyContainer container)
        {
            if (myGameServiceInstance != null)
            {
                // Registrar una instancia existente de un MonoBehaviour como Singleton
                container.RegisterMonoSingleton<IMyGameService, MyGameService>(myGameServiceInstance);
            }
            else
            {
                Debug.LogWarning("MiniInject: myGameServiceInstance no asignado en MyGameInstaller.");
                // Si no lo asignas, podr�as buscarlo o crearlo, o usar un singleton no-MonoBehaviour.
                // Ejemplo de Singleton no-MonoBehaviour:
                // container.RegisterSingleton<IMyOtherService, MyOtherServiceImplementation>();
            }
        }
    }
}