public class GroundedState : State
{
    public GroundedState(Pawn context, PawnStateFactory factory) : base(context, factory) { }

    public override void CheckSwitchState()
    {
        if (m_context.IsJumping)
        {
            SwitchState(m_factory.Jumping());
        }
        else if (m_context.IsMoving)
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
        m_context.Animator.speed = 1;
        m_context.Animator.Play("Idle");
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
