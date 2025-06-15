using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
