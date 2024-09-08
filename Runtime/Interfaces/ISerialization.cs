using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine.Interfaces
{
    public interface ISerialization
    {
        T Deserialize<T>(string data);

        string Serialize(object data);
    }
}
