using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEventController : Invokee
{
    [Header("Pawn Event Controller Specs")]
    [SerializeField] private float moveDuration;
    [SerializeField, Range(0.2f, 1f)] private float moveSpeed = 0.5f;
    private float durationTime;
    private void Start()
    {
        base.Start();
        if (GameManager.Instance == null)
        {
            Debug.LogError("Pawn Event Controller Requires presence of GameManager");
        }
        durationTime = -1f;
    }
    protected override void OnActivate()
    {
        durationTime = Time.time + moveDuration;
    }
    protected override void OnDeactivate()
    {
        Debug.Log("PawnEventController Deactivate");
    }
    // Temp BS before I fully implement Event Controller
    private void Update()
    {
        if (Time.time < durationTime)
        {
            GameManager.Instance.Tinker.Move(Vector2.right * moveSpeed);
            GameManager.Instance.Ashe.Move(Vector2.right * moveSpeed);
        }        
    }
}
