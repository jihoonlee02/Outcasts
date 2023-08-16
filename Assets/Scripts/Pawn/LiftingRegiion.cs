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
        if (!ashe.IsLifting)
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
            Debug.Log("Stopped lifting");
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
                ashe.IsLifting = false;
                cooldownTime = Time.time + delay;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (Time.time > cooldownTime && !ashe.IsLifting)
        {
            Debug.Log("Lifting");
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ashe.IsLifting)
        {
            Debug.Log("Stopped lifting");
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
                ashe.IsLifting = false;
                cooldownTime = Time.time + delay;
            }
        }      
    }

    private void OnDisable()
    {
        
    }

    private void OnEnable()
    {
        
    }
}
