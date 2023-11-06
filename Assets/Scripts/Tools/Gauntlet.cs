using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gauntlet : Tool
{
    [SerializeField] private float m_forcePower;
    [SerializeField] private Animator m_animator;
    [SerializeField] private BoxCollider2D m_punchCollider;
    [SerializeField] private BoxCollider2D m_grabCollider;
    [SerializeField] private float m_offset = 0.001f;

    public float ForcePower => Mathf.Sign(m_user.Animator.GetFloat("MoveX")) * m_forcePower;
    private Collider2D userCollider;

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
        userCollider = m_user.GetComponent<Collider2D>();
    }

    public override void UsePrimaryAction()
    {
        if (inUse) { return; }
        //m_punchCollider.enabled = true;
        inUse = true;
        currTime = Time.time + animationLength;
        ((AshePawn)m_user).IsPunching = true;
    }

    public override void UseSecondaryAction()
    {
        if (((AshePawn)m_user).HeldObject != null) 
        {
            UsePrimaryAction();  
            return;
        }
        if (inUse) { return; }

        // Raycast that looks in front of Ashe Gauntlets based on orientation
        RaycastHit2D[] hit2Ds = Physics2D.BoxCastAll(userCollider.bounds.center, userCollider.bounds.extents,
            0f, Vector2.right * Mathf.Sign(m_user.Animator.GetFloat("MoveX")), 0.2f);
        foreach (RaycastHit2D hit2D in hit2Ds)
        {
            if (hit2D.collider.GetComponent<TinkerPawn>())
            {
                currTime = Time.time + animationLength;
                inUse = true;
                ((AshePawn)m_user).HeldObject = hit2D.collider.gameObject;
                ((AshePawn)m_user).HeldObject.GetComponent<TinkerPawn>().IsHeld = true;
                StartCoroutine(MoveToAshePaws());
                //((AshePawn)m_user).IsLifting = true;
                return;
            }

            if (hit2D.collider.GetComponent<Grabbable>())
            {
                currTime = Time.time + animationLength;
                inUse = true;
                ((AshePawn)m_user).HeldObject = hit2D.collider.gameObject;
                ((AshePawn)m_user).HeldObject.GetComponent<Grabbable>().Grab();
                StartCoroutine(MoveToAshePaws());
                //((AshePawn)m_user).IsLifting = true;
                return;
            }
        }

        //m_grabCollider.enabled = true;
        //m_grabCollider.offset = new Vector2(offsetXGrab * Mathf.Sign(m_user.Animator.GetFloat("MoveX")), offsetYGrab);

        //((AshePawn)m_user).IsPunching = true;  
    }
    private IEnumerator MoveToAshePaws()
    {
        var yDist = ((AshePawn)m_user).HeldObject.GetComponent<Collider2D>().bounds.extents.y + m_user.GetComponent<Collider2D>().bounds.extents.y;
        Vector3 goal = new Vector3(m_user.transform.position.x, m_user.transform.position.y + yDist, m_user.transform.position.z);
        ((AshePawn)m_user).HeldObject.GetComponent<Collider2D>().isTrigger = true;
        ((AshePawn)m_user).DisableLiftingRegion();
        while (((AshePawn)m_user).HeldObject.transform.position.x > goal.x + m_offset || ((AshePawn)m_user).HeldObject.transform.position.x < goal.x - m_offset
            || ((AshePawn)m_user).HeldObject.transform.position.y > goal.y + m_offset || ((AshePawn)m_user).HeldObject.transform.position.y < goal.y - m_offset)
        //while (((AshePawn)m_user).HeldObject.transform.position != goal)
        {
            goal = new Vector3(m_user.transform.position.x, m_user.transform.position.y + yDist, m_user.transform.position.z);
            var postion = ((AshePawn)m_user).HeldObject.transform.position;
            ((AshePawn)m_user).HeldObject.transform.position = Vector2.MoveTowards(postion,goal,Time.deltaTime * 30);
            yield return new WaitForSeconds(Time.deltaTime);  
        }
        ((AshePawn)m_user).IsLifting = true;
        ((AshePawn)m_user).HeldObject.GetComponent<Collider2D>().isTrigger = false;
        ((AshePawn)m_user).EnableLiftingRegion();
    }
    private void MoveOutOfPaws()
    {
        //((AshePawn)m_user).HeldObject.transform.position = ;
    }
    public void FixedUpdate()
    {
        //Debug.DrawRay((userCollider.bounds.center + userCollider.bounds.extents) * Mathf.Sign(m_user.Animator.GetFloat("MoveX")), Vector2.down);
        if (Time.time > currTime) 
        { 
            inUse = false; 
            m_punchCollider.enabled = false; 
            ((AshePawn)m_user).IsPunching = false;
            m_grabCollider.enabled = false;
        }
    }
    // [DEPERACATED] For the grab collider
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Grabbable grabbable = collision.gameObject.GetComponent<Grabbable>();
    //    if (grabbable != null)
    //    {
    //        ((AshePawn)m_user).IsLifting = true;
    //        grabbable.transform.SetParent(transform, true);
    //        grabbable.transform.position = new Vector3(grabbable.transform.position.x, ((AshePawn)m_user).transform.position.y + 5f);
    //        ((AshePawn)m_user).HeldObject = grabbable.gameObject;
    //        grabbable.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //    }
    //}
}
