using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PawnEventController : Invokee
{
    [Header("Pawn Event Controller Specs")]
    [SerializeField] private PawnEventData m_pawnEventData;
    private new void Start()
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
    private IEnumerator RunEvents()
    {
        if (m_pawnEventData.Skippable) StartCoroutine(CheckForSkipping());
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
                    if (p.JumpForce <= 0) pawn.Jump();
                    else pawn.Jump(p.JumpForce);

                    // Not very good yet tbh
                    //StartCoroutine(StartJumping(pawn, p.TimeDuration, p.JumpForce));
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
        StopCoroutine("CheckForSkipping");
    }
    // Not Very Functional, but sure :/
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

    private IEnumerator CheckForSkipping()
    {
        float tinkerHeld = 0f;
        float asheHeld = 0f;
        if (GameManager.Instance.AshePC != null && GameManager.Instance.TinkerPC != null)
        {
            yield return new WaitUntil(() =>
            {
                if (GameManager.Instance.TinkerPC.PlayerInput.actions["UseToolPrimary"].inProgress)
                {
                    tinkerHeld += Time.deltaTime;
                }
                else
                {
                    tinkerHeld -= Time.deltaTime;
                }
                if (GameManager.Instance.AshePC.PlayerInput.actions["UseToolPrimary"].inProgress)
                {
                    asheHeld += Time.deltaTime;
                }
                else
                {
                    asheHeld -= Time.deltaTime;
                }

                // Clamp Value
                tinkerHeld = Mathf.Clamp(tinkerHeld, 0f, 0.5f);
                asheHeld = Mathf.Clamp(asheHeld, 0f, 0.5f);

                // Notify UI
                GameManager.Instance.SkipIndicator.TinkerHalf = tinkerHeld;
                GameManager.Instance.SkipIndicator.AsheHalf = asheHeld;

                return tinkerHeld == 0.5f && asheHeld == 0.5f;
            });
        }
        else if (GameManager.Instance.SC != null)
        {
            yield return new WaitUntil(() =>
            {
                if (GameManager.Instance.SC.PlayerInput.actions["PrimaryTinker"].inProgress)
                {
                    tinkerHeld += Time.deltaTime;
                }
                else
                {
                    tinkerHeld -= Time.deltaTime;
                }
                if (GameManager.Instance.SC.PlayerInput.actions["PrimaryAshe"].inProgress)
                {
                    asheHeld += Time.deltaTime;
                }
                else
                {
                    asheHeld -= Time.deltaTime;
                }

                // Clamp Value
                tinkerHeld = Mathf.Clamp(tinkerHeld, 0f, 0.5f);
                asheHeld = Mathf.Clamp(asheHeld, 0f, 0.5f);

                // Notify UI
                GameManager.Instance.SkipIndicator.TinkerHalf = tinkerHeld;
                GameManager.Instance.SkipIndicator.AsheHalf = asheHeld;

                return tinkerHeld == 0.5f && asheHeld == 0.5f;
            });        
        }
        else
        {
            Debug.LogError("No Pawn Controllers Available To Skip Pawn Event");
            yield return null;
        }

        StopAllCoroutines();
        if (m_pawnEventData.SkipEvent.ChangeMusic)
        {
            GameManager.Instance.LevelManager.ChangeMusic(m_pawnEventData.SkipEvent.MusicSelection);
            GameManager.Instance.LevelManager.PlayMusic();
        }
        if (m_pawnEventData.SkipEvent.ChangeCameraPosition)
        {
            Camera.Instance.ShiftTo(m_pawnEventData.SkipEvent.CameraNewLocation);
        }
        if (m_pawnEventData.SkipEvent.HideDialogue)
        {
            DialogueManager.Instance.HideDialogue();
        }
        if (m_pawnEventData.SkipEvent.PausePawnControl)
        {
            GameManager.Instance.LevelManager.PausePawnControl(!m_pawnEventData.SkipEvent.NotCinematic);
        }
        if (m_pawnEventData.SkipEvent.ResumePawnControl)
        {
            GameManager.Instance.LevelManager.ResumePawnControl();
        }
        if (m_pawnEventData.SkipEvent.ChangeTinkerLocation)
        {
            GameManager.Instance.Tinker.transform.position = m_pawnEventData.SkipEvent.TinkerNewLocation;
        }
        if (m_pawnEventData.SkipEvent.ChangeAsheLocation)
        {
            GameManager.Instance.Ashe.transform.position = m_pawnEventData.SkipEvent.AsheNewLocation;
        }
        if (m_pawnEventData.SkipEvent.Invoke && m_pawnEventData.SkipEvent.Id >= 0)
        {
            if (m_pawnEventData.SkipEvent.Activate)
            {
                EventManager.GetEventManager.Activated.Invoke(m_pawnEventData.SkipEvent.Id);
            }
            else
            {
                EventManager.GetEventManager.Deactivated.Invoke(m_pawnEventData.SkipEvent.Id);
            }
        }

    }
}
