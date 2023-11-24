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
            if (p.PausePawnControl)
            {
                GameManager.Instance.LevelManager.PausePawnControl();
            }
            if (p.ResumePawnControl)
            {
                GameManager.Instance.LevelManager.ResumePawnControl();
            }
            // Could only be either ashe or tinke to be controlled for now
            Pawn pawn = (p.PawnSelection == PawnSelection.Tinker) ? GameManager.Instance.Tinker : GameManager.Instance.Ashe;
            switch (p.EventAction)
            {
                case EventAction.None:
                    break;
                case EventAction.Move:
                    StartCoroutine(StartMoving(pawn, p.TimeDuration, p.MoveSpeed, (p.MoveDirection == Direction.Right ? Vector2.right : Vector2.left)));
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
            if (p.ActiveDialogueAtTime && p.Dialogues.Length > 0)
            {
                DialogueManager.Instance.DisplayDialogue(p.Dialogues);
            }
            else if (!p.ActiveDialogueAtTime)
            {
                DialogueManager.Instance.HideDialogue();
            }
            yield return new WaitForSeconds(p.Delay);

        }
    }

    private IEnumerator StartMoving(Pawn pawn, float durationTime, float moveSpeed, Vector2 moveDirection)
    {
        float startTime = Time.time;
        while (Time.time - startTime < durationTime)
        {
            pawn.Move(moveSpeed * moveDirection);
            
            yield return new WaitForFixedUpdate();
        }
    }
}
