using UnityEngine;

public class NoneState : State
{
    public NoneState(Pawn context, PawnStateFactory factory) : base(context, factory) 
    {

    }
    public override void EnterState()
    {
        Debug.Log("Enter None State");
        m_context.Animator.speed = 1;
    }

    public override void UpdateState()
    {

    }
    public override void CheckSwitchState()
    {

    }
}
