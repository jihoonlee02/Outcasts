using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Pawn : MonoBehaviour
{
    [Header("Pawn Component References")]
    [SerializeField] protected Rigidbody2D m_rb;
    [SerializeField] protected Collider2D m_collider;
    [SerializeField] protected Animator m_animator;
    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] protected PawnData m_pawnData;
    [SerializeField] protected HingeJoint2D m_hingeJoint;
    [SerializeField] protected FixedJoint2D m_fixedJoint;
    [SerializeField] protected PlayerController m_pc;

    public PlayerController PC
    {
        get { return m_pc; }
        set { m_pc = value; }
    }

    public Rigidbody2D RB => m_rb;
    public Animator Animator => m_animator;
    public FixedJoint2D FixedJoint => m_fixedJoint;
    public PawnData Data => m_pawnData;
    public AudioSource AudioSource => m_audioSource;

    #region Platforming Modifiers
    [Header("Movement Modifiers")]
    [SerializeField] protected float movementSpeed = 10f;
    [SerializeField] protected float acceleration = 7f;
    [SerializeField] protected float decceleration = 7f;
    [SerializeField] protected float velPower = 0.8f;
    [SerializeField] protected float frictionAmount = 0.25f;

    [Header("Jump Modifiers")]
    [SerializeField] protected float jumpForce = 6f;
    [SerializeField, Range(0f, 1f)] protected float jumpCutMultiplier = 0.1f;
    [SerializeField] protected float jumpCoyoteTime;
    [SerializeField] protected float jumpBufferTime;
    [SerializeField] protected float gravityScale;
    [SerializeField] protected float fallGravityMultiplier;
    #endregion

    [Header("Misc. Modifiers")]
    [SerializeField, Range(-1f, 2f)] protected float m_minPitch = 0.8f;
    [SerializeField, Range(-1f, 2f)] protected float m_maxPitch = 1.5f;

    #region Technical
    protected GameObject ropeSegment;
    protected float lastGroundedTime;
    protected float lastJumpTime;
    protected bool canMove;
    protected bool canJump;
    protected bool ropeAttached;
    public GameObject RopeSegment {
        get => ropeSegment;
        set {
            ropeSegment = value;
        }
    }

    public bool CanMove 
    {
        set { canMove = value; }
    }
    public bool CanJump
    {
        set { canJump = value; }
    }

    #endregion

    #region State Info
    protected PawnStateFactory m_states;

    protected bool isMoving;
    protected bool isJumping;
    protected bool isGrounded;
    public bool IsMoving => isMoving;
    public bool IsJumping => isJumping;
    public bool IsGrounded => isGrounded;

    private State m_currentState;
    public State CurrentState 
    {
        get { return m_currentState; }
        set { m_currentState = value; }
    }
     
    #endregion 

    protected void Start()
    {
        //Unity Components
        m_rb = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
        //m_animator = GetComponentInChildren<Animator>();
        //m_audioSource = GetComponentInChildren<AudioSource>();
        m_hingeJoint = GetComponent<HingeJoint2D>();
        m_fixedJoint = GetComponent<FixedJoint2D>();
        m_hingeJoint.enabled = false;
        m_fixedJoint.enabled = false;

        //Technical
        canMove = true;
        canJump = true;

        //Defaulting SoundEffects
        m_audioSource.clip = m_pawnData.Footstep;

        //State Setup
        m_states = new PawnStateFactory(this);
        m_currentState = m_states.PawnDefaultState();
    }

    /// <summary>
    /// Handles Movement of attach gameobject and deals with updating movement related animations
    /// Movements are only left and right in this world
    /// </summary>
    /// <param name="inputValue">
    /// This is the raw value given by an external input source
    /// </param>
    public virtual void Move(Vector2 inputVector)
    {
        //Movement Using Forces (direct reference from Dawnosaur)

        /*
         * Setting the variables of these since the pawn's movement intention
         * should be reflected in the animation.
        */

        if (Mathf.Abs(inputVector.x) > 0.001f)
        {
            isMoving = true;
        }
        else if (Mathf.Abs(m_rb.velocity.x) <= 0.001f)
        {
            isMoving = false;
        }

        m_animator.SetFloat("MoveY", inputVector.y);
        if (Mathf.Abs(inputVector.x) > 0.1f)
            m_animator.SetFloat("MoveX", inputVector.x);

        //Movement code emulated from Dawnsaur Aug 10, 2021
        //Physics Calculation of Pawn Movement
        //Relative force to adjust local position specifically when inside platforms
        
        if (canMove && Mathf.Abs(inputVector.x) > 0)
        {
            float targetSpeed = inputVector.x * movementSpeed;
            float speedDif = targetSpeed - m_rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            m_rb.AddRelativeForce(movement * Vector2.right);
        }

        //Add Force to movement
        //Friction code emulate from Dawnsaur Aug 10, 2021 
        if (lastGroundedTime > 0 && Mathf.Abs(inputVector.x) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(m_rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(m_rb.velocity.x);
            m_rb.AddRelativeForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    protected void Update()
    {
        //timer emulateed from Dawnsau Aug 10, 2021
        lastGroundedTime += Time.deltaTime;
        lastJumpTime += Time.deltaTime;
        //HandleAudio();
        m_currentState.UpdateStates();
    }

    protected void FixedUpdate()
    {
        // Calculates if the Pawn is grounded (better than constantly invoking a subroutine call)
        isGrounded = Physics2D.BoxCast(m_collider.bounds.center, m_collider.bounds.size,
            0f, Vector2.down, .1f, LayerMask.GetMask("Platforms"));
        isJumping = isJumping ? m_rb.velocity.y >= 0.01f : false;

        //Make ending of jumps feel more fluid
        if (m_rb.velocity.y < 0)
        {
            m_rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            m_rb.gravityScale = gravityScale;
        }
        m_animator.SetBool("IsGrounded", isGrounded);
        if (isGrounded)
        {
            lastGroundedTime = jumpCoyoteTime;
        }
        
        //Dev Reasons
        Debug.DrawRay(m_collider.bounds.center + new Vector3(m_collider.bounds.extents.x, 0), Vector2.down * (m_collider.bounds.extents.y + .1f), Color.green);
        Debug.DrawRay(m_collider.bounds.center - new Vector3(m_collider.bounds.extents.x, 0), Vector2.down * (m_collider.bounds.extents.y + .1f), Color.green);
        Debug.DrawRay(m_collider.bounds.center - new Vector3(m_collider.bounds.extents.x, m_collider.bounds.extents.y), Vector2.right * (m_collider.bounds.extents.x), Color.green);
    }

    public void Jump()
    {
        if (!canJump) return;

        //This allowed double jump o.O
        //if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
        //{
        //    m_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //    lastGroundedTime = 0;
        //    isJumping = true;
        //    lastJumpTime = jumpBufferTime;
        //}
        if (ropeAttached) {
            ToggleGrabRope();
            isGrounded = true;
        }
        if (isGrounded)
        {
            m_rb.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastGroundedTime = 0;
            isJumping = true;
            lastJumpTime = jumpBufferTime;
            //m_audioSource.pitch = 1.05f;
            //m_audioSource.clip = m_pawnData.Jump;
            //m_audioSource.loop = false;
            //m_animator.Play("Jump");
            //m_audioSource.Play();
        }

    }

    public void JumpCut()
    {
        if (m_rb.velocity.y > 0 && !isJumping)
        {
            m_rb.AddRelativeForce(Vector2.down * m_rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }

        lastJumpTime = 0;
    }

    public void ToggleGrabRope() {
        if (!ropeAttached) {
            if (EventManager.GetEventManager.TinkerRopeAttach != null) {
                EventManager.GetEventManager.TinkerRopeAttach.Invoke(this.gameObject);
                //transform.position = RopeSegment.transform.position;
                //RopeSegment.transform
                m_hingeJoint.enabled = true;
                m_hingeJoint.connectedBody = RopeSegment.GetComponent<Rigidbody2D>();
                ropeAttached = true;
            }
        } else {
            m_hingeJoint.enabled = false;
            ropeAttached = false;
        }
    }

     public virtual void PrimaryAction(InputAction.CallbackContext context)
    {
        Debug.LogError("Error: " + m_pawnData.Name + " Pawn does not define Primary Action");
    }

    public virtual void SecondaryAction(InputAction.CallbackContext context)
    {
        Debug.LogError("Error: " + m_pawnData.Name + " Pawn does not define Secondary Action");
    }

    #region Togglers

    //Movement
    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void ToggleMovement()
    {
        canMove = !canMove;
    }

    public void DisableJump()
    {
        canJump = false;
    }

    public void EnableJump()
    {
        canJump = true;
    }

    public void ToggleJump()
    {
        canJump = !canJump;
    }

    #endregion
}
