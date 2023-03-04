using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheMovingState : MovingState
{
    public AsheMovingState(AshePawn context, PawnStateFactory factory) : base(context, factory) { }

    public override void CheckSwitchState()
    {
        if (m_context.IsGrounded && !m_context.IsMoving)
        {
            SwitchState(m_factory.Grounded());
        }
        else if (m_context.IsJumping)
        {
            SwitchState(m_factory.Jumping());
        }
        else if (!m_context.IsGrounded)
        {
            SwitchState(m_factory.Falling());
        } 
        //else if (((AshePawn)m_context).IsLifting) 
        //{ 
        //    SwitchState(m_factory.LiftingMoving());
        //}
    }
}
