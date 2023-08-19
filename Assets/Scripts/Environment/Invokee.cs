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
    [SerializeField] protected bool ActivateOnly;
    [SerializeField] protected bool ActivateOnce;
    [SerializeField] protected bool ActivateUpdate;

    #region Technical
    private float currentDelayTime = 0f;
    #endregion

    protected void Awake()
    {
        EventManager.GetEventManager.Activated += ReactOnActivate;

        if (ActivateOnly) return;

        EventManager.GetEventManager.Deactivated += ReactOnDeactivate;
    }

    private void Update()
    {
        // Unused Polling version
        //if (ActivateUpdate && Time.time >= currentDelayTime)
        //{
        //    OnActivate();
        //    if (ActivateOnce) EventManager.GetEventManager.Activated -= ReactOnActivate;
        //    ActivateUpdate = false;
        //}
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
            //ActivateUpdate = true;
            //currentDelayTime = delay + Time.time;
            StartCoroutine(DelayActivate());        
        }    
    }

    private void ReactOnDeactivate(int other_id)
    {
        if (other_id == id)
        {
            OnDeactivate();
        }

        if (ActivateOnce) EventManager.GetEventManager.Deactivated -= ReactOnDeactivate;
    }

    protected abstract void OnActivate();

    protected abstract void OnDeactivate();

    private IEnumerator DelayActivate()
    {
        yield return new WaitForSeconds(delay);
        OnActivate();
        if (ActivateOnce) EventManager.GetEventManager.Activated -= ReactOnActivate;
    }
}
