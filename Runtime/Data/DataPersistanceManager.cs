using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    public PersistanceData data = null;
    public string filename = "";

    private FileDataHandler m_fileDataHandler;

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
        data = new PersistanceData();
        m_fileDataHandler = new FileDataHandler(Application.persistentDataPath, filename);
    }

    public void Load()
    { 
        if(data == null)
        {
            New();
            return;
        }

        data = m_fileDataHandler.Load<PersistanceData>();

        foreach(IPersistanceObject Obj in FindAllPersistenceObjects()){
            Obj.Load(data);
        }

    }
    
    public void Save()
    {
        foreach (IPersistanceObject Obj in FindAllPersistenceObjects())
        {
            Obj.Save(ref data);
        }

        m_fileDataHandler.Save(data);
    }

    private List<IPersistanceObject> FindAllPersistenceObjects()
    {
        IEnumerable<IPersistanceObject> Objects;
        Objects = FindObjectsOfType<MonoBehaviour>().OfType<IPersistanceObject>();
        return new List<IPersistanceObject>(Objects);
    }
}
