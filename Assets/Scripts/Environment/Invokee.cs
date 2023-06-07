using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Invokee : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] protected GameObject targetColor;

    [Header("Inovkee Details")]
    [SerializeField] protected int id;
    [SerializeField] protected float delay = 0f;

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
            StartCoroutine(DelayActivate());        
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

    private IEnumerator DelayActivate()
    {
        yield return new WaitForSeconds(delay);
        OnActivate();
    }
}
