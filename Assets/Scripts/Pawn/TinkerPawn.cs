using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinkerPawn : Pawn
{
    [SerializeField] private Tool m_nailGunReference;
    public override void PrimaryAction()
    {
        m_nailGunReference.UsePrimaryAction();
    }

    public override void SecondaryAction()
    {
        m_nailGunReference.UseSecondaryAction();
    }
}
