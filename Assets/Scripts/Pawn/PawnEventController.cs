using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PawnEventController : Invokee
{
    [Header("Pawn Event Controller Specs")]
    [SerializeField] private PawnEventData m_pawnEventData;
    private void Start()
    {
        base.Start();
        if (GameManager.Instance == null)
        {
            Debug.LogError("Pawn Event Controller Requires presence of GameManager");
        }
    }
    protected override void OnActivate()
    {
        StartCoroutine(RunEvents());
    }
    protected override void OnDeactivate()
    {
       StopAllCoroutines();
    }
    // TODO: Include Dialogue Manager here
    private IEnumerator RunEvents()
    {
        foreach (PawnEvent p in m_pawnEventData.PawnEvents)
        {
            // Could only be either ashe or tinke to be controlled for now
            Pawn pawn = (p.PawnSelection == PawnSelection.Tinker) ? GameManager.Instance.Tinker : GameManager.Instance.Ashe;
            switch (p.EventAction)
            {
                case EventAction.None:
                    break;
                case EventAction.Move:
                    float durationTime = p.TimeDuration + Time.time;
                    while (Time.time < durationTime)
                    {
                        pawn.Move(p.MoveSpeed * (p.MoveDirection == Direction.Right ? Vector2.right : Vector2.left));
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                    break;
                case EventAction.Jump:
                    if (p.JumpForce <= 0) pawn.Jump();
                    else pawn.Jump(p.JumpForce);
                    break;
                case EventAction.Punch:
                    pawn.GetComponent<AshePawn>()?.PrimaryAction();
                    break;
                case EventAction.Grab:
                    pawn.GetComponent<AshePawn>()?.SecondaryAction();
                    break;
                case EventAction.Shoot:
                    pawn.GetComponent<TinkerPawn>()?.PrimaryAction();
                    break;
                default: break;
            }
            yield return new WaitForSeconds(p.Delay);

        }
    }
}
