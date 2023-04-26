using UnityEngine;

public class TinkerHeldState : State
{
    private Vector2 m_worldPosition;
    private float m_followingY;
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
            //m_context.RB.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
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
        m_context.RB.mass = 0f;
        //Distance between tinker's center position and Ashe's center position
        m_followingY = m_context.transform.position.y - GameManager.Instance.Ashe.transform.position.y;
    }

    public override void UpdateState()
    {
        //Couppled Line Right, however this can only really happen if Ashe exists -> This is the hope
        //m_context.transform.position = new Vector3(GameManager.Instance.Ashe.transform.position.x, m_context.transform.position.y, m_context.transform.position.z);
        //m_context.transform.position = new Vector3(GameManager.Instance.Ashe.transform.position.x, m_followingY + GameManager.Instance.Ashe.transform.position.y, m_context.transform.position.z);
        //m_worldPosition = m_context.transform.position.x != 0 ? m_context.transform.position : m_worldPosition;
    }

    public override void ExitState()
    {
        m_context.CanMove = true;
        //m_context.transform.position = m_worldPosition;
        //m_context.FixedJoint.enabled = false;
        m_context.RB.mass = 1f;
    }
}
