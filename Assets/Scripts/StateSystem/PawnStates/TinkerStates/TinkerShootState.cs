using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinkerShootState : State
{
    public TinkerShootState(Pawn context, PawnStateFactory factory) : base(context, factory)
    {
        m_isRootState = true;
        m_animationName = "";
        InitializeSubState();
    }

    public override void EnterState()
    {
        m_context.AudioSource.pitch = 1;
        m_context.AudioSource.clip = m_context.Data.ScratchPadSounds[0];
        m_context.AudioSource.Play();
    }
    public override void UpdateState()
    {

    }
    public override void ExitState()
    {

    }
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
        if (!((TinkerPawn)m_context).IsShooting)
        {
            SwitchState(m_factory.TinkerDefaultState());
        }
    }
}
