using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private int id;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.GetEventManager.ButtonPressed += OpenDoor;
        EventManager.GetEventManager.ButtonUnpressed += CloseDoor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy() {
        EventManager.GetEventManager.ButtonPressed -= OpenDoor;
        EventManager.GetEventManager.ButtonUnpressed -= CloseDoor;
    }

    private void OpenDoor(int id) {
        if (this.id == id) {
            Debug.Log("Open");
        }
    }

    private void CloseDoor(int id) {
        if (this.id == id) {
            Debug.Log("Close");
        }
    }
}
