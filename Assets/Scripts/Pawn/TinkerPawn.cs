using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinkerPawn : Pawn
{
    [SerializeField] private NailGun m_nailGunReference;
    public override void PrimaryAction()
    {
        m_nailGunReference.UsePrimaryAction(m_pc.PlayerInputVector);
    }

    public override void SecondaryAction()
    {
        m_nailGunReference.UseSecondaryAction();
    }
}
