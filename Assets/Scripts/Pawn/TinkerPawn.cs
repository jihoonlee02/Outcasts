using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TinkerPawn : Pawn
{
    [Header("Tinker Specific")]
    [SerializeField] private NailGun m_nailGunReference;
    [SerializeField] private bool canShoot;
    private bool m_isShooting;
    private bool m_isJumpingOff;

    #region Technical
    private float initialMass;
    #endregion
    public bool IsShooting 
    {
        get { return m_isShooting; }
        set { m_isShooting = value;}
    }
    public bool IsJumpingOff
    {
        get { return m_isJumpingOff;}
        set { m_isJumpingOff = value;}
    }
    public NailGun NailGunRef => m_nailGunReference;
    protected new void Start()
    {
        base.Start();
        CurrentState = m_states.TinkerDefaultState();
        initialMass = m_rb.mass;

        //Technical
        canShoot = true;
    }
    public override void PrimaryAction(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (!canShoot) return;
        m_isShooting = true;
        m_nailGunReference.UsePrimaryAction(Vector2.zero); // Nail Gun just looks at animator for direction lmao
    }

    public override void SecondaryAction(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        // Hot useless garbage, we need no reloading mate
        // Commented out to optimize performance if players try calling this
        //if (context.performed) m_nailGunReference.UseSecondaryAction();
        //if (context.canceled) m_nailGunReference.UseSecondaryAction();
    }
    public override void Jump()
    {
        if (!canJump) return;
        if (ropeAttached)
        {
            ToggleGrabRope();
            isGrounded = true;
        }
        if (isHeld)
        {
            // Trigger the exit state when player attempts to jump off of ashe's head
            isHeld = false;

            // So that Ashe of Lifting Exit will drop tinker
            m_isJumpingOff = true;
            canJump = true;
            isGrounded = true;
        }
        else if (isGrounded)
        {
            // In response to lifting region swap
            m_rb.mass = initialMass;
            //if (CurrentState != m_states.TinkerHeldState()) 
            m_rb.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastGroundedTime = 0;
            isJumping = true;
            lastJumpTime = jumpBufferTime;
        }
    }
    public void DisableShoot()
    {
        canShoot = false;
    }
    public void EnableShoot()
    {
        canShoot = true;
    }
}
