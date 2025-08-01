using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace CoreEngine
{
    /// <summary>
    /// Simple service locator for <see cref="IBaseService"/> instances.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// Currently registered services.
        /// </summary>
        private static readonly Dictionary<Type, IBaseService> s_services = new Dictionary<Type, IBaseService>();

        /// <summary>
        /// Gets the service instance of the given type.
        /// </summary>
        /// <typeparam name="Type">The type of the service to lookup.</typeparam>
        /// <returns>The service instance.</returns>
        public static Type GetService<Type>() where Type : IBaseService
        {
            string key = typeof(Type).Name;
            if (!s_services.ContainsKey(typeof(Type)))
            {
                Debug.LogAssertion($"{key} not registered with {typeof(ServiceLocator).Name}");
                throw new InvalidOperationException();
            }

            return (Type)s_services[typeof(Type)];
        }

        /// <summary>
        /// Search in current scene and gets the service instance of the given type. (if found)
        /// </summary>
        /// <typeparam name="Type">The type for the serach</typeparam>
        /// <param name="inactive">Determines if can search inactive objects</param>
        /// <returns>Instance of the service</returns>
        public static Type GetSlowService<Type>(FindObjectsInactive find = FindObjectsInactive.Include) where Type : Object, IBaseService
        {
            Assert.IsNotNull(s_services, "Someone has requested a service prior to the locator's intialization.");

            if (!s_services.ContainsKey(typeof(Type)))
            {
                Type var = GameObject.FindAnyObjectByType<Type>(find);

                if (ReferenceEquals(var, null))
                {
                    Assert.IsNull(var, $"{typeof(Type).Name} service is not found.");
                }

                s_services.Add(typeof(Type), var);
            }

            Assert.IsTrue(s_services.ContainsKey(typeof(Type)), "Could not find service: " + typeof(Type));
            var service = (Type)s_services[typeof(Type)];

            Assert.IsNotNull(service, typeof(Type).Name + " could not be found.");
            return service;
        }

        /// <summary>
        /// Registers the service with the current service locator.
        /// </summary>
        /// <typeparam name="Type">Service type.</typeparam>
        /// <param name="service">Service instance.</param>
        public static void Register<Type>(Type service) where Type : IBaseService
        {
            string key = typeof(Type).Name;
            if (s_services.ContainsKey(typeof(Type)))
            {
                string name = typeof(ServiceLocator).Name;
                Debug.LogAssertion($"Attempted to register service of type {key} which is already registered with the {name}.");
                return;
            }

            //Add.
            s_services.Add(typeof(Type), service);
        }

        /// <summary>
        /// Unregisters the service from the current service locator.
        /// </summary>
        /// <typeparam name="Type">Service type.</typeparam>
        public static void Unregister<Type>() where Type : IBaseService
        {
            string key = typeof(Type).Name;
            if (!s_services.ContainsKey(typeof(Type)))
            {
                Debug.LogAssertion($"Attempted to unregister service of type {key} which is not registered with the {typeof(ServiceLocator).Name}.");
                return;
            }

            s_services.Remove(typeof(Type));
        }

        /// <summary>
        /// Unregisters the service from the current service locator (by reference).
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="service"></param>
        public static void Unregister<Type>(Type service) where Type : IBaseService
        {
            string key = typeof(Type).Name;
            if (!s_services.ContainsKey(service.GetType()))
            {
                Debug.LogAssertion($"Attempted to unregister service of type {key} which is not registered with the {typeof(ServiceLocator).Name}.");
                return;
            }

            s_services.Remove(typeof(Type));
        }

        /// <summary>
        /// Clear all instances found of all services from the current service locator.
        /// </summary>
        public static void Clear()
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log($"Cleared {s_services.Count} Services");
            }
            s_services.Clear();
        }

        /// <summary>
        /// Show all registered services type names
        /// </summary>
        public static void ShowServices()
        {
            if (Debug.isDebugBuild)
            {
                foreach (var service in s_services)
                {
                    Debug.Log($"Service: '{service.Key.Name}' Located".Color(Color.green));
                }
            }
        }

        /// <summary>
        /// Get all the active and registered services.
        /// </summary>
        /// <returns></returns>
        public static IBaseService[] GetServices()
        {
            return s_services.Values.ToArray();
        }
    }
}
