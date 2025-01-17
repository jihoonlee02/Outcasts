using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : State
{
    public FallingState(Pawn context, PawnStateFactory factory) : base(context, factory) 
    {
        m_animationName = "Falling";
    }
    public override void EnterState()
    {
        m_context.Animator.Play(m_animationName + m_superState.AnimationName);
    }
    public override void UpdateState()
    {
 
    }
    public override void CheckSwitchState()
    {
        if (m_context.IsGrounded && m_context.IsMoving)
        {
            SwitchState(m_factory.Moving());
        }
        else if (m_context.IsGrounded)
        {
            SwitchState(m_factory.Grounded());
        }
        //else if (m_context.IsHeld)
        //{
        //    SwitchState(m_factory.Held());
        //}
    }
}
