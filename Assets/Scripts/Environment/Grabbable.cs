using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private Transform ogParent;
    [SerializeField] private Collider2D collision;
    public void Grab(Pawn pawn)
    {
        ogParent = transform.parent;
        transform.SetParent(pawn.transform, false);
    }

    public void UnGrab(Pawn pawn)
    {
        transform.SetParent(ogParent, false);
    }
}
