using UnityEngine;

/// <summary>
/// I will be honest, this state was a mess in the past, I just swept it all up
/// Your welcome future reader :)
/// </summary>
public class HeldState : State
{
    public HeldState(Pawn context, PawnStateFactory factory) : base(context, factory)
    {
        m_isRootState = true;
        m_animationName = "";
        InitializeSubState();
    }

    public override void CheckSwitchState()
    {
        if (!m_context.IsHeld)
        {
            SwitchState(m_factory.Falling());
        }
        if (m_context.IsJumping) 
        {
            SwitchState(m_factory.Jumping());
        }
    }

    public override void EnterState()
    {
        m_context.CanMove = false;   
    }

    public override void ExitState()
    {
        // Be able to move right before Ashe lets held pawn go
        m_context.CanMove = true;
        GameManager.Instance.Ashe.IsLifting = false;
    }
}
