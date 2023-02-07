using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region Technical
    protected float lastGroundedTime;
    protected float lastJumpTime;
    protected bool isJumping;
    protected bool canMove;
    protected bool canJump;
    protected float moveSoundCoolDown;
    protected float moveCurrentSoundTime;
    #endregion

    protected void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
        m_animator = GetComponentInChildren<Animator>();
        m_audioSource = GetComponentInChildren<AudioSource>();
        canMove = true;
        canJump = true;
        moveSoundCoolDown = 0.5f;
        moveCurrentSoundTime = Time.time;
        m_audioSource.clip = m_pawnData.Footstep;
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
        //Old way
        //transform.Translate((inputValue < 0 ? Vector2.left : (inputValue > 0 ? Vector2.right : Vector2.zero)) * Time.deltaTime * movementSpeed);
        if (!canMove) return;
        //Movement Using Forces (direct reference from Dawnosaur)
        inputVector.x = (Mathf.Abs(inputVector.x) > 0.6f) ?Mathf.Sign(inputVector.x) : 0;

        m_animator.SetFloat("MoveY", inputVector.y);
        if (Mathf.Abs(inputVector.x) > 0.1f)
            m_animator.SetFloat("MoveX", inputVector.x);

        //Movement code emulated from Dawnsaur Aug 10, 2021
        float targetSpeed = inputVector.x * movementSpeed;
        float speedDif = targetSpeed - m_rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        if (Mathf.Abs(inputVector.x) > 0)
        {
            m_rb.AddForce(movement * Vector2.right);
        }

        if (Mathf.Abs(m_rb.velocity.x) < 0.01f)
        {
            m_animator.SetBool("IsIdle", true);
            m_animator.speed = 1;
            return;
        }

        m_animator.SetBool("IsIdle", false);
        m_animator.Play("Movement");
        m_animator.speed = Mathf.Abs(m_rb.velocity.x / 3);


        if (Time.time > moveCurrentSoundTime)
        {
            //m_audioSource.clip = m_pawnData.Footstep;
            m_audioSource.Play();
            moveCurrentSoundTime = Time.time + moveSoundCoolDown;
        }

        //Add Force to movement

        //Friction code emulate from Dawnsaur Aug 10, 2021 
        if (lastGroundedTime > 0 && Mathf.Abs(inputVector.x) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(m_rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(m_rb.velocity.x);
            m_rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    protected void Update()
    {
        //timer emulateed from Dawnsau Aug 10, 2021
        lastGroundedTime += Time.deltaTime;
        lastJumpTime += Time.deltaTime;
    }

    protected void FixedUpdate()
    {

        //Make ending of jumps feel more fluid
        if (m_rb.velocity.y < 0)
        {
            m_rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            m_rb.gravityScale = gravityScale;
        }
        if (IsGrounded()) 
        {
            isJumping = false;
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

        if (IsGrounded())
        {
            m_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastGroundedTime = 0;
            isJumping = true;
            lastJumpTime = jumpBufferTime;
            //m_audioSource.clip = m_pawnData.Jump;
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


    public bool IsGrounded()
    {
        return Physics2D.BoxCast(m_collider.bounds.center, m_collider.bounds.size,
            0f, Vector2.down, .1f, LayerMask.GetMask("Platforms"));
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
