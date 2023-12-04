using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistanceObject
{
    public void Load(PersistanceData data);

    public void Save(ref PersistanceData data);
}
