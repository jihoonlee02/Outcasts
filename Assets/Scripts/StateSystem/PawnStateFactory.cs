using System.Xml;

public class PawnStateFactory
{
    private Pawn m_context;

    //Pawn Internal States
    private MovingState moving_state;
    private JumpingState jumping_state;
    private GroundedState grounded_state;
    private FallingState falling_state;


    //Pawn Super States
    private PawnDefaultState pawn_default_state;
    private AsheDefaultState ashe_default_state;
    private AsheLiftingState ashe_lifting_state;

    public PawnStateFactory(Pawn currentContext)
    {
        m_context = currentContext;
    }

    public State None()
    {
        return new NoneState(m_context, this);
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
    public State Held()
    {
        return new HeldState(m_context, this);
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
    public State AshePunchingState()
    {
        return new AshePunchingState(m_context, this);
    }
    public State TinkerDefaultState()
    {
        return new TinkerDefaultState(m_context, this);
    }
    public State TinkerShootState()
    {
        return new TinkerShootState(m_context, this);
    }
    public State TinkerHeldState()
    {
        return new TinkerHeldState(m_context, this);
    }
    //public State Moving()
    //{
    //    if (moving_state == null) moving_state = new MovingState(m_context, this);
    //    return moving_state;
    //}
    //public State Jumping()
    //{
    //    if (jumping_state == null) jumping_state = new JumpingState(m_context, this);
    //    return jumping_state;
    //}
    //public State Grounded()
    //{
    //    if (grounded_state == null) grounded_state = new GroundedState(m_context, this);
    //    return grounded_state;
    //}
    //public State Falling()
    //{
    //    if (falling_state == null) falling_state = new FallingState(m_context, this);
    //    return falling_state;
    //}
    //public State PawnDefaultState()
    //{
    //    if (pawn_default_state == null) pawn_default_state = new PawnDefaultState(m_context, this);
    //    return pawn_default_state;  
    //}
    //public State AsheDefaultState() 
    //{ 
    //    if (ashe_default_state == null) ashe_default_state = new AsheDefaultState(m_context, this);
    //    return ashe_default_state;
    //}
    //public State AsheLifitngState()
    //{
    //    if (ashe_lifting_state == null) ashe_lifting_state = new AsheLiftingState(m_context, this);
    //    return ashe_lifting_state;
    //}
}
