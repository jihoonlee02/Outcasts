using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftingRegiion : MonoBehaviour
{
    [SerializeField] private AshePawn ashe;

    //Technical
    private Transform oldParent;
    private float delay;
    private float cooldownTime;

    private void Start()
    {
        delay = 0.4f; // 0.4f was better
        cooldownTime = Time.time;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
        if (tinkerPawn != null)
        {
            ashe.IsLifting = true;
            tinkerPawn.IsHeld = true;
            ashe.HeldObject = tinkerPawn.gameObject;
            tinkerPawn.RB.velocity = Vector2.zero;
            cooldownTime = Time.time + delay;
            //oldParent = tinkerPawn.transform.parent;
            //tinkerPawn.transform.SetParent(transform, true);
            //tinkerPawn.FixedJoint.connectedBody = ashe.RB;
            //tinkerPawn.transform.position = new Vector3(ashe.transform.position.x, tinkerPawn.transform.position.y, tinkerPawn.transform.position.z);
            return;
        }

        // Physical Object defintion
        var physical = collision.gameObject.GetComponent<Grabbable>();
        if (physical != null)
        {
            ashe.IsLifting = true;
        }
            
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
        if (tinkerPawn != null)
        {
            ashe.IsLifting = false;
            tinkerPawn.IsHeld = false;
            cooldownTime = Time.time + delay;
            //tinkerPawn.transform.SetParent(oldParent, true);
            //tinkerPawn.GetComponent<FixedJoint2D>().connectedBody = null;
            return;
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
        if (Time.time > cooldownTime && tinkerPawn != null)
        {
            ashe.IsLifting = true;
            tinkerPawn.IsHeld = true;
            ashe.HeldObject = tinkerPawn.gameObject;
            tinkerPawn.RB.velocity = Vector2.zero;
            //cooldownTime = Time.time + delay; //This is the sus one
            //oldParent = tinkerPawn.transform.parent;
            //tinkerPawn.transform.SetParent(transform, true);
            //tinkerPawn.FixedJoint.connectedBody = ashe.RB;
            //tinkerPawn.transform.position = new Vector3(ashe.transform.position.x, tinkerPawn.transform.position.y, tinkerPawn.transform.position.z);
            return;
        }

        // Physical Object defintion
        var physical = collision.gameObject.GetComponent<Grabbable>();
        if (physical != null)
        {
            ashe.IsLifting = true;
            ashe.HeldObject = physical.gameObject;
            physical.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
        if (tinkerPawn != null) //This is the sus one
        {
            ashe.IsLifting = false;
            tinkerPawn.IsHeld = false;
            cooldownTime = Time.time + delay;
            return;
        }

        // Physical Object defintion
        var physical = collision.gameObject.GetComponent<Grabbable>();
        if (physical != null)
        {
            ashe.IsLifting = true;
            physical.transform.SetParent(transform, true);
            ashe.HeldObject = physical.gameObject;
            physical.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnDisable()
    {
        
    }

    private void OnEnable()
    {
        
    }
}
