using UnityEngine;

namespace CoreEngine
{
    public abstract class Singleton<Type> : MonoBehaviour where Type : Component
    {
        [Tooltip("If true, the singleton instance will persist across scene loads. If false, it will be destroyed when a new scene is loaded.")]
        public bool dontDestroyOnLoad = true;

        protected static Type s_instance;

        public static Type instance 
        {
            get 
            {
                if (s_instance == null)
                {
#if UNITY_2020_1_OR_NEWER
                    s_instance = FindAnyObjectByType<Type>(FindObjectsInactive.Include);
#else
                    s_instance = Object.FindObjectOfType<Type>(true);
#endif

                    if (s_instance == null)
                    {
                        var go = new GameObject(typeof(Type).Name + " (Singleton)");
                        s_instance = go.AddComponent<Type>();
                    }
                }

                return s_instance;
            } 
        }

        #region Unity
        protected virtual void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as Type;

                if (dontDestroyOnLoad)
                {
                    if (!TryGetComponent(out DontDestroy ddol))
                    {
                        gameObject.AddComponent<DontDestroy>();
                    }
                }
            }
            else
            {
                if (instance.gameObject == this.gameObject)
                    return;

                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (s_instance != this)
            {
                return;
            }

            s_instance = null;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (dontDestroyOnLoad)
            {
                if (!TryGetComponent(out DontDestroy ddol))
                {
                    gameObject.AddComponent<DontDestroy>();
                }
            }
        }
#endif
    }
}
