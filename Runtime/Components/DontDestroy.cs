using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreEngine
{
    public class DontDestroy : MonoBehaviour
    {
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
                if (obj != temp)
                    Object.Destroy(obj);
            }

            Object.Destroy(temp);
        }
    }
}
