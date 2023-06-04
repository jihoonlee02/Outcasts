using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : Invokee
{
    [SerializeField]
    private bool open;
    [SerializeField]
    private float timeToOpen = 0.5f;
    [SerializeField] private bool horizontal = false;
    private Vector3 doorPosDown;
    private Vector3 doorPosUp;
    private Vector3 doorPosRight;
    private Vector3 doorPosLeft;
    private Vector3 doorVel = Vector3.zero;
    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tilemapRenderer;
    private float doorDist;
    private float perOpen = 1.0f;
    private bool initOpen;

    // Start is called before the first frame update
    void Start()
    {
        initOpen = open;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (horizontal)
            {
                doorPosRight = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (!open)
                {
                    doorPosRight = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosLeft = doorPosRight + (Vector3.left * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                }
                else
                {
                    doorPosLeft = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosRight = doorPosLeft + (Vector3.right * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                }
                //doorPosUp = !open ? doorPosDown + (Vector3.up * spriteRenderer.sprite.bounds.size.y * transform.localScale.y) : doorPosDown + (Vector3.down * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                doorDist = Vector3.Distance(doorPosRight, doorPosLeft);
            }
            else
            {
                doorPosDown = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (!open)
                {
                    doorPosDown = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosUp = doorPosDown + (Vector3.up * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                }
                else
                {
                    doorPosUp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosDown = doorPosUp + (Vector3.down * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                }
                //doorPosUp = !open ? doorPosDown + (Vector3.up * spriteRenderer.sprite.bounds.size.y * transform.localScale.y) : doorPosDown + (Vector3.down * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                doorDist = Vector3.Distance(doorPosDown, doorPosUp);
            }
            
        } 
        else
        {
            if (horizontal)
            {
                tilemapRenderer = gameObject.GetComponentInChildren<TilemapRenderer>();
                doorPosRight = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (!open)
                {
                    doorPosRight = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosLeft = doorPosRight + (Vector3.left * tilemapRenderer.bounds.size.y * transform.localScale.y);
                }
                else
                {
                    doorPosLeft = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosRight = doorPosLeft + (Vector3.right * tilemapRenderer.bounds.size.y * transform.localScale.y);
                }
                //doorPosUp = !open ? doorPosDown + (Vector3.up * spriteRenderer.sprite.bounds.size.y * transform.localScale.y) : doorPosDown + (Vector3.down * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                doorDist = Vector3.Distance(doorPosRight, doorPosLeft);
            }
            else
            {
                tilemapRenderer = gameObject.GetComponentInChildren<TilemapRenderer>();
                doorPosDown = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (!open)
                {
                    doorPosDown = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosUp = doorPosDown + (Vector3.up * tilemapRenderer.bounds.size.y * transform.localScale.y);
                }
                else
                {
                    doorPosUp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    doorPosDown = doorPosUp + (Vector3.down * tilemapRenderer.bounds.size.y * transform.localScale.y);
                }
                //doorPosUp = !open ? doorPosDown + (Vector3.up * spriteRenderer.sprite.bounds.size.y * transform.localScale.y) : doorPosDown + (Vector3.down * spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
                doorDist = Vector3.Distance(doorPosDown, doorPosUp);
            }
        }
            
        
    }
    // Update is called once per frame
    void Update()
    {
        if (horizontal)
        {
            if (open)
            {
                transform.position = Vector3.SmoothDamp(transform.position, doorPosLeft, ref doorVel, timeToOpen * perOpen);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, doorPosRight, ref doorVel, timeToOpen * perOpen);
            }
        }
        else
        {
            if (open)
            {
                transform.position = Vector3.SmoothDamp(transform.position, doorPosUp, ref doorVel, timeToOpen * perOpen);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, doorPosDown, ref doorVel, timeToOpen * perOpen);
            }
        }   
    }
    protected override void OnActivate()
    {
        open = !open;
    }
    protected override void OnDeactivate()
    {
        open = !open;
    }
}
