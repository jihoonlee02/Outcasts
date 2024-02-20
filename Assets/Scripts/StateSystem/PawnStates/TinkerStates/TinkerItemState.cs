using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinkerItemState : State
{
    public TinkerItemState(Pawn context, PawnStateFactory factory) : base(context, factory)
    {
        m_isRootState = true;
        m_animationName = "_holdingItem";
        InitializeSubState();
    }
    public override void EnterState()
    {
        ((TinkerPawn)m_context).DisableShoot();
    }
    public override void ExitState()
    {
        ((TinkerPawn)m_context).EnableShoot();
    }
    public override void InitializeSubState()
    {
        if (!m_context.IsGrounded)
        {
            SetSubState(m_factory.Falling());
        }
        else if (m_context.IsJumping)
        {
            SetSubState(m_factory.Jumping());
        }
        else if (m_context.IsMoving)
        {
            SetSubState(m_factory.Moving());
        }
        else
        {
            SetSubState(m_factory.Grounded());
        }

        m_subState.EnterState();
    }

    public override void CheckSwitchState()
    {
        if (!m_context.IsHoldingItem)
        {
            SwitchState(m_factory.TinkerDefaultState());
        }
    }
}
