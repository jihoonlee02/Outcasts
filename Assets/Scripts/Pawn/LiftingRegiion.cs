using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftingRegiion : MonoBehaviour
{
    [SerializeField] private AshePawn parent;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        parent.IsLifting = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        parent.IsLifting = false;
    }
}
