using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RelativeMotionGroup : MonoBehaviour
{
    private Dictionary<Transform, Transform> objectsOnGroup;

    private void Start()
    {
        objectsOnGroup = new Dictionary<Transform, Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "feet")
        {
            objectsOnGroup.Add(collision.transform.parent, collision.transform.parent.parent);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Pawn pawn = collision.transform.parent?.GetComponent<Pawn>();
        if (pawn != null && objectsOnGroup.ContainsKey(collision.transform.parent) && pawn.IsGrounded)
        {
            collision.transform.parent.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "feet" && objectsOnGroup.ContainsKey(collision.transform.parent))
        {
            collision.transform.parent.SetParent(objectsOnGroup[collision.transform.parent]);
            objectsOnGroup.Remove(collision.transform.parent);
        }

        if (collision.gameObject.tag == "physical")
        {
            collision.transform.SetParent(objectsOnGroup[collision.transform]);
            objectsOnGroup.Remove(collision.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "physical")
        {
            objectsOnGroup.Add(collision.transform, collision.transform.parent);
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "physical")
        {
            collision.transform.SetParent(objectsOnGroup[collision.transform]);
            objectsOnGroup.Remove(collision.transform);
        }
    }
}
