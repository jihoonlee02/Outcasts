using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PawnDefaultState : State
{
    public PawnDefaultState(Pawn context, PawnStateFactory factory) : base(context, factory) 
    { 
        InitializeSubState();
        m_isRootState = true;
    }
    public override void UpdateState()
    {

    }
    public override void CheckSwitchState()
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

    }
}
