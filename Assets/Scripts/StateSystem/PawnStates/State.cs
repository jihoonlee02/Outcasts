using Unity.VisualScripting;
/// <summary>
/// Class that defines the blueprints for State implementation for the Pawn class
/// Uses an accompaniying factory, required in the instance to reduce coupling and quicker access
/// by state bodies.
/// </summary>
public abstract class State
{
    protected bool m_isRootState = false;
    protected Pawn m_context;
    protected PawnStateFactory m_factory;
    protected State m_superState;
    protected State m_subState;

    //Animation Buisness
    protected string m_animationName;
    public string AnimationName => m_animationName;
    public State(Pawn context, PawnStateFactory factory) 
    { 
        m_context = context;
        m_factory = factory;
    }
    public virtual void EnterState() {}
    public abstract void UpdateState();
    public virtual void ExitState() {}
    public virtual void InitializeSubState() {}
    /// <summary>
    /// Required checker that is intended to invoke SwitchState(new State) when needed
    /// This method is usually called within the context pawn.
    /// </summary>
    public abstract void CheckSwitchState();
    /// <summary>
    /// Switches the State class type to a new type
    /// </summary>
    /// <param name="newState"> The state being transitioned to</param>
    protected void SwitchState(State newState) 
    { 
        ExitStates();

        if (m_isRootState) m_context.CurrentState = newState;
        else m_superState?.SetSubState(newState);

        newState.EnterState(); 
    }
    public void UpdateStates()
    {
        UpdateState();
        m_subState?.UpdateStates();
    }

    public void ExitStates()
    {
        ExitState();
        m_subState?.ExitStates();
    }

    protected void SetSuperState(State newSuperState)
    {
        m_superState = newSuperState;
    }

    protected void SetSubState(State newSubState) 
    { 
        m_subState = newSubState;
        newSubState.SetSuperState(this);
    }
}
