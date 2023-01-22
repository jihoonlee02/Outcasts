using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : Tool
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private BoxCollider2D m_collider;

    #region Technical

    private float currTime = 0f;
    private float animationLength = 0.5f; //Most likely will get replaced by the animation's clip time length
    private bool inUse;
    #endregion

    public void Start()
    {
        m_collider.enabled = false;
    }

    public override void UsePrimaryAction()
    {
        if (inUse) { return; }
        //m_animator.SetFloat("MoveX", Mathf.Sign(m_user.Animator.GetFloat("MoveX")));
        m_user.DisableMovement();
        m_user.DisableJump();
        m_user.Animator.Play("Punch");
        m_collider.enabled = true;
        inUse = false;
        m_collider.offset = new Vector2(m_collider.offset.x * Mathf.Sign(((PlayerPawn)m_user).PC.PlayerInputVector.x), m_collider.offset.y);
        currTime = Time.time + animationLength;
    }

    public void FixedUpdate()
    {
        if (Time.time > currTime) { inUse = false; m_collider.enabled = false; m_user.EnableMovement(); m_user.EnableJump(); }
    }


}
