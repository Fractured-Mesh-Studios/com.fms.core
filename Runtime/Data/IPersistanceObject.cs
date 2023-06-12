using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistanceObject
{
    public void Load(PersistanceData Data);

    public void Save(ref PersistanceData Data);
}
