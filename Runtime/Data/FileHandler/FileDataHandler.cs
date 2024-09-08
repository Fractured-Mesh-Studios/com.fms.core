using System;
using System.IO;
using UnityEngine;
using CoreEngine.Interfaces;
using CoreEngine.Data.Serializer;

namespace CoreEngine.Data
{
    public class FileDataHandler
    {
        private string m_path;
        private string m_filename;
        private string m_key;
        private ISerialization m_serializer;

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
            m_serializer = new JsonSerializer();
        }

        public FileDataHandler(string path, string name, string key, ISerialization serializer)
        {
            m_path = path;
            m_filename = name;
            m_key = key;
            m_serializer = serializer;
        }

        #region LOAD
        public T Load<T>(bool encrypted = false)
        {
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
                        
                            loadedData = m_serializer.Deserialize<T>(dataToLoad);
                        }
                    }
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
            else
            {
                throw new FileNotFoundException(null, m_filename);
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
        #endregion

        #region SAVE
        public void Save<T>(T data, bool encrypted = false) 
        {
            string fullPath = Path.Combine(m_path, m_filename);
            try 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
                string dataToStore = m_serializer.Serialize(data);

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
        #endregion

        private void WriteEncryptedData<T>(T data, FileStream stream)
        {
            string text = m_serializer.Serialize(data);

            if(m_key == string.Empty)
            {
                Debug.LogError("Encryption key is <null>");
                return;
            }

            AesOperation.EncryptString(m_key, text, stream);
        }

        private T ReadEncryptedData<T>(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            if (m_key == string.Empty)
            {
                Debug.LogError("Encryption key is <null>");
                return default;
            }

            string result = AesOperation.DecryptString(m_key, data);

            return m_serializer.Deserialize<T>(result);
        }

    }
}
