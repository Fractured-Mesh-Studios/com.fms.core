
using System.IO;
using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string m_path;
    private string m_filename;

    public FileDataHandler(string path, string name)
    {
        m_path = path;
        m_filename = name;
    }

    public T Load<T>()
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
                using (FileStream FileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader Reader = new StreamReader(FileStream))
                    {
                        dataToLoad = Reader.ReadToEnd();
                    }
                }

                loadedData = JsonConvert.DeserializeObject<T>(dataToLoad, settings);

            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        return loadedData;
    }

    /*public T[] LoadArray<T>() 
    {
        string fullPath = Path.Combine(m_path, m_filename);

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Formatting = Formatting.Indented;
        settings.NullValueHandling = NullValueHandling.Include;

        T[] loadedData = default;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = String.Empty;
                using (FileStream FileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader Reader = new StreamReader(FileStream))
                    {
                        dataToLoad = Reader.ReadToEnd();
                    }
                }

                loadedData = JsonConvert.DeserializeObject<T[]>(dataToLoad, settings);

            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        return loadedData;
    }*/

    public void Save<T>(T data) 
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
                using(StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(dataToStore);  
                }
            }

        } catch(Exception e) {
            Debug.LogException(e);
        }
    }

    /*public void SaveArray<T>(T[] data)
    {
        string fullPath = Path.Combine(m_path, m_filename);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }*/
}
