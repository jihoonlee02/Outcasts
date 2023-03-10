using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (otherTag == "Tinker") {
            EventManager.GetEventManager.TinkerRopeAttach += CheckNode;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (otherTag == "Tinker") {
            EventManager.GetEventManager.TinkerRopeAttach -= CheckNode;
        }
    }

    private void CheckNode(GameObject gObject) {
        Pawn controller = gObject.GetComponent<Pawn>();
        GameObject ropeSegment = controller.RopeSegment;
        if (ropeSegment == null || Vector3.Distance(gObject.transform.position, gameObject.transform.parent.position) < Vector3.Distance(gObject.transform.position, ropeSegment.transform.position)) {
            controller.RopeSegment = gameObject.transform.parent.gameObject;
        }
    }
}
