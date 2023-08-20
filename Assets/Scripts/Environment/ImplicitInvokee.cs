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

    public void AlertAshe(float time)
    {
        GameManager.Instance.Ashe.Alert(time);
    }
    public void AlertTinker(float time)
    {
        GameManager.Instance.Tinker.Alert(time);
    }
    public void SetAsheMovement(bool move)
    {
        GameManager.Instance.Ashe.CanMove = move;
    }
    public void SetTinkerMovement(bool move)
    {
        GameManager.Instance.Tinker.CanMove = move;
    }
}
