using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TinkerPawn : Pawn
{
    [SerializeField] private NailGun m_nailGunReference;
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
