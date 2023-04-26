using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : Tool
{
    [SerializeField] private float m_forcePower;
    [SerializeField] private Animator m_animator;
    [SerializeField] private BoxCollider2D m_punchCollider;
    [SerializeField] private BoxCollider2D m_grabCollider;

    public float ForcePower => Mathf.Sign(m_user.Animator.GetFloat("MoveX")) * m_forcePower;

    #region Technical
    private float currTime = 0f;
    private float animationLength = 0.5f; //Most likely will get replaced by the animation's clip time length
    private bool inUse;
    private float offsetXpunch;
    private float offsetYpunch;
    private float offsetXGrab;
    private float offsetYGrab;
    private Grabbable item;
    #endregion

    public void Start()
    {
        m_punchCollider.enabled = false;
        offsetXpunch = m_punchCollider.offset.x;
        offsetYpunch = m_punchCollider.offset.y;
        offsetXGrab = m_grabCollider.offset.x;
        offsetYGrab = m_grabCollider.offset.y;
        m_grabCollider.enabled = false;
    }

    public override void UsePrimaryAction()
    {
        if (inUse) { return; }
        Debug.Log("Punch");
        //m_punchCollider.enabled = true;
        inUse = true;
        currTime = Time.time + animationLength;
        ((AshePawn)m_user).IsPunching = true;
    }

    public override void UseSecondaryAction()
    {
        if (item != null) 
        {
            item.UnGrab(m_user);
            item = null;
            return;
        }
        if (inUse) { return; }
        m_grabCollider.enabled = true;
        inUse = true;
        m_grabCollider.offset = new Vector2(offsetXGrab * Mathf.Sign(m_user.Animator.GetFloat("MoveX")), offsetYGrab);
        currTime = Time.time + animationLength;
        ((AshePawn)m_user).IsPunching = true;
    }

    public void FixedUpdate()
    {
        if (Time.time > currTime) { inUse = false; m_punchCollider.enabled = false; ((AshePawn)m_user).IsPunching = false;
            m_grabCollider.enabled = false;
        }
    }
    //For the grab collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Grabbable grabbable = collision.gameObject.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            ((AshePawn)m_user).IsLifting = true;
            grabbable.transform.SetParent(transform, true);
            grabbable.transform.position = new Vector3(grabbable.transform.position.x, ((AshePawn)m_user).transform.position.y + 5f);
            ((AshePawn)m_user).HeldObject = grabbable.gameObject;
            grabbable.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        
    }


}
