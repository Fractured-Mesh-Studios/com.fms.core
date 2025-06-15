using System;
using System.Reflection;
using UnityEngine;
using System.Linq; // Para el LINQ en FindObjectsOfType

namespace CoreEngine.DI
{
    public class Injector : MonoBehaviour
    {
        public DependencyContainer Container { get; private set; }

        // Puedes configurar esto en el Inspector si prefieres
        // public bool InjectOnAwake = true;

        void Awake()
        {
            Container = new DependencyContainer();
            // Aquí puedes registrar tus servicios iniciales
            // Por ejemplo:
            // Container.RegisterSingleton<IMyService, MyServiceImplementation>();

            // Luego, inyecta todos los MonoBehaviour's activos en la escena.
            // Esto se hace en Awake para que las dependencias estén listas para Start().
            InjectAllMonoBehaviours();
        }

        public void InjectAllMonoBehaviours()
        {
            // Nota: FindObjectsByType es más moderno que FindObjectsOfType
            // Asegúrate de que este MonoBehaviourInjector esté en un GameObject activo en la escena
            // para que pueda escanear otros MonoBehaviour.
            var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            foreach (var monoBehaviour in monoBehaviours)
            {
                // No intentes inyectar en el propio inyector
                if (monoBehaviour == this) continue;

                Inject(monoBehaviour);
            }
            Debug.Log($"MiniInject: Inyección completada para {monoBehaviours.Length} MonoBehaviours.");
        }

        /// <summary>
        /// Realiza la inyección de dependencias en un MonoBehaviour específico.
        /// </summary>
        public void Inject(MonoBehaviour target)
        {
            if (target == null) return;

            var type = target.GetType();

            // 1. Inyección de Campos (Fields)
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (field.IsDefined(typeof(InjectAttribute), true))
                {
                    var dependency = Container.Resolve(field.FieldType);
                    if (dependency != null)
                    {
                        field.SetValue(target, dependency);
                        // Debug.Log($"MiniInject: Inyectado campo '{field.Name}' en '{target.name}'.");
                    }
                }
            }

            // 2. Inyección de Métodos (Methods)
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(InjectAttribute), true))
                {
                    var parameters = method.GetParameters();
                    var resolvedDependencies = new object[parameters.Length];

                    bool allDependenciesResolved = true;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramType = parameters[i].ParameterType;
                        var dependency = Container.Resolve(paramType);
                        if (dependency == null)
                        {
                            Debug.LogError($"MiniInject: No se pudo resolver la dependencia '{paramType.Name}' para el método '{method.Name}' en '{target.name}'.");
                            allDependenciesResolved = false;
                            break;
                        }
                        resolvedDependencies[i] = dependency;
                    }

                    if (allDependenciesResolved)
                    {
                        method.Invoke(target, resolvedDependencies);
                        // Debug.Log($"MiniInject: Inyectado método '{method.Name}' en '{target.name}'.");
                    }
                }
            }
        }
    }
}
