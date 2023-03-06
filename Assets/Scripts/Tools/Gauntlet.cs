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
    private float offsetX;
    private float offsetY;
    #endregion

    public void Start()
    {
        m_punchCollider.enabled = false;
        offsetX = m_punchCollider.offset.x;
        offsetY = m_punchCollider.offset.y;
        //m_grabCollider.enabled = false;
    }

    public override void UsePrimaryAction()
    {
        if (inUse) { return; }
        m_punchCollider.enabled = true;
        inUse = false;
        m_punchCollider.offset = new Vector2(offsetX * Mathf.Sign(m_user.Animator.GetFloat("MoveX")), offsetY);
        currTime = Time.time + animationLength;
        ((AshePawn)m_user).IsPunching = true;
    }

    public override void UseSecondaryAction()
    {
        //m_grabCollider.enabled = true;
    }

    public void FixedUpdate()
    {
        if (Time.time > currTime) { inUse = false; m_punchCollider.enabled = false; ((AshePawn)m_user).IsPunching = false; }
    }


}
