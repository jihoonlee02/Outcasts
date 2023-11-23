using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AshePawn : Pawn
{
    [Header("Ashe Specific")]
    [SerializeField] private Tool m_gauntletReference;
    [SerializeField] private Collider2D m_liftingRegion;
    [SerializeField] private Collider2D m_heldObjectCollider;
    private bool m_isLifting = false;
    private bool m_isPunching = false;
    private bool m_isDropping = false;
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
    public bool IsDropping
    {
        get { return m_isDropping; }
        set { m_isDropping = value; }
    }
    public Collider2D LifitingRegion => m_liftingRegion;
    public Collider2D HeldObjectCollider => m_heldObjectCollider;
    protected void Start()
    {
        base.Start();
        CurrentState = m_states.AsheDefaultState();
        // So that Ashe can Jump and the held Object collider collides with external objects
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), HeldObjectCollider, true);
    }
    public override void PrimaryAction(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        m_gauntletReference.UsePrimaryAction();
    }

    public override void SecondaryAction(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        m_gauntletReference.UseSecondaryAction();
    }
    public void DisableLiftingRegion()
    {
        m_liftingRegion.enabled = false;
    }
    public void EnableLiftingRegion()
    {
        m_liftingRegion.enabled = true;
    }
}
