using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Invoker
{
    [SerializeField] private bool pressOnce = false;
    [SerializeField]
    private bool heavy = false;
    [SerializeField]
    private float lengthOfTime = 0.5f;
    private Vector3 buttonPos;
    private Vector3 basePos;
    private Vector3 buttonVel;
    private int entered;
    private float timer;
    private bool buttonPressed;
    [SerializeField] private Transform m_pushableButton;

    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.position;
        buttonPos = basePos + m_pushableButton.position - basePos;
        buttonVel = Vector3.zero;
        entered = 0;
        buttonPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= lengthOfTime) {
            if (buttonPressed) {
                m_pushableButton.position = Vector3.LerpUnclamped(m_pushableButton.position, basePos, timer/lengthOfTime);
            } else {
                m_pushableButton.position = Vector3.LerpUnclamped(m_pushableButton.position, buttonPos, timer/lengthOfTime);
            }
            timer += Time.deltaTime;
        }   
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (entered == 0 && (otherTag == "Ashe" || (!heavy && otherTag == "Tinker") || (otherTag == "physical"))) {
            buttonPressed = true;
            timer = 0f;
            Activate();
        }
        entered++;
    }


    private void OnTriggerExit2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (!pressOnce && entered == 1 && (otherTag == "Ashe" || (!heavy && otherTag == "Tinker") || (otherTag == "physical"))) { 
            buttonPressed = false;
            timer = 0f;
            Deactivate();
        }
        entered--;
    }
}
