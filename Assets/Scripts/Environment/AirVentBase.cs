using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentBase : MonoBehaviour
{
    private BoxCollider2D baseCollider;
    private AirVent airVent;
    private List<Collider2D> objectsOn;
    private Vector2 Extent;
    private Vector2 Center;
    // Start is called before the first frame update
    void Start()
    {
        baseCollider = GetComponent<BoxCollider2D>();
        airVent = GetComponentInChildren<AirVent>();
        objectsOn = new List<Collider2D>();
        Extent = new Vector2(baseCollider.bounds.center.x - baseCollider.bounds.extents.x, baseCollider.bounds.center.x + baseCollider.bounds.extents.x);
        Center = baseCollider.bounds.center;
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (Collider2D obj in objectsOn)
        //{
        //    if (obj.tag == "physical" && !baseCollider.bounds.Contains(obj.bounds.center))
        //    {
        //        OnTriggerExit2D(obj);
        //        return;
        //    }
        //}
    }

    private void OnTriggerStay2D(Collider2D other) {
        Rigidbody2D otherRB = other.attachedRigidbody;
        
        if (otherRB.mass > 2 || other.gameObject.tag == "Ashe" || other.gameObject.tag == "door") {
            CheckEnter(other);
            CheckExit(other);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        Rigidbody2D otherRB = other.attachedRigidbody;
        if (otherRB.mass > 2 || other.gameObject.tag == "Ashe" || other.gameObject.tag == "door") {
            if (!objectsOn.Contains(other)) {
                return;
            }
            objectsOn.Remove(other);
            if (objectsOn.Count == 0) {
                airVent.Activate();
            }
        }
    }

    private void CheckEnter(Collider2D other) {
        if (objectsOn.Contains(other)) {
            return;
        }
        //if ((Extent.x <= other.bounds.center.x && Extent.y >= other.bounds.center.x && Center.y < other.bounds.center.y)
        //    || (Extent.x <= other.bounds.center.y && Extent.y >= other.bounds.center.y && Center.y < other.bounds.center.x))
        {
            objectsOn.Add(other);
            if (objectsOn.Count == 1)
            {
                Debug.Log("Did you do Deactivate?");
                airVent.Deactivate();
            }
        }
    }

    private void CheckExit(Collider2D other) {
        //if (!objectsOn.Contains(other)) {
        //    return;
        //}
        
        //if ((Extent.x > other.bounds.center.x || Extent.y < other.bounds.center.x || Center.y >= other.bounds.center.y)
        //    && (Extent.x > other.bounds.center.y || Extent.y < other.bounds.center.y || Center.y >= other.bounds.center.x))
        //{
        //    objectsOn.Remove(other);
        //    if (objectsOn.Count == 0)
        //    {
        //        airVent.Activate();
        //    }
        //}
    }
}
