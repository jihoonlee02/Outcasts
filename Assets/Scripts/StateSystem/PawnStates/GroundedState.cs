using UnityEngine;

public class GroundedState : State
{
    public GroundedState(Pawn context, PawnStateFactory factory) : base(context, factory) {}

    public override void CheckSwitchState()
    {
        if (m_context.IsJumping)
        {
            SwitchState(m_factory.Jumping());
        }
        else if (m_context.IsMoving && m_context.IsGrounded)
        {
            SwitchState(m_factory.Moving());
        }
        else if (!m_context.IsGrounded)
        {
            SwitchState(m_factory.Falling());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Switched to Grounded");
        m_context.Animator.speed = 1;
        m_context.Animator.Play("Idle");
    }
}
