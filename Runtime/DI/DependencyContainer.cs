using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CoreEngine.DI
{
    public class DependencyContainer
    {
        // Almacena los servicios por tipo.
        // Usamos 'object' porque no sabemos el tipo exacto en tiempo de compilaci�n.
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        // Almacena las funciones para crear instancias (Factories)
        private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Registra una instancia existente de un servicio.
        /// </summary>
        public void Register<TInterface, TImplementation>(TImplementation instance)
            where TImplementation : TInterface
        {
            var interfaceType = typeof(TInterface);
            if (_services.ContainsKey(interfaceType))
            {
                Debug.LogWarning($"MiniInject: Servicio '{interfaceType.Name}' ya registrado. Sobrescribiendo.");
            }
            _services[interfaceType] = instance;
        }

        /// <summary>
        /// Registra una factor�a para crear un Singleton.
        /// La instancia se crear� la primera vez que se solicite.
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new() // Restricci�n: debe ser una clase con constructor sin par�metros
        {
            var interfaceType = typeof(TInterface);
            if (_factories.ContainsKey(interfaceType))
            {
                Debug.LogWarning($"MiniInject: Factor�a Singleton para '{interfaceType.Name}' ya registrada. Sobrescribiendo.");
            }
            _factories[interfaceType] = () =>
            {
                // Si a�n no tenemos la instancia, la creamos y la guardamos
                if (!_services.ContainsKey(interfaceType))
                {
                    _services[interfaceType] = new TImplementation();
                }
                return _services[interfaceType];
            };
        }

        /// <summary>
        /// Registra un Singleton que es un MonoBehaviour (debe existir en la escena o ser creado).
        /// </summary>
        public void RegisterMonoSingleton<TInterface, TImplementation>(TImplementation instance)
            where TImplementation : MonoBehaviour, TInterface
        {
            var interfaceType = typeof(TInterface);
            if (_services.ContainsKey(interfaceType))
            {
                Debug.LogWarning($"MiniInject: Mono Singleton '{interfaceType.Name}' ya registrado. Sobrescribiendo.");
            }
            _services[interfaceType] = instance;
        }


        /// <summary>
        /// Resuelve y devuelve una instancia del servicio solicitado.
        /// </summary>
        public object Resolve(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
            {
                return _services[serviceType];
            }

            if (_factories.ContainsKey(serviceType))
            {
                return _factories[serviceType].Invoke(); // Crea y devuelve el Singleton
            }

            Debug.LogError($"MiniInject: Servicio '{serviceType.Name}' no resuelto. �Est� registrado?");
            return null;
        }
    }
}
