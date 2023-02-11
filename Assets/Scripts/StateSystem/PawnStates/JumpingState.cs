using UnityEngine;

public class JumpingState : State
{
    public JumpingState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }
    public override void EnterState()
    {
        m_context.HandleJump();
    }

    public override void ExitState()
    {
        m_context.HandleJumpCut();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void CheckSwitchState()
    {
        if (m_context.IsGrounded())
        {
            SwitchState(m_factory.Grounded());
        }
    }
}
