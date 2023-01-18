using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private InputActions m_inputActions;
    [SerializeField] private Pawn controlledPawn;
    [SerializeField] private bool isDevMode;

    private void Awake()
    {
        m_inputActions = new InputActions();
        m_playerInput = GetComponent<PlayerInput>();
        //Adding Methods to inputActions using InputActions()
        //m_inputActions.Player.Jump.performed += JumpAction;
        //m_inputActions.Player.Jump.canceled += JumpAction;
        //m_inputActions.Player.Enable();

        //Adding methods to inputActions in PlayerInput
        m_playerInput.actions["Jump"].performed += JumpAction;
        m_playerInput.actions["Jump"].canceled += JumpAction;
        m_playerInput.actions["UseToolPrimary"].performed += UseToolPrimaryAction;
        m_playerInput.actions["UseToolSecondary"].performed += UseToolSecondaryAction;
        m_playerInput.actions["NextTool"].performed += NextToolAction;
        m_playerInput.actions["Prevtool"].performed += PrevToolAction;
        m_playerInput.actions.actionMaps[0].Enable();
    }

    private void Update()
    {
        if (isDevMode && Input.GetKeyDown(KeyCode.K))
        {
            controlledPawn.transform.position = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        //Using a new InputActions()
        //controlledPawn.Move(m_inputActions.Player.Movement.ReadValue<Vector2>());

        //Using PlayerInput itself
        controlledPawn.Move(m_playerInput.actions["Movement"].ReadValue<Vector2>());
    }

    #region Actions
    private void JumpAction(InputAction.CallbackContext context)
    {
        //This certainly could be coupled :p
        if (context.performed)
        {
            controlledPawn.Jump();
        }
        if (context.canceled)
        {
            controlledPawn.JumpCut();
        }
    }

    private void UseToolPrimaryAction(InputAction.CallbackContext context)
    {
        ((ToolUser)controlledPawn).UseToolPrimaryAction();
    }

    private void UseToolSecondaryAction(InputAction.CallbackContext context)
    {
        ((ToolUser)controlledPawn).UseToolSecondaryAction();
    }

    private void NextToolAction(InputAction.CallbackContext context)
    {
        ((ToolUser)controlledPawn).NextTool();
    }

    private void PrevToolAction(InputAction.CallbackContext context)
    {
        ((ToolUser)controlledPawn).PrevTool();
    }
    #endregion

    public void DisableMovement()
    {
        m_playerInput.actions["Movement"].Disable();
    }

    public void EnableMovement()
    {
        m_playerInput.actions["Movement"].Enable();
    }
}
