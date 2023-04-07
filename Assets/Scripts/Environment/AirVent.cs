using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    [SerializeField]
    private float windVel;
    [SerializeField]
    private bool holdObject;
    [SerializeField]
    private float holdForce;
    private float windAngle;
    private BoxCollider2D windAreaCol;
    private SpriteRenderer windAreaSR;

    // Start is called before the first frame update
    void Start()
    {
        windAreaCol = gameObject.GetComponent<BoxCollider2D>();
        windAngle = transform.rotation.eulerAngles.z;
        Debug.Log(windAngle);
        if (windAngle == 0) {
            holdObject = true;
        }
        windAreaSR = gameObject.GetComponent<SpriteRenderer>();
        //windAreaSR.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject gObject = other.gameObject;
        Rigidbody2D gRBody = other.attachedRigidbody;
        if (gObject.layer == 9 || gObject.tag == "physical") {
            if (gRBody.velocity.y < 0) {
                gRBody.velocity = new Vector2(gRBody.velocity.x, 0);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        GameObject gObject = other.gameObject;
        Rigidbody2D gRBody = other.attachedRigidbody;
        if (gObject.layer == 9 || gObject.tag == "physical") {
            Quaternion windAngQuat = Quaternion.AngleAxis(windAngle, Vector3.forward);
            gRBody.AddForce(windAngQuat * (Vector2.up * windVel));
            if (gObject.tag == "physical" && holdObject) {
                if (gObject.transform.position.x < transform.position.x && gRBody.velocity.x < 0) {
                    gRBody.AddForce(windAngQuat * (Vector2.right * holdForce * Vector3.Distance(gObject.transform.position, gameObject.transform.position)));
                }
                else if (gObject.transform.position.x > transform.position.x && gRBody.velocity.x > 0) {
                    gRBody.AddForce(windAngQuat * (Vector2.left * holdForce * Vector3.Distance(gObject.transform.position, gameObject.transform.position)));
                }
            }
        }
    }
}
