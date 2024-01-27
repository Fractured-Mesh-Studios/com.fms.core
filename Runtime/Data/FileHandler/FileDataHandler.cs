
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CoreEngine.Data
{
    public class FileDataHandler
    {
        private string m_path;
        private string m_filename;
        private string m_key;

        public FileDataHandler(string path, string name)
        {
            m_path = path;
            m_filename = name;
            m_key = string.Empty;
        }

        public FileDataHandler(string path, string name, string key)
        {
            m_path = path;
            m_filename = name;
            m_key = key;
        }

        public T Load<T>(bool encrypted = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            T loadedData = default;
            string fullPath = Path.Combine(m_path, m_filename);
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = String.Empty;
                    if (encrypted) 
                    {
                        loadedData = ReadEncryptedData<T>(fullPath);
                    } 
                    else
                    { 
                        using (FileStream FileStream = new FileStream(fullPath, FileMode.Open))
                        {
                            using (StreamReader Reader = new StreamReader(FileStream))
                            {
                                dataToLoad = Reader.ReadToEnd();
                            }
                        
                            loadedData = JsonConvert.DeserializeObject<T>(dataToLoad, settings);
                        }
                    }
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }

            return loadedData;
        }

        public string LoadRaw()
        {
            string loadedData = default;
            string fullPath = Path.Combine(m_path, m_filename);
            if (File.Exists(fullPath))
            {
                try
                {
                    using (FileStream FileStream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader Reader = new StreamReader(FileStream))
                        {
                            loadedData = Reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return loadedData;
        }

        public void Save<T>(T data, bool encrypted = false) 
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();   
            settings.Formatting = Formatting.Indented;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            string fullPath = Path.Combine(m_path, m_filename);
            try 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
                string dataToStore = JsonConvert.SerializeObject(data, settings);

                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    if(encrypted)
                    {
                        WriteEncryptedData(data, fs);
                    }
                    else
                    {
                        using(StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(dataToStore);  
                        }
                    }
                }

            } catch(Exception e) {
                Debug.LogException(e);
            }
        }

        public void SaveRaw(string data)
        {
            string fullPath = Path.Combine(m_path, m_filename);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(data);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void WriteEncryptedData<T>(T data, FileStream stream)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            string text = JsonConvert.SerializeObject(data, settings);

            if(m_key == string.Empty)
            {
                Debug.LogError("Encryption key is <null>");
                return;
            }

            AesOperation.EncryptString(m_key, text, stream);
        }

        private T ReadEncryptedData<T>(string path)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            
            byte[] data = File.ReadAllBytes(path);

            if (m_key == string.Empty)
            {
                Debug.LogError("Encryption key is <null>");
                return default;
            }

            string result = AesOperation.DecryptString(m_key, data);

            return JsonConvert.DeserializeObject<T>(result);
        }

    }
}
