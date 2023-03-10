using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private Animator m_animator;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void CloseDoors()
    {
        m_animator.Play("CloseDoor");
        //StartCoroutine(rumble());
    }

    public void OpenDoors()
    {
        m_animator.Play("OpenDoor");
    }

    private IEnumerator rumble()
    {
        yield return new WaitForSeconds(0.4f);
        Camera.Instance.CameraShaker.StartShakingFor(1f);
    }
}
