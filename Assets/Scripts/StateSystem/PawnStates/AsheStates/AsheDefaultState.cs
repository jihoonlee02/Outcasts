using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheDefaultState : State
{
    public AsheDefaultState(Pawn context, PawnStateFactory factory) : base(context, factory) 
    {
        m_isRootState = true;
        m_animationName = "";
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Switched to AsheDefault");
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
        if (((AshePawn)m_context).IsPunching)
        {
            SwitchState(m_factory.AshePunchingState());
        }
        else if (((AshePawn)m_context).IsLifting)
        {
            SwitchState(m_factory.AsheLifitngState());
        }      
    }
}
