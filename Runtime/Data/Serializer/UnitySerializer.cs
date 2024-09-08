using CoreEngine.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine.Data.Serializer
{
    public class UnitySerializer : ISerialization
    {
        public T Deserialize<T>(string data)
        {
            return JsonUtility.FromJson<T>(data);
        }

        public string Serialize(object data)
        {
            return JsonUtility.ToJson(data);
        }
    }
}
