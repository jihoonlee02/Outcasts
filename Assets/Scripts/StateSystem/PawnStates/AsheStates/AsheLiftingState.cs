using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AsheLiftingState : State
{
    public AsheLiftingState(Pawn context, PawnStateFactory factory) : base(context, factory)
    {
        m_isRootState = true;
        m_animationName = "Lifting";
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Switched to AsheLifting");
    }
    public override void ExitState() { }
    public override void InitializeSubState()
    {
        if (m_context.IsJumping)
        {
            SetSubState(m_factory.Jumping());
        }
        else if (!m_context.IsGrounded)
        {
            SetSubState(m_factory.Falling());
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
        if (!((AshePawn)m_context).IsLifting)
        {
            SwitchState(m_factory.AsheDefaultState());
        }
        else if (((AshePawn)m_context).IsPunching)
        {
            SwitchState(m_factory.AshePunchingState());
        }
    }
}
