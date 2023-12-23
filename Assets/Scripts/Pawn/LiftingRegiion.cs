using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LiftingRegiion : MonoBehaviour
{
    [SerializeField] private AshePawn ashe;
    private Collider2D m_collider;

    //Technical
    private Transform oldParent;
    private float delay;
    private float cooldownTime;

    private void Start()
    {
        delay = 0.4f; // 0.4f was better
        cooldownTime = Time.time;
        m_collider = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Grabbed>()) return;
        if (collision.gameObject.layer == LayerMask.GetMask("Platforms"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        if (Time.time > cooldownTime && !ashe.IsLifting)
        {
            var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
            if (tinkerPawn != null)
            {
                ashe.IsLifting = true;
                tinkerPawn.IsHeld = true;
                ashe.HeldObject = tinkerPawn.gameObject;
                return;
            }

            // Physical Object defintion
            var physical = collision.gameObject.GetComponent<Grabbable>();
            if (physical != null)
            {
                ashe.IsLifting = true;
                ashe.HeldObject = physical.gameObject;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (ashe.IsLifting)
        {
            if (ashe.HeldObject?.GetComponent<TinkerPawn>() != null)
            {
                var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
                if (tinkerPawn != null) //This is the sus one
                {
                    ashe.IsLifting = false;
                    tinkerPawn.IsHeld = false;
                    cooldownTime = Time.time + delay;
                    return;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Grabbed>()) return;
        if (Time.time > cooldownTime && !ashe.IsLifting)
        {
            var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
            if (tinkerPawn != null)
            {
                ashe.IsLifting = true;
                tinkerPawn.IsHeld = true;
                ashe.HeldObject = tinkerPawn.gameObject;
                return;
            }

            // Physical Object defintion
            var physical = collision.gameObject.GetComponent<Grabbable>();
            if (physical != null)
            {
                ashe.IsLifting = true;
                physical.Grab();
                ashe.HeldObject = physical.gameObject;
                return;
            }
        }     
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (ashe.IsLifting)
    //    {
    //        if (ashe.HeldObject?.GetComponent<TinkerPawn>() != null)
    //        {
    //            var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
    //            if (tinkerPawn != null) //This is the sus one
    //            {
    //                ashe.IsLifting = false;
    //                tinkerPawn.IsHeld = false;
    //                Debug.Log("Tinker Hopped off");
    //                cooldownTime = Time.time + delay;
    //            }
    //        }
    //        else if (ashe.HeldObject?.GetComponent<Grabbable>() != null)
    //        {
    //            var physical = collision.gameObject.GetComponent<Grabbable>();
    //            if (physical != null)
    //            {
    //                ashe.IsLifting = false;
    //                cooldownTime = Time.time + delay;
    //            }
    //        }  
    //    }      
    //}
    private IEnumerator DisableCollisionTemp()
    {
        m_collider.enabled = false;
        yield return new WaitForSeconds(0.2f);
        m_collider.enabled = true;
    }
}
