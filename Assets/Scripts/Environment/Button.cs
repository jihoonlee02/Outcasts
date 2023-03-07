using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour
{
    [SerializeField]
    private bool heavy = false;
    [SerializeField]
    private int id;
    [SerializeField]
    private float lengthOfTime = 5f;
    private BoxCollider2D collider;
    private Vector3 buttonPos;
    private Vector3 basePos;
    private Vector3 buttonVel;
    private int entered;
    private float timer;
    private bool buttonPressed;

    // Start is called before the first frame update
    void Start()
    {
        basePos = gameObject.transform.parent.position;
        buttonPos = basePos + ((new Vector3(transform.position.x, transform.position.y, transform.position.z)) - (new Vector3(basePos.x, basePos.y, basePos.z)));
        Debug.Log(buttonPos);
        buttonVel = Vector3.zero;
        collider = gameObject.GetComponent<BoxCollider2D>();
        entered = 0;
        buttonPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= lengthOfTime) {
            if (buttonPressed) {
                transform.position = Vector3.LerpUnclamped(transform.position, basePos, timer/lengthOfTime);
            } else {
                transform.position = Vector3.LerpUnclamped(transform.position, buttonPos, timer/lengthOfTime);
            }
            timer += Time.deltaTime;
        }   
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (entered == 0 && (otherTag == "Ashe" || (!heavy && otherTag == "Tinker"))) {
            buttonPressed = true;
            timer = 0f;
            EventManager.GetEventManager.Activated.Invoke(id);
        }
        entered++;
    }


    private void OnTriggerExit2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (entered == 1 && (otherTag == "Ashe" || (!heavy && otherTag == "Tinker"))) { 
            buttonPressed = false;
            timer = 0f;
            EventManager.GetEventManager.Deactivated.Invoke(id);
        }
        entered--;
    }
}
