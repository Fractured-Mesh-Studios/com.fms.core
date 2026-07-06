using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreEngine
{
    public class DontDestroy : MonoBehaviour
    {
        [SerializeField]
        public bool persistent;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void Clear()
        {
            GameObject temp = new GameObject("Temp_DDOL");
            Object.DontDestroyOnLoad(temp);

            Scene dontDestroyOnLoad = temp.scene;

            GameObject[] rootObjects = dontDestroyOnLoad.GetRootGameObjects();

            foreach (GameObject obj in rootObjects)
            {
                if (obj.TryGetComponent(out DontDestroy dd))
                {
                    if (dd.persistent) continue;
                }
                
                if (obj != temp)
                    Object.Destroy(obj);
            }

            Object.Destroy(temp);
        }
    }
}
