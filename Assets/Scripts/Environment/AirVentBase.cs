using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentBase : MonoBehaviour
{
    private BoxCollider2D baseCollider;
    private AirVent airVent;
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

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("test");
        Rigidbody2D otherRB = other.rigidbody;
        if (otherRB.mass > 2) {
            Debug.Log("lol lmao");
            airVent.Deactivate();
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        Rigidbody2D otherRB = other.rigidbody;
        if (otherRB.mass > 2) {
            airVent.Activate();
        }
    }
}
