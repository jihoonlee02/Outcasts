using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Invoker : MonoBehaviour
{
    //[Header("Resources")]
    //[SerializeField] private GameObject targetColor;

    [Header("Invoker Details")]
    [SerializeField] private int id = -1;

    //// Switch ID system to color Id system?
    //[SerializeField, ColorUsage(true, true)] private Color colorId;

    protected void Activate()
    {
        // Cannot Invoke negative invokees
        if (this.id < 0) return;
        EventManager.GetEventManager.Activated.Invoke(this.id);
    }

    protected void Deactivate()
    {
        // Cannot Invoke negative invokees
        if (this.id < 0) return;
        EventManager.GetEventManager.Deactivated.Invoke(this.id);
    }

    protected void Activate(int id)
    {
        // Cannot Invoke negative invokees
        if (id < 0) return;
        EventManager.GetEventManager.Activated.Invoke(id);
    }

    protected void Deactivate(int id)
    {
        // Cannot Invoke negative invokees
        if (id < 0) return;
        EventManager.GetEventManager.Deactivated.Invoke(id);
    }
    #if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(transform.position, this.id < 0 ? "Static" : "Invoker ID: " + this.id);
    }
    #endif
}
