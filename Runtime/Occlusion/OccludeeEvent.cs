using CoreEngine.Occlusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Occludee))]
public class OccludeeEvent : MonoBehaviour
{
    [Header("Event")]
    public UnityEvent onEnable = new UnityEvent();
    public UnityEvent onDisable = new UnityEvent();

    private Occludee m_occludee;

    #region UNITY
    public void Awake()
    {
        m_occludee = GetComponent<Occludee>();
    }

    private void OnEnable()
    {
        m_occludee.onEnable += onEnable.Invoke;
        m_occludee.onDisable += onDisable.Invoke;
    }

    private void OnDisable()
    {
        m_occludee.onEnable -= onEnable.Invoke;
        m_occludee.onDisable -= onDisable.Invoke;
    }
    #endregion
}
