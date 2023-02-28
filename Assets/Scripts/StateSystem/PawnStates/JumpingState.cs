using UnityEngine;

public class JumpingState : State
{
    public JumpingState(Pawn context, PawnStateFactory factory) : base(context, factory) { }
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        if (m_context.IsGrounded)
        {
            SwitchState(m_factory.Grounded());
        }
    }
}
