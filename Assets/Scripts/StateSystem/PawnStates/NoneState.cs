using UnityEngine;

public class NoneState : State
{
    public NoneState(Pawn context, PawnStateFactory factory) : base(context, factory) 
    {

    }
    public override void EnterState()
    {
        m_context.Animator.speed = 1;
        m_context.Animator.Play(m_animationName + m_superState.AnimationName);
    }

    public override void UpdateState()
    {

    }
    public override void CheckSwitchState()
    {

    }
}
