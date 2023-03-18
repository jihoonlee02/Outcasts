using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TinkerPawn : Pawn
{
    [SerializeField] private NailGun m_nailGunReference;
    private bool m_isShooting;
    private bool m_isHeld;
    public bool IsShooting => m_isShooting;
    public bool IsHeld
    {
        get { return m_isHeld; }
        set { m_isHeld = value; }
    }
    protected void Start()
    {
        base.Start();
        CurrentState = m_states.TinkerDefaultState();
    }
    public override void PrimaryAction(InputAction.CallbackContext context)
    {
        m_nailGunReference.UsePrimaryAction(m_pc.PlayerInputVector);
    }

    public override void SecondaryAction(InputAction.CallbackContext context)
    {
        if (context.performed) m_nailGunReference.UseSecondaryAction();
        if (context.canceled) m_nailGunReference.UseSecondaryAction();
    }
}
