public class MovingState : State
{
    public MovingState(Pawn context, PawnStateFactory factory) : base(context, factory) { }

    public override void CheckSwitchState()
    {
        if (m_context.IsGrounded)
        {
            SwitchState(m_factory.Grounded());
        }
    }

    public override void EnterState()
    {
        m_context.Animator.Play("Movement");
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
