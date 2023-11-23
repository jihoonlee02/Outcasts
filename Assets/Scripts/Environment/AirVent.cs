using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    private AudioSource ventSource;
    public AudioSource VentSource => ventSource;
    public AirVentGroupStruct AirVentGroupStruct {
        get => airVentGroupStruct;
        set {
            airVentGroupStruct = value;
        }
    }
    private static Hashtable airVentTable;
    private Coroutine previousCo;
    private void Awake()
    {
        ventSource = gameObject.GetComponent<AudioSource>();
        ventSource.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        windAreaCol = gameObject.GetComponent<BoxCollider2D>();
        windAreaCol.enabled = true;
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
        if (previousCo != null) {
            StopCoroutine(previousCo);
        }
        previousCo = StartCoroutine(ChangeHeight(power));
    }
    private IEnumerator ChangeHeight(float power) {
        float startScale = airPivot.localScale.y;
        float diffScale = power - startScale;
        // ---Audio-----
        float newVolume = (power / AirVentManager.Instance.TotalAirVentPower[airVentGroup].airVentPower) * 0.8f;
        float initialVolume = ventSource.volume;
        float deltaVolume = (newVolume - initialVolume);
        // ------
        float elapsedTime = 0;
        while (elapsedTime <= 1.0f) {
            float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / 1.0f);
            airPivot.localScale = new Vector3(airPivot.localScale.x, startScale + (diffScale * lerpFactor), airPivot.localScale.z);
            ventSource.volume = initialVolume + (deltaVolume * lerpFactor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject gObject = other.gameObject;
        Rigidbody2D gRBody = other.attachedRigidbody;
        if (gRBody.velocity.y < 0) {
            if (gObject.tag == "Tinker") {
                gRBody.velocity = new Vector2(gRBody.velocity.x, gRBody.velocity.y/5.0f);
            } else if (gObject.tag == "physical" && gObject.GetComponent<Box>() != null && !gObject.GetComponent<Box>().IsHeavy) {
                gRBody.velocity = new Vector2(gRBody.velocity.x, 0);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        GameObject gObject = other.gameObject;
        Rigidbody2D gRBody = other.attachedRigidbody;
        if (gObject.tag == "Tinker" || (gObject.tag == "physical" && gObject.GetComponent<Box>() == null) || !gObject.GetComponent<Box>().IsHeavy) {
            Quaternion windAngQuat = Quaternion.AngleAxis(windAngle, Vector3.forward);
            gRBody.AddForce(windAngQuat * (Vector2.up * windVel));
            if (gObject.tag == "physical" && gObject.GetComponent<Box>() != null && !gObject.GetComponent<Box>().IsHeavy) {
                if (gRBody.velocity.y < 0) {
                    gRBody.velocity = new Vector2(gRBody.velocity.x, 0);
                }
                if (holdObject && gObject.transform.position.x < transform.position.x && gRBody.velocity.x < 0) {
                    gRBody.AddForce(windAngQuat * (Vector2.right * holdForce * Vector3.Distance(gObject.transform.position, gameObject.transform.position)));
                }
                else if (holdObject && gObject.transform.position.x > transform.position.x && gRBody.velocity.x > 0) {
                    gRBody.AddForce(windAngQuat * (Vector2.left * holdForce * Vector3.Distance(gObject.transform.position, gameObject.transform.position)));
                }
            }
        }
    }
}
