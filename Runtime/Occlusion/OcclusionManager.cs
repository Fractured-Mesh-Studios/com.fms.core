using CoreEngine.Occlusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OcclusionManager : MonoBehaviour
{
    private IOccludee[] m_container;

    public void Detect()
    {
        m_container = FindObjectsByType<MonoBehaviour>(0).OfType<IOccludee>().ToArray();
    }
    
    public void Clear()
    {
        m_container = null;
    }

    void Update()
    {
        
    }
}
