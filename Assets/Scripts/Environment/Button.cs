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
    private UnityAction m_OnTriggerEnter;
    [SerializeField]
    private UnityAction m_OnTriggerExit;

    private BoxCollider2D collider;
    private int entered;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
        id = id == null ? -1 : id;
        entered = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (entered == 0 && (otherTag == "Ashe" || (!heavy && otherTag == "Tinker"))) {
            entered++;
            EventManager.GetEventManager.ButtonPressed.Invoke(id);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (entered == 1 && (otherTag == "Ashe" || (!heavy && otherTag == "Tinker"))) {
            entered--;
            EventManager.GetEventManager.ButtonUnpressed.Invoke(id);
        }
    }


}
