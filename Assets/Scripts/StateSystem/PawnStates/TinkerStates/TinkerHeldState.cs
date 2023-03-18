

using UnityEngine;

public class TinkerHeldState : State
{
    private Vector2 m_worldPosition;
    public TinkerHeldState(Pawn context, PawnStateFactory factory) : base(context, factory)
    {
        m_isRootState = true;
        m_animationName = "";
        InitializeSubState();
    }

    public override void CheckSwitchState()
    {
        if (!((TinkerPawn)m_context).IsHeld)
        {
            SwitchState(m_factory.TinkerDefaultState());
        }
        if (m_context.IsJumping) 
        {
            SwitchState(m_factory.TinkerDefaultState());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(m_factory.Grounded());
    }

    public override void EnterState()
    {
        m_context.CanMove = false;
        //m_context.FixedJoint.enabled = true;
        //m_context.RB.mass = 0f;
    }

    public override void UpdateState()
    {
        m_context.transform.localPosition = new Vector3(0, m_context.transform.localPosition.y, m_context.transform.localPosition.z);
        m_worldPosition = m_context.transform.position.x != 0 ? m_context.transform.position : m_worldPosition;
    }

    public override void ExitState()
    {
        m_context.CanMove = true;
        m_context.transform.position = m_worldPosition;
        //m_context.FixedJoint.enabled = false;
        //m_context.RB.mass = 1f;
    }
}
