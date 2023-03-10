using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Invokee
{
    [SerializeField]
    private bool open;
    [SerializeField]
    private float timeToOpen = 0.5f;
    private Vector3 doorPosDown;
    private Vector3 doorPosUp;
    private Vector3 doorVel = Vector3.zero;
    private SpriteRenderer spriteRenderer;
    private float doorDist;
    private float perOpen = 1.0f;
    private bool initOpen;

    // Start is called before the first frame update
    void Start()
    {
        initOpen = open;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        doorPosDown = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (!open) {
            doorPosDown = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            doorPosUp = doorPosDown + (Vector3.up * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
        } else {
            doorPosUp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            doorPosDown = doorPosUp + (Vector3.down * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
        }
        //doorPosUp = !open ? doorPosDown + (Vector3.up * spriteRenderer.sprite.bounds.size.y * transform.localScale.y) : doorPosDown + (Vector3.down * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
        doorDist = Vector3.Distance(doorPosDown, doorPosUp);
    }

    // Update is called once per frame
    void Update()
    {
        if (open) {
            transform.position = Vector3.SmoothDamp(transform.position, doorPosUp, ref doorVel, timeToOpen * perOpen);
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, doorPosDown, ref doorVel, timeToOpen * perOpen);
        }
    }

    //Open Door Logic
    protected override void OnActivate()
    {
        Debug.Log("Open");
        perOpen = !initOpen ? Vector3.Distance(transform.position, doorPosUp) / doorDist : Vector3.Distance(transform.position, doorPosDown);
        open = !open;    
    }

    //Close Door Logic
    protected override void OnDeactivate()
    {
        Debug.Log("Close");
        perOpen = !initOpen ? Vector3.Distance(transform.position, doorPosDown) / doorDist : Vector3.Distance(transform.position, doorPosUp) / doorDist;
        open = !open;
    }
}
