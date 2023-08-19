using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AsheLiftingState : State
{
    private float m_followingY;
    private Transform priorParent;
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

        // The Y above ashe's head to always follow
        m_followingY = ((AshePawn)m_context).HeldObject.GetComponent<Collider2D>().bounds.extents.y +m_context.GetComponent<Collider2D>().bounds.extents.y /*+ ((AshePawn)m_context).LifitingRegion.bounds.size.y*/;

        // Set the held object to ashe as its parent
        priorParent = ((AshePawn)m_context).HeldObject.transform.parent;
        //((AshePawn)m_context).HeldObject.transform.SetParent(m_context.transform, true);

        // Set the mass of the Held object to 0 and store the previous and stop all of its previous movement
        prevMass = ((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>().mass;
        ((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>().mass = 0f;
        ((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // Enable the Collision for the held object so that ashe cannot phase these objects through walls
        ((AshePawn)m_context).HeldObjectCollider.offset = new Vector2(((AshePawn)m_context).HeldObjectCollider.offset.x, m_followingY);
        ((BoxCollider2D)((AshePawn)m_context).HeldObjectCollider).size = Vector2.Scale(((AshePawn)m_context).HeldObject.GetComponent<BoxCollider2D>().size, ((AshePawn)m_context).HeldObject.transform.localScale);
        ((AshePawn)m_context).HeldObjectCollider.enabled = true;
    }
    public override void UpdateState()
    {
        // OG WOrking SHITT below here ---->> Old
        // AND HERE specifically the ash context chagning
        ((AshePawn)m_context).HeldObject.transform.position 
            = new Vector3(m_context.transform.position.x, m_followingY + m_context.transform.position.y, ((AshePawn)m_context).HeldObject.transform.position.z);
    }
    public override void ExitState() 
    {
        if (((AshePawn)m_context).HeldObject.tag == "Tinker")
        {
            ((AshePawn)m_context).HeldObject.GetComponent<TinkerPawn>().IsHeld = false;
        }
        ((AshePawn)m_context).HeldObject.GetComponent<Rigidbody2D>().mass = prevMass;
        //((AshePawn)m_context).HeldObject.transform.SetParent(priorParent, true);
        ((AshePawn)m_context).HeldObjectCollider.enabled = false;
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
                .AddForce(new Vector2(Mathf.Sign(m_context.Animator.GetFloat("MoveX")) * 0.0007f, 0.0007f), ForceMode2D.Impulse);
            SwitchState(m_factory.AshePunchingState());
        }
    }
}
