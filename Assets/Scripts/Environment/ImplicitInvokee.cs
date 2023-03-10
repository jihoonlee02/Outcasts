using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImplicitInvokee : Invokee
{
    [SerializeField] private UnityEvent activate;
    [SerializeField] private UnityEvent deactivate;
    protected override void OnActivate()
    {
        activate.Invoke();
    }

    protected override void OnDeactivate()
    {
        deactivate.Invoke();
    }
}
