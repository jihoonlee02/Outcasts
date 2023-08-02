using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentBase : MonoBehaviour
{
    private BoxCollider2D baseCollider;
    private AirVent airVent;
    private List<Collider2D> objectsOn;
    private Vector2 xExtent;
    private float yCenter;
    // Start is called before the first frame update
    void Start()
    {
        baseCollider = GetComponent<BoxCollider2D>();
        airVent = GetComponentInChildren<AirVent>();
        objectsOn = new List<Collider2D>();
        xExtent = new Vector2(baseCollider.bounds.center.x - baseCollider.bounds.extents.x, baseCollider.bounds.center.x + baseCollider.bounds.extents.x);
        yCenter = baseCollider.bounds.center.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log("test");
        Rigidbody2D otherRB = other.attachedRigidbody;
        
        if ((otherRB.mass > 2 || other.gameObject.tag == "Ashe")) {
            CheckEnter(other);
            CheckExit(other);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        Rigidbody2D otherRB = other.attachedRigidbody;
        if (otherRB.mass > 2 || other.gameObject.tag == "Ashe") {
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
        if ((xExtent.x <= other.bounds.center.x && xExtent.y >= other.bounds.center.x && yCenter < other.bounds.center.y)) {
            objectsOn.Add(other);
            if (objectsOn.Count == 1) {
                airVent.Deactivate();
            }
        }
    }

    private void CheckExit(Collider2D other) {
        if (!objectsOn.Contains(other)) {
            return;
        }
        if ((xExtent.x > other.bounds.center.x || xExtent.y < other.bounds.center.x || yCenter >= other.bounds.center.y)) {
            objectsOn.Remove(other);
            if (objectsOn.Count == 0) {
                airVent.Activate();
            }
        }
    }
}
