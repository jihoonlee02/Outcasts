using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : Tool
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private BoxCollider2D m_punchCollider;
    [SerializeField] private BoxCollider2D m_grabCollider;

    #region Technical
    private float currTime = 0f;
    private float animationLength = 0.5f; //Most likely will get replaced by the animation's clip time length
    private bool inUse;
    #endregion

    public void Start()
    {
        m_punchCollider.enabled = false;
        m_grabCollider.enabled = false;
    }

    public override void UsePrimaryAction()
    {
        if (inUse) { return; }
        //m_animator.SetFloat("MoveX", Mathf.Sign(m_user.Animator.GetFloat("MoveX")));
        m_user.DisableMovement();
        m_user.DisableJump();
        m_user.Animator.Play("Punch");
        m_punchCollider.enabled = true;
        inUse = false;
        m_punchCollider.offset = new Vector2(m_punchCollider.offset.x * Mathf.Sign(((PlayerPawn)m_user).PC.PlayerInputVector.x), m_punchCollider.offset.y);
        currTime = Time.time + animationLength;
    }

    public override void UseSecondaryAction()
    {
        m_grabCollider.enabled = true;
    }

    public void FixedUpdate()
    {
        if (Time.time > currTime) { inUse = false; m_punchCollider.enabled = false; m_user.EnableMovement(); m_user.EnableJump(); }
    }


}
