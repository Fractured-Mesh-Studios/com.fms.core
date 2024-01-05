using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;

namespace CoreEngine.Data
{
    public class DataLoader
    {
        public static bool autoSave = false;

        private class SerializedData : Dictionary<string, object> { }
        private const string m_key = "ZzP5rMHiMkWzGzh8fHP9JQ==";
        private static SerializedData g_data = new SerializedData();
        private static FileDataHandler g_file;
        private static bool g_encryption = false;
        private static string g_path;

        public static void Initialize(string filename)
        {
            string path = Application.persistentDataPath;
            string name = $"{filename}.data";

            g_path = path;
            g_file = new FileDataHandler(path, name, m_key);
            g_data.Clear();
        }

        #region LOAD
        public static T Load<T>(string key)
        {
            if (g_file != null)
            {
                g_data = g_file.Load<SerializedData>(g_encryption);

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
        #endregion

        #region SAVE
        public static void Save()
        {
            g_file.Save(g_data, g_encryption);
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

        public static void SaveInto<T>(string key, T data, string path)
        {
            Initialize(path);
            Save(key, data);
        } 
        #endregion

        #region KEYHANDLING
        public static void AddKey<T>(string key, T value)
        {
            if(!g_data.ContainsKey(key))
                g_data.Add(key, value);
            else
                g_data[key] = value;

            AutoSave();
        }

        public static void RemoveKey(string key)
        {
            g_data.Remove(key);

            AutoSave();
        }

        public static bool ContainsKey(string key)
        {
            return g_data.ContainsKey(key);
        }

        public static string[] GetKeys()
        {
            List<string> keys = new List<string>();
            g_data = g_file.Load<SerializedData>();
            foreach(var item in g_data)
            {
                keys.Add(item.Key);
            }
            return keys.ToArray();
        }

        public static object[] GetValues()
        {
            List<object> values = new List<object>();
            g_data = g_file.Load<SerializedData>();
            foreach (var item in g_data)
            {
                values.Add(item.Value);
            }
            return values.ToArray();
        }

        public static void Clear()
        {
            g_data.Clear();

            AutoSave();
        }
        #endregion

        #region FILES
        public static void RemoveFile(string path, bool absolute = false)
        {
            string fullpath = Application.persistentDataPath + '/';

            fullpath = absolute ? path : fullpath + path;

            if (File.Exists(fullpath))
                File.Delete(fullpath);
            else
                Debug.LogError($"File <{path}> not exist in directory");
        }

        public static string[] GetFiles(string pattern)
        {
            List<string> fileNames = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(g_path);

            FileInfo[] files = directory.GetFiles(pattern);
            foreach (FileInfo file in files)
            {
                fileNames.Add(file.Name);
            }
            return fileNames.ToArray();
        }

        public static string[] GetFiles(string path, string pattern)
        {
            List<string> fileNames = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(path);

            FileInfo[] files = directory.GetFiles(pattern);
            foreach (FileInfo file in files)
            {
                fileNames.Add(file.Name);
            }
            return fileNames.ToArray();
        }
        #endregion

        #region PRIVATE
        private static void AutoSave()
        {
            if (autoSave)
            {
                g_file.Save(g_data, g_encryption);
            }
        }
        #endregion
    }
}
