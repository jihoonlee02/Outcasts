using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Invoker : MonoBehaviour
{
    [Header("Invoker Details")]
    [SerializeField] private int id;

    protected void Activate()
    {
        EventManager.GetEventManager.Activated.Invoke(id);
    }

    protected void Deactivate()
    {
        EventManager.GetEventManager.Deactivated.Invoke(id);
    }
}