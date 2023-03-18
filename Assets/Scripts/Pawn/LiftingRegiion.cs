using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftingRegiion : MonoBehaviour
{
    [SerializeField] private AshePawn ashe;

    //Technical
    private Transform oldParent;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
        if (tinkerPawn != null)
        {
            ashe.IsLifting = true;
            tinkerPawn.IsHeld = true;
            oldParent = tinkerPawn.transform.parent;
            tinkerPawn.transform.SetParent(transform, true);
            //tinkerPawn.FixedJoint.connectedBody = ashe.RB;
            //tinkerPawn.transform.position = new Vector3(ashe.transform.position.x, tinkerPawn.transform.position.y, tinkerPawn.transform.position.z);
            return;
        }

        // Physical Object defintion
            
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var tinkerPawn = collision.gameObject.GetComponent<TinkerPawn>();
        if (tinkerPawn != null)
        {
            ashe.IsLifting = false;
            tinkerPawn.IsHeld = false;
            tinkerPawn.transform.SetParent(oldParent, true);
            //tinkerPawn.GetComponent<FixedJoint2D>().connectedBody = null;
            return;
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void OnEnable()
    {
        
    }
}
