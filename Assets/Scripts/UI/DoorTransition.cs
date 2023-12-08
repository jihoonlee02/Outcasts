using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private Animator m_animator;
    private bool doorClosed;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        doorClosed = true;
    }

    public void CloseDoors()
    {
        if (!gameObject.activeSelf || doorClosed) return;
        m_animator.Play("CloseSingleDoor");
        doorClosed = true;
        //StartCoroutine(rumble());
    }

    public void OpenDoors()
    {
        if (!gameObject.activeSelf || !doorClosed) return;
        m_animator.Play("OpenSingleDoor");
        doorClosed = false;
    }

    private IEnumerator rumble()
    {
        yield return new WaitForSeconds(0.4f);
        Camera.Instance.CameraShaker.StartShakingFor(1f);
    }
}
