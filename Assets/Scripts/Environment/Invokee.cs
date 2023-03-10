using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Invokee : MonoBehaviour
{
    [Header("Inovkee Details")]
    [SerializeField] protected int id;

    protected void Awake()
    {
        EventManager.GetEventManager.Activated += ReactOnActivate;
        EventManager.GetEventManager.Deactivated += ReactOnDeactivate;
    }

    private void OnDestroy()
    {
        EventManager.GetEventManager.Activated -= ReactOnActivate;
        EventManager.GetEventManager.Deactivated -= ReactOnDeactivate;
    }

    private void ReactOnActivate(int other_id)
    {
        if (other_id == id) 
        {
            OnActivate();
        }
    }

    private void ReactOnDeactivate(int other_id)
    {
        if (other_id == id)
        {
            OnDeactivate();
        }
    }

    protected abstract void OnActivate();

    protected abstract void OnDeactivate();
}
