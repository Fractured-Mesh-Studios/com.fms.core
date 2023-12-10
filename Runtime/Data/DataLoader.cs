using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Data
{
    public class DataLoader
    {
        private const string m_key = "ZzP5rMHiMkWzGzh8fHP9JQ==";
        private static FileDataHandler g_file;
        private static bool g_encryption = false;
        private static Dictionary<string, object> g_data = new Dictionary<string, object>();

        #region PUBLIC

        public static void Initialize(string filename)
        {
            string path = Application.persistentDataPath;
            string name = $"{filename}.data";

            g_file = new FileDataHandler(path, name, m_key);
        }

        public static void Save<T>(string key, T data)
        {
            if (g_file != null)
            {
                if (!g_data.ContainsKey(key))
                {
                    g_data.Add(key, data);
                }
                else
                {
                    g_data[key] = data;
                }

                g_file.Save(g_data, g_encryption);
            }
            else
            {
                Debug.LogError("The static loading and saving system needs to be initialized before it can be used in the project.");
            }

        }

        public static T Load<T>(string key)
        {
            if (g_file != null)
            {
                g_data = g_file.Load<Dictionary<string, object>>(g_encryption);

                var data = g_data.Where(x => x.Key == key);

                return (T)data.FirstOrDefault().Value;
            }
            else
            {
                Debug.LogError("The static loading and saving system needs to be initialized before it can be used in the project.");
                return default;
            }
        }

        public static void LoadInto<T>(string key, ref T data)
        {
            data = Load<T>(key);
        }

        public static KeyValuePair<string, object>[] LoadAll()
        {
            if (g_file != null)
            {
                g_data = g_file.Load<Dictionary<string, object>>(g_encryption);

                List<KeyValuePair<string, object>> values = new List<KeyValuePair<string, object>>();
                foreach (var item in g_data)
                {
                    values.Add(item);
                }

                return values.ToArray();
            }
            else
            {
                Debug.LogError("The static loading and saving system needs to be initialized before it can be used in the project.");
                return null;
            }
        }

        #endregion

        #region KEYHANDLING

        public static void RemoveKey(string key)
        {
            g_data.Remove(key);
            g_file.Save(g_data, g_encryption);
        }

        public static bool ContainsKey(string key)
        {
            g_data = g_file.Load<Dictionary<string, object>>(g_encryption);
            return g_data.ContainsKey(key);
        }

        #endregion
    }
}
