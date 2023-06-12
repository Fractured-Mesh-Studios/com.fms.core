
using System.IO;
using System;
using System.Linq;
using UnityEngine;

public class FileDataHandler 
{
    private string PathData;
    private string FileName;

    public FileDataHandler(string path,string name)
    {
        PathData = path;
        FileName = name;
    }

    public PersistanceData Load()
    {
        string FullPath = Path.Combine(PathData, FileName);
        PersistanceData LoadedData = null;
        if (File.Exists(FullPath))
        {
            try
            {
                string DataToLoad = String.Empty;
                using (FileStream FileStream = new FileStream(FullPath, FileMode.Open))
                {
                    using (StreamReader Reader = new StreamReader(FileStream))
                    {
                        DataToLoad = Reader.ReadToEnd();
                    }
                }

                LoadedData = JsonUtility.FromJson<PersistanceData>(DataToLoad);

            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        return LoadedData;
    }

    public void Save(PersistanceData Data) 
    {
        string FullPath = Path.Combine(PathData, FileName);
        try 
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            
            string DataToStore = JsonUtility.ToJson(Data, true);

            using (FileStream fs = new FileStream(FullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(DataToStore);  
                }
            }

        } catch(Exception e) {
            Debug.LogException(e);
        }
    }
}
