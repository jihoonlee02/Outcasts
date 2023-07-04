using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TinkerPawn : Pawn
{
    [SerializeField] private NailGun m_nailGunReference;
    private bool m_isShooting;
    private bool m_isHeld;

    #region Technical
    private float initialMass;
    #endregion
    public bool IsShooting => m_isShooting;
    public bool IsHeld
    {
        get { return m_isHeld; }
        set { m_isHeld = value; }
    }
    protected void Start()
    {
        base.Start();
        CurrentState = m_states.TinkerDefaultState();
        initialMass = m_rb.mass;
    }
    public override void PrimaryAction(InputAction.CallbackContext context)
    {
        var direction = context.action.actionMap["Movement"].ReadValue<Vector2>();
        m_nailGunReference.UsePrimaryAction(direction);
    }

    public override void SecondaryAction(InputAction.CallbackContext context)
    {
        if (context.performed) m_nailGunReference.UseSecondaryAction();
        if (context.canceled) m_nailGunReference.UseSecondaryAction();
    }
    public override void Jump()
    {
        if (!canJump) return;
        if (ropeAttached)
        {
            ToggleGrabRope();
            isGrounded = true;
        }
        if (isGrounded)
        {
            m_rb.mass = initialMass;
            //if (CurrentState != m_states.TinkerHeldState()) 
            m_rb.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastGroundedTime = 0;
            isJumping = true;
            lastJumpTime = jumpBufferTime;
        }
    }
}
