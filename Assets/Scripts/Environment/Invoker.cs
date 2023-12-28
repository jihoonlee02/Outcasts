using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Invoker : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private GameObject targetColor;

    [Header("Invoker Details")]
    [SerializeField] private int id;

    // Switch ID system to color Id system?
    [SerializeField, ColorUsage(true, true)] private Color colorId;

    protected void Activate()
    {
        EventManager.GetEventManager.Activated.Invoke(this.id);
    }

    protected void Deactivate()
    {
        EventManager.GetEventManager.Deactivated.Invoke(this.id);
    }

    protected void Activate(int id)
    {
        EventManager.GetEventManager.Activated.Invoke(id);
    }

    protected void Deactivate(int id)
    {
        EventManager.GetEventManager.Deactivated.Invoke(id);
    }

    protected void OnDrawGizmos()
    {
        if (id < 0) return;
        Handles.Label(Vector3.zero, "Invoker ID: " + id);
    }
}
