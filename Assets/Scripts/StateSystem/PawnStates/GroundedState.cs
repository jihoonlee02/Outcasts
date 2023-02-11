public class GroundedState : State
{
    public GroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSubState()
    {
        SetSubState(m_factory.Idle());
        SetSubState(m_factory.Moving());
        SetSubState(m_factory.Lifting());
        SetSubState(m_factory.Punching());
        SetSubState(m_factory.Shooting());
    }

    public override void CheckSwitchState()
    {
        if (m_context.IsJumpPressed) 
        {
            SwitchState(m_factory.Jumping());
        }
    }
}
