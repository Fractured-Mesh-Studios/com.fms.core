using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    public PersistanceData Data = null;
    public string FileName = "";

    private FileDataHandler FileDataHandler;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void New() 
    {
        Data = new PersistanceData();
        FileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName);
    }

    public void Load()
    { 
        if(Data == null)
        {
            New();
            return;
        }

        Data = FileDataHandler.Load();

        foreach(IPersistanceObject Obj in FindAllPersistenceObjects()){
            Obj.Load(Data);
        }

    }
    
    public void Save()
    {
        foreach (IPersistanceObject Obj in FindAllPersistenceObjects())
        {
            Obj.Save(ref Data);
        }

        FileDataHandler.Save(Data);
    }

    private List<IPersistanceObject> FindAllPersistenceObjects()
    {
        IEnumerable<IPersistanceObject> Objects;
        Objects = FindObjectsOfType<MonoBehaviour>().OfType<IPersistanceObject>();
        return new List<IPersistanceObject>(Objects);
    }
}
