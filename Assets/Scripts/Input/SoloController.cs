using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoloController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private TinkerPawn m_tinkerPawn;
    [SerializeField] private AshePawn m_ashePawn;

    private void Awake()
    {
        // Jump Actions
        m_playerInput.actions["TinkerJump"].performed += TinkerJumpAction;
        m_playerInput.actions["TinkerJump"].canceled += TinkerJumpAction;
        m_playerInput.actions["AsheJump"].performed += AsheJumpAction;
        m_playerInput.actions["AsheJump"].canceled += AsheJumpAction;

        // Tool Actions
        m_playerInput.actions["TinkerPrimaryTool"].performed += TinkerPrimaryToolAction;
        m_playerInput.actions["TinkerSecondaryTool"].performed += TinkerSecondaryToolAction;
        m_playerInput.actions["TinkerSecondaryTool"].canceled += TinkerSecondaryToolAction;
        m_playerInput.actions["AshePrimaryTool"].performed += AshePrimaryToolAction;
        m_playerInput.actions["AsheSecondaryTool"].performed += AsheSecondaryToolAction;
        m_playerInput.actions["AsheSecondaryTool"].canceled += AsheSecondaryToolAction;

        // Interact Actions
        m_playerInput.actions["TinkerInteract"].performed += TinkerInteractAction;
        m_playerInput.actions["AsheInteract"].performed += AsheInteractAction;

        // Combined Actions
        m_playerInput.actions["Pause"].performed += PauseAction;

        // Enable These Player and UI Actions
        m_playerInput.actions.actionMaps[0].Enable();
        m_playerInput.actions.actionMaps[1].Enable();
    }

    #region Actions
    private void FixedUpdate()
    {
        //Tinker Movement
        Vector2 inputVector = m_playerInput.actions["MoveTinker"].ReadValue<Vector2>();
        inputVector.x = (Mathf.Abs(inputVector.x) > 0.6f) ? Mathf.Sign(inputVector.x) : 0;
        m_tinkerPawn?.Move(inputVector);

        //Ashe Movement
        inputVector = m_playerInput.actions["MoveAshe"].ReadValue<Vector2>();
        inputVector.x = (Mathf.Abs(inputVector.x) > 0.6f) ? Mathf.Sign(inputVector.x) : 0;
        m_ashePawn?.Move(inputVector);
    }
    // Created seperate methods so that we only go one method down rather than two
    private void TinkerJumpAction(InputAction.CallbackContext context)
    {
        // Ripped from Player Controller
        if (context.performed)
        {
            m_tinkerPawn.Jump();
        }
        if (context.canceled)
        {
            m_tinkerPawn.JumpCut();
        }
    }
    private void AsheJumpAction(InputAction.CallbackContext context)
    {
        // Ripped from Player Controller
        if (context.performed)
        {
            m_ashePawn.Jump();
        }
        if (context.canceled)
        {
            m_ashePawn.JumpCut();
        }
    }
    private void TinkerPrimaryToolAction(InputAction.CallbackContext context)
    {
        m_tinkerPawn.PrimaryAction(context);
    }
    private void TinkerSecondaryToolAction(InputAction.CallbackContext context)
    {
        m_tinkerPawn.SecondaryAction(context);
    }
    private void AshePrimaryToolAction(InputAction.CallbackContext context)
    {
        m_ashePawn.PrimaryAction(context);
    }
    private void AsheSecondaryToolAction(InputAction.CallbackContext context)
    {
        m_ashePawn.SecondaryAction(context);
    }
    private void TinkerInteractAction(InputAction.CallbackContext context)
    {
        // This might be a thing?
        m_tinkerPawn.ToggleGrabRope();
    }
    private void AsheInteractAction(InputAction.CallbackContext context)
    {
        // Maybe a thing O>O
        m_ashePawn.ToggleGrabRope();
    }
    private void PauseAction(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePause();
    }
    #endregion
}
