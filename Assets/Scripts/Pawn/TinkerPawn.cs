using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TinkerPawn : Pawn
{
    [SerializeField] private NailGun m_nailGunReference;
    private bool m_isShooting;
    public bool IsShooting => m_isShooting;
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
