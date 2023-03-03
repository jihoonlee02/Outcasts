using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Pawn : MonoBehaviour
{
    [Header("Pawn Component References")]
    [SerializeField] protected Rigidbody2D m_rb;
    [SerializeField] protected Collider2D m_collider;
    [SerializeField] protected Animator m_animator;
    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] protected PawnData m_pawnData;

    public Rigidbody2D RB => m_rb;
    public Animator Animator => m_animator;
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
    protected float lastGroundedTime;
    protected float lastJumpTime;
    protected bool canMove;
    protected bool canJump;
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
        m_animator = GetComponentInChildren<Animator>();
        m_audioSource = GetComponentInChildren<AudioSource>();

        //Technical
        canMove = true;
        canJump = true;

        //Defaulting SoundEffects
        m_audioSource.clip = m_pawnData.Footstep;

        //State Setup
        m_states = new PawnStateFactory(this);
        m_currentState = m_states.Grounded();
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

        //Should be handled by Input
        //inputVector.x = (Mathf.Abs(inputVector.x) > 0.6f) ?Mathf.Sign(inputVector.x) : 0;

        /*
         * Setting the variables of these since the pawn's movement intention
         * should be reflected in the animation.
        */
        m_animator.SetFloat("MoveY", inputVector.y);
        if (Mathf.Abs(inputVector.x) > 0.1f)
            m_animator.SetFloat("MoveX", inputVector.x);

        //Movement code emulated from Dawnsaur Aug 10, 2021
        //Physics Calculation of Pawn Movement
        
        if (canMove && Mathf.Abs(inputVector.x) > 0)
        {
            float targetSpeed = inputVector.x * movementSpeed;
            float speedDif = targetSpeed - m_rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            m_rb.AddForce(movement * Vector2.right);
        }

        //Add Force to movement
        //Friction code emulate from Dawnsaur Aug 10, 2021 
        if (lastGroundedTime > 0 && Mathf.Abs(inputVector.x) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(m_rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(m_rb.velocity.x);
            m_rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        //Better if done in a state machine
        //if (Mathf.Abs(m_rb.velocity.x) < 0.001f)
        //{
        //    m_animator.SetBool("IsIdle", true);
        //    m_animator.speed = 1;

        //    //if (m_audioSource.clip == m_pawnData.Footstep)
        //    //    m_audioSource.clip = null;
        //    return;
        //}

        ////State Machine Please resolve this!!!
        //if (isGrounded) 
        //{
        //    m_animator.SetBool("IsIdle", false);
        //    //m_animator.Play("Movement");
        //    m_animator.speed = Mathf.Abs(m_rb.velocity.x / 3);
        //    //float pitchCalc = 0.8f + Mathf.Abs(m_rb.velocity.x / 4);
        //    //m_audioSource.pitch = pitchCalc > 2f ? 2f : pitchCalc < 0.8f ? 0.8f : pitchCalc;
        //}

        //Sound Handle
        //if (!m_audioSource.isPlaying && IsGrounded())
        //{
        //    m_audioSource.clip = m_pawnData.Footstep;
        //    m_audioSource.loop = true;
        //    m_audioSource.Play();
        //}
    }

    protected void Update()
    {
        //timer emulateed from Dawnsau Aug 10, 2021
        lastGroundedTime += Time.deltaTime;
        lastJumpTime += Time.deltaTime;
        //HandleAudio();
        m_currentState.UpdateState();
        m_currentState.CheckSwitchState();
    }

    protected void FixedUpdate()
    {
        // Calculates if the Pawn is grounded (better than constantly invoking a subroutine call)
        isGrounded = Physics2D.BoxCast(m_collider.bounds.center, m_collider.bounds.size,
            0f, Vector2.down, .1f, LayerMask.GetMask("Platforms"));

        isMoving = Mathf.Abs(m_rb.velocity.x) >= 0.001f;
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
        HandlePhysics();
        
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

        if (isGrounded)
        {
            m_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        if (m_rb.velocity.y > 0 && isJumping)
        {
            m_rb.AddForce(Vector2.down * m_rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }

        lastJumpTime = 0;
    }

    public void HandleAnimation()
    {
        
    }

    public void HandleAudio()
    {
        //Jump Audio
        if (isJumping)
        {
            return;
        }

        //Movement Audio
        if (Mathf.Abs(m_rb.velocity.x) < 0.01f)
        {
            m_audioSource.loop = false;
            return;
        }
        if (!m_audioSource.isPlaying && isGrounded)
        {
            //m_audioSource.clip = m_pawnData.Footstep;
            //m_audioSource.loop = true;
            //m_audioSource.Play();
        }
        if (isGrounded) 
        {
            //float pitchCalc = m_minPitch + Mathf.Abs(m_rb.velocity.x / 3);
            //m_audioSource.pitch = pitchCalc > m_maxPitch ? m_maxPitch : pitchCalc < m_minPitch ? m_minPitch : pitchCalc;
        }     
    }

    //Resolve
    private void HandlePhysics()
    {
        
    }

    public void HandleMoveAnimation()
    {

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
