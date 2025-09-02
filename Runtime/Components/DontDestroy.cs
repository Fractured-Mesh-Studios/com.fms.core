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

        private static List<string> tagsToIgnore = new List<string>();

        public static void Clear()
        {
            //DontDestroy[] contianer = FindObjectsOfType<DontDestroy>();
            //foreach (DontDestroy cont in contianer)
            //{
            //    tagsToIgnore.AddRange(cont.ignore);
            //}

            GameObject temp = new GameObject("Temp_DDOL");
            Object.DontDestroyOnLoad(temp);

            Scene dontDestroyOnLoad = temp.scene;

            GameObject[] rootObjects = dontDestroyOnLoad.GetRootGameObjects();

            foreach (GameObject obj in rootObjects)
            {
                //if(tagsToIgnore.Contains(obj.tag)) continue;
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
