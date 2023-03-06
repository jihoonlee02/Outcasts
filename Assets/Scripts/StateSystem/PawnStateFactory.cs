public class PawnStateFactory
{
    private Pawn m_context;

    public PawnStateFactory(Pawn currentContext)
    {
        m_context = currentContext;
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
    public State Falling()
    {
        return new FallingState(m_context, this);
    }
    public State PawnDefaultState()
    {
        return new PawnDefaultState(m_context, this);
    }
    public State AsheDefaultState() 
    { 
        return new AsheDefaultState(m_context, this);
    }
    public State AsheLifitngState()
    {
        return new AsheLiftingState(m_context, this);
    }
    //public State Shooting()
    //{
    //    return new ShootingState(m_context, this);
    //}
    //public State Punching()
    //{
    //    return new PunchingState(m_context, this);
    //}
    //public State Lifting() 
    //{ 
    //    return new LiftingState(m_context, this);
    //}
}
