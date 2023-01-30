public class PlayerStateFactory
{
    private PlayerStateMachine m_context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        m_context = currentContext;
    }
    public State Idle()
    {
        return new IdleState(m_context, this);
    }
    public State Moving()
    {
        return new MovingState(m_context, this);
    }
    public State Jumping()
    {
        return new JumpingState(m_context, this);
    }
    public State Grounded()
    {
        return new GroundedState(m_context, this);
    }
    public State Shooting()
    {
        return new ShootingState(m_context, this);
    }
    public State Punching()
    {
        return new PunchingState(m_context, this);
    }
    public State Lifting() 
    { 
        return new LiftingState(m_context, this);
    }
}
