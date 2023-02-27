public abstract class State
{
    protected Pawn m_context;
    protected PawnStateFactory m_factory;
    //protected State m_currentSuperState;
    //protected State m_currentSubState;
    public State(Pawn context, PawnStateFactory factory) 
    { 
        m_context= context;
        m_factory= factory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    //public abstract void InitializeSubState();
    public abstract void CheckSwitchState();
    protected void SwitchState(State newState) 
    { 
        ExitState();
        newState.EnterState();
        m_context.CurrentState = newState;
    }
    //protected void SetSuperState(State newSuperState) 
    //{
    //    m_currentSuperState = newSuperState;
    //}
    //protected void SetSubState(State newSubState) 
    //{
    //    m_currentSubState = newSubState;
    //    newSubState.SetSuperState(this);
    //}
}
