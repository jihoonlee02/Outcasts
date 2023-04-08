using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    [SerializeField]
    private bool activated = true;
    [SerializeField]
    private int airVentGroup;
    [SerializeField]
    private float windVel;
    [SerializeField]
    private bool holdObject;
    [SerializeField]
    private float holdForce;
    public bool Activated {
        get => activated;
        set {
            activated = value;
        }
    }
    public int AirVentGroup {
        get => airVentGroup;
    }
    private float windAngle;
    private Transform airPivot;
    private BoxCollider2D windAreaCol;
    private SpriteRenderer windAreaSR;
    private AirVentGroupStruct airVentGroupStruct;
    public AirVentGroupStruct AirVentGroupStruct {
        get => airVentGroupStruct;
        set {
            airVentGroupStruct = value;
        }
    }
    private static Hashtable airVentTable;

    // Start is called before the first frame update
    void Start()
    {
        windAreaCol = gameObject.GetComponent<BoxCollider2D>();
        windAreaCol.enabled = activated;
        windAngle = transform.rotation.eulerAngles.z;
        airPivot = gameObject.transform.parent;
        if (windAngle == 0) {
            holdObject = true;
        }
        windAreaSR = gameObject.GetComponent<SpriteRenderer>();
        //windAreaSR.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        windAreaCol.enabled = activated;
    }

    public void Activate() {
        activated = true;
        AirVentManager.ActivateVent(this);
    }

    public void Deactivate() {
        activated = false;
        AirVentManager.DeactivateVent(this);
    }

    public void ChangePower(float power) {
        StopCoroutine("ChangeHeight");
        StartCoroutine(ChangeHeight(power));
    }
    private IEnumerator ChangeHeight(float power) {
        Debug.Log("BRUH BRUH BURH");
        float startScale = airPivot.localScale.y;
        float diffScale = power - startScale;
        float elapsedTime = 0;
        while (elapsedTime <= 1.0f) {
            float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / 1.0f);
            airPivot.localScale = new Vector3(airPivot.localScale.x, startScale + (diffScale * lerpFactor), airPivot.localScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject gObject = other.gameObject;
        Rigidbody2D gRBody = other.attachedRigidbody;
        if (gObject.tag == "physical" || gObject.tag == "Tinker") {
            if (gRBody.velocity.y < 0) {
                gRBody.velocity = new Vector2(gRBody.velocity.x, gRBody.velocity.y/5.0f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        GameObject gObject = other.gameObject;
        Rigidbody2D gRBody = other.attachedRigidbody;
        if (gObject.tag == "physical" || gObject.tag == "Tinker") {
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
