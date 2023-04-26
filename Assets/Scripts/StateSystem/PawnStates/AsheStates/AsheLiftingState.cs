using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AsheLiftingState : State
{
    private float m_followingY;
    public AsheLiftingState(Pawn context, PawnStateFactory factory) : base(context, factory)
    {
        m_isRootState = true;
        m_animationName = "Lifting";
        InitializeSubState();
    }
    private float prevMass = 0f;
    public override void EnterState()
    {
        Debug.Log("Switched to AsheLifting");
        m_followingY = ((AshePawn)m_context).HeldObject.transform.position.y - m_context.transform.position.y - 0.01f;
        //prevMass = ((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>().mass;
        //((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>().mass = 0f;

    }
    public override void UpdateState()
    {
        ((AshePawn)m_context).HeldObject.transform.position 
            = new Vector3(m_context.transform.position.x, m_followingY + m_context.transform.position.y, ((AshePawn)m_context).HeldObject.transform.position.z);
    }
    public override void ExitState() 
    {
        //if (((AshePawn)m_context).HeldObject.tag == "Tinker")
        //{
        //    ((AshePawn)m_context).HeldObject.GetComponent<TinkerPawn>().IsHeld = false;
        //}
        //((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>().mass = prevMass;
        ((AshePawn)m_context).HeldObject = null;
    }
    public override void InitializeSubState()
    {
        if (m_context.IsJumping)
        {
            SetSubState(m_factory.Jumping());
        }
        else if (!m_context.IsGrounded)
        {
            SetSubState(m_factory.Falling());
        }
        else if (m_context.IsMoving)
        {
            SetSubState(m_factory.Moving());
        }
        else
        {
            SetSubState(m_factory.Grounded());
        }

        m_subState.EnterState();
    }

    public override void CheckSwitchState()
    {
        if (!((AshePawn)m_context).IsLifting)
        {
            SwitchState(m_factory.AsheDefaultState());
        }
        else if (((AshePawn)m_context).IsPunching)
        {
            ((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(Mathf.Sign(m_context.Animator.GetFloat("MoveX")) * 0.0009f, 0.0009f), ForceMode2D.Impulse);
            SwitchState(m_factory.AshePunchingState());
        }
    }
}
