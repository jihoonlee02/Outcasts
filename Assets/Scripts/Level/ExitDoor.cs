using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private float m_stayTime = 3f;
    [SerializeField] private string m_targetPawn;
    private float timeTrigger;
    private bool isTargetPawnOnDoor;
    public bool OnDoor;

    private void Update()
    {
        OnDoor = timeTrigger > m_stayTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Pawn>()?.Data.Name == m_targetPawn) timeTrigger += Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        timeTrigger = 0;
    }
}
