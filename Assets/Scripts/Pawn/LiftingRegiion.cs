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
        if (collision.gameObject.GetComponent<Pawn>() != null)
        {
            ashe.IsLifting = true;
            oldParent = collision.gameObject.transform.parent;
            collision.transform.SetParent(transform, true);
        }
            
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Pawn>() != null)
        {
            ashe.IsLifting = false;
            collision.transform.SetParent(oldParent, true);
        }
            
    }
}
