using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInvoker : MonoBehaviour
{
    [Header("Random Invoker")]
    [SerializeField] private float m_minimumWait;
    [SerializeField] private float m_maximumWait;
    [SerializeField] private int[] events; 

    #region technical
    private float cooldown;
    #endregion
    private void Start()
    {
        cooldown = Time.time + Random.Range(m_minimumWait, m_maximumWait);
    }
    private void Update()
    {
        if (Time.time > cooldown)
        {
            cooldown = Time.time + Random.Range(m_minimumWait, m_maximumWait);
            if (events != null) EventManager.GetEventManager.Activated.Invoke(events[Random.Range(0, events.Length)]);
        }
    }
}
