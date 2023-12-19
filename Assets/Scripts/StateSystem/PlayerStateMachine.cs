using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// UNUSED LIKE FUCK CAUSE I ENDED UP JUST NOT NEEDING THIS :PPPPPP
public class PlayerStateMachine : ToolUser
{
    ////Input
    //[SerializeField] private PlayerInput m_playerInput;
    //[SerializeField] private bool isDevMode;
    //public Vector2 PlayerInputVector => m_playerInput.actions["Movement"].ReadValue<Vector2>();
    //private bool m_isJumpPressed;
    //public bool IsJumpPressed => m_isJumpPressed;


    ////// State
    ////private State m_currentState;
    ////private PawnStateFactory m_states;

    ////public State CurrentState
    ////{
    ////    get { return m_currentState; }
    ////    set { m_currentState = value; }
    ////}
        

    //public void Awake()
    //{
    //    //StateMachine
    //    m_states = new PawnStateFactory(this);
    //    m_currentState = m_states.Grounded();
    //    m_currentState.EnterState();

    //    //Adding methods to PlayerInputActions
    //    m_playerInput = GetComponent<PlayerInput>();
    //    m_playerInput.actions["Jump"].performed += OnJump;
    //    m_playerInput.actions["Jump"].canceled += OnJump;
    //    m_playerInput.actions["UseToolPrimary"].performed += OnPrimaryUse;
    //    m_playerInput.actions["UseToolSecondary"].performed += OnSecondaryUse;
    //    m_playerInput.actions["Interact"].performed += OnInteract;
    //    m_playerInput.actions["NextTool"].performed += NextToolAction;
    //    m_playerInput.actions["Prevtool"].performed += PrevToolAction;
    //    m_playerInput.actions.actionMaps[0].Enable();
    //}

    //private new void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    //Old -- Using a new InputActions()
    //    //controlledPawn.Move(m_inputActions.Player.Movement.ReadValue<Vector2>());

    //    //Using PlayerInput itself
    //    Move(m_playerInput.actions["Movement"].ReadValue<Vector2>());
    //    m_currentState.UpdateState();
    //}

    //#region Actions
    //private void OnJump(InputAction.CallbackContext context)
    //{
    //    m_isJumpPressed = context.ReadValueAsButton();
    //}

    //private void OnPrimaryUse(InputAction.CallbackContext context)
    //{
    //    UseToolPrimaryAction();
    //}

    //private void OnSecondaryUse(InputAction.CallbackContext context)
    //{
    //    UseToolSecondaryAction();
    //}

    //private void OnInteract(InputAction.CallbackContext context) {
    //    //GrabRope();
    //}

    //private void NextToolAction(InputAction.CallbackContext context)
    //{
    //    NextTool();
    //}

    //private void PrevToolAction(InputAction.CallbackContext context)
    //{
    //    PrevTool();
    //}
    //#endregion

    //public void HandleJump()
    //{
    //    m_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //    lastGroundedTime = 0;
    //    isJumping = true;
    //    lastJumpTime = jumpBufferTime;
    //}

    //public void HandleJumpCut()
    //{
    //    if (m_rb.velocity.y > 0)
    //    {
    //        m_rb.AddForce(Vector2.down * m_rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
    //    }

    //    lastJumpTime = 0;
    //}

    ////public void ControlPawn(PlayerPawn pawn)
    ////{
    ////    pawn.PC = this;
    ////}
}
