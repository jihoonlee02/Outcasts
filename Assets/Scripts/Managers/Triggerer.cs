using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Triggerer : MonoBehaviour
{
    [SerializeField] private UnityEvent[] m_events;

    public UnityEvent[] Events => m_events;

    public void Trigger()
    {
        foreach (var e in m_events)
        {
            e.Invoke();
        }
    }
}