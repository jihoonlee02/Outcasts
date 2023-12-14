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
            // TNA Controls
            if (p.PausePawnControl)
            {
                GameManager.Instance.LevelManager.PausePawnControl(!p.NotCinematic);
            }
            if (p.ResumePawnControl)
            {
                GameManager.Instance.LevelManager.ResumePawnControl();
            }
            
            // TNA Custom Actions
            Pawn pawn = (p.PawnSelection == PawnSelection.Tinker) ? GameManager.Instance.Tinker : GameManager.Instance.Ashe;
            switch (p.EventAction)
            {
                case EventAction.None:
                    break;
                case EventAction.Move:
                    StartCoroutine(StartMoving(pawn, p.TimeDuration, p.MoveSpeed, (p.MoveDirection == Direction.Right ? Vector2.right : Vector2.left)));
                    break;
                case EventAction.Jump:
                    StartCoroutine(StartJumping(pawn, p.TimeDuration, p.JumpForce));
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

            // Event Manager Invoking
            if (p.Invoke && p.Id != this.id)
            {
                if (p.Activate)
                {
                    EventManager.GetEventManager.Activated.Invoke(p.Id);
                } 
                else
                {
                    EventManager.GetEventManager.Deactivated.Invoke(p.Id);
                }
            }

            // Dialogue Related --> Last because of potential yielding (Could change this in the future!)
            if (p.ActiveDialogueAtTime && p.Dialogues.Length > 0)
            {
                var display = DialogueManager.Instance.DisplayDialogue(p.Dialogues);

                if (p.WaitOnDialogue) yield return display;
            }
            else if (!p.ActiveDialogueAtTime)
            {
                DialogueManager.Instance.HideDialogue();
            }

            // Only if we didn't wait for dialogue already
            if (p.Dialogues.Length <= 0 || !p.WaitOnDialogue) yield return new WaitForSeconds(p.Delay); 
        }
    }
    private IEnumerator StartJumping(Pawn pawn, float durationTime, float jumpForce)
    {
        if (durationTime <= 0)
        {
            if (jumpForce <= 0) pawn.Jump();
            else pawn.Jump(jumpForce);
        } 
        else
        {
            float startTime = Time.time;
            while (Time.time - startTime < durationTime)
            {
                if (pawn.IsGrounded)
                {
                    if (jumpForce <= 0) pawn.Jump();
                    else pawn.Jump(jumpForce);
                }
               
                yield return new WaitForFixedUpdate();
            }
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
