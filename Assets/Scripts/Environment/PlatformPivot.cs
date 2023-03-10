using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformPivot : MonoBehaviour
{
    [SerializeField] private Transform attachTo;
    private Transform oldParent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            oldParent = collision.transform.parent;
            collision.transform.SetParent(attachTo, true);
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
            collision.transform.SetParent(oldParent, true);
    }
}
