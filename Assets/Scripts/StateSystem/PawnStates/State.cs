using Unity.VisualScripting;
/// <summary>
/// Class that defines the blueprints for State implementation for the Pawn class
/// Uses an accompaniying factory, required in the instance to reduce coupling and quicker access
/// by state bodies.
/// </summary>
public abstract class State
{
    protected Pawn m_context;
    protected PawnStateFactory m_factory;
    protected State m_superState;
    protected State m_subState;
    public State(Pawn context, PawnStateFactory factory) 
    { 
        m_context = context;
        m_factory = factory;
    }
    public virtual void EnterState() {}
    public virtual void UpdateState() {}
    public virtual void ExitState() {}
    public virtual void InitizeSubState() {}
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
        ExitState();
        newState.EnterState();
        m_context.CurrentState = newState;
    }

    protected void SetSuperState(State newSuperState)
    {
        m_superState = newSuperState;
    }

    protected void SetSubState(State newSubState) 
    { 
        m_subState = newSubState;
        newSubState.SetSubState(this);
    }
}
