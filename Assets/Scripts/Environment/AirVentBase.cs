using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentBase : MonoBehaviour
{
    private BoxCollider2D baseCollider;
    private AirVent airVent;
    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        baseCollider = GetComponent<BoxCollider2D>();
        airVent = GetComponentInChildren<AirVent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("test");
        Rigidbody2D otherRB = other.attachedRigidbody;
        if (otherRB.mass > 2 || other.gameObject.tag == "Ashe") {
            Debug.Log("lol lmao");
            counter++;
            if (counter == 1) {
                airVent.Deactivate();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        Rigidbody2D otherRB = other.attachedRigidbody;
        if (otherRB.mass > 2 || other.gameObject.tag == "Ashe") {
            counter--;
            if (counter == 0) {
                airVent.Activate();
            }
        }
    }
}
