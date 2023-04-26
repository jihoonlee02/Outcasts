using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AshePawn : Pawn
{
    [SerializeField] private Tool m_gauntletReference;
    [SerializeField] private Collider2D m_liftingRegion; 
    private bool m_isLifting = false;
    private bool m_isPunching = false;
    public GameObject HeldObject;
    public bool IsLifting
    {
        get { return m_isLifting;}
        set { m_isLifting = value;}
    }
    public bool IsPunching
    {
        get { return m_isPunching;}
        set { m_isPunching = value;}
    }
    public Collider2D LifitingRegion => m_liftingRegion;
    protected void Start()
    {
        base.Start();
        CurrentState = m_states.AsheDefaultState();
    }
    public override void PrimaryAction(InputAction.CallbackContext context)
    {
        m_gauntletReference.UsePrimaryAction();
    }

    public override void SecondaryAction(InputAction.CallbackContext context)
    {
        //m_gauntletReference.UseSecondaryAction();
    }
}
