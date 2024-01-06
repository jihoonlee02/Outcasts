using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private InputActions m_inputActions;
    [SerializeField] private Pawn controlledPawn;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI m_pawnText;
    [SerializeField] private GameObject m_controllerPanel;
    public Pawn ControlledPawn => controlledPawn;
    public PlayerInput PlayerInput => m_playerInput;
    public bool PawnControlDisabled => pawnControlDisabled;
    public bool JumpActive
    {
        set 
        {
            if (value)
            {
                m_playerInput.actions["Jump"].Enable();
            } 
            else
            {
                m_playerInput.actions["Jump"].Disable();
            }        
        }
    }
    public bool MoveActive
    {
        set
        {
            if (value)
            {
                m_playerInput.actions["Movement"].Enable();
            }
            else
            {
                m_playerInput.actions["Movement"].Disable();
            }
        }
    }
    public bool PrimaryActive
    {
        set
        {
            if (value)
            {
                m_playerInput.actions["UseToolPrimary"].Enable();
            }
            else
            {
                m_playerInput.actions["UseToolPrimary"].Disable();
            }
        }
    }
    public bool SecondaryActive
    {
        set
        {
            if (value)
            {
                m_playerInput.actions["UseToolSecondary"].Enable();
            }
            else
            {
                m_playerInput.actions["UseToolSecondary"].Disable();
            }
        }
    }
    #region Technical
    private bool pawnControlDisabled;
    #endregion

    private void Awake()
    {
        //m_inputActions = new InputActions();
        m_playerInput = GetComponent<PlayerInput>();

        //Old -- Adding Methods to inputActions using InputActions()
        //m_inputActions.Player.Jump.performed += JumpAction;
        //m_inputActions.Player.Jump.canceled += JumpAction;
        //m_inputActions.Player.Enable();

        //Adding methods to inputActions in PlayerInput
        m_playerInput.actions["Jump"].performed += JumpAction;
        m_playerInput.actions["Jump"].canceled += JumpAction;
        m_playerInput.actions["UseToolPrimary"].performed += UseToolPrimaryAction;
        m_playerInput.actions["UseToolSecondary"].performed += UseToolSecondaryAction;
        m_playerInput.actions["UseToolSecondary"].canceled += UseToolSecondaryAction;
        m_playerInput.actions["Interact"].performed += InteractAction;
        m_playerInput.actions["Pause"].performed += PauseAction;
        //m_playerInput.actions["Swap"].performed += SwapAction;

        m_playerInput.actions.actionMaps[0].Enable();
        //m_playerInput.actions["Pause"].Disable();
        m_playerInput.actions.actionMaps[1].Enable();
    }
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        // Movement handle
        Vector2 inputVector = m_playerInput.actions["Movement"].ReadValue<Vector2>();
        inputVector.x = (Mathf.Abs(inputVector.x) > 0.6f) ? Mathf.Sign(inputVector.x) : 0;
        controlledPawn?.Move(inputVector);
    }
    #region Actions
    private void JumpAction(InputAction.CallbackContext context)
    {
        //This certainly could be coupled :p
        if (context.performed)
        {
            controlledPawn?.Jump();
        }
        if (context.canceled)
        {
            controlledPawn?.JumpCut();
        }
    }
    private void UseToolPrimaryAction(InputAction.CallbackContext context)
    {
        controlledPawn?.PrimaryAction(context);
    }
    private void UseToolSecondaryAction(InputAction.CallbackContext context)
    {
        controlledPawn?.SecondaryAction(context);
    }
    // Interact Action is Unused
    private void InteractAction(InputAction.CallbackContext context)
    {
        //GameManager.Instance.CurrLevelManager.OnLevelExit();
        //SlideManager.Instance.CurrSlide.RemoveInfo();
        controlledPawn?.ToggleGrabRope();
    }
    private void PauseAction(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePause(this);
    }
    private void SwapAction(InputAction.CallbackContext context)
    {
        if (controlledPawn == GameManager.Instance.Ashe)
        {
            ControlPawn(GameManager.Instance.Tinker);
        }
        else if (controlledPawn == GameManager.Instance.Tinker)
        {
            ControlPawn(GameManager.Instance.Ashe);
        }
    }
    #endregion
    public void ControlPawn(Pawn pawn)
    {
        if (pawn == null)
        {
            controlledPawn = pawn;
            return;
        }
        controlledPawn = pawn;
        m_pawnText.text = pawn.Data.Name;
        StartCoroutine(ShowControlSchemeUsed());
        m_playerInput.actions["Pause"].Enable();
        m_playerInput.actions["Join"].Disable();
    }
    public void EnablePawnControl()
    {
        if (!pawnControlDisabled) return;
        pawnControlDisabled = false;
        m_playerInput.actions["UseToolPrimary"].performed += UseToolPrimaryAction;
        m_playerInput.actions.actionMaps[0].Enable();
        m_playerInput.actions["Join"].Disable();
    }
    public void DisablePawnControl()
    {
        if (pawnControlDisabled) return;
        pawnControlDisabled = true;
        m_playerInput.actions.actionMaps[0].Disable();

        // For Skipping (Ryan) HIGHLY INEFFICENT, But works
        m_playerInput.actions["UseToolPrimary"].performed -= UseToolPrimaryAction;
        m_playerInput.actions["UseToolPrimary"].Enable();
        //m_playerInput.actions["Pause"].Enable(); --> (Ryan) There are no instances where this is beneficial so far
        // Plus this is too jank for my liking ;-;
    }
    private IEnumerator ShowControlSchemeUsed()
    {
        if (controlledPawn.Data.Name == "Tinker")
        {
            var rectComponent = m_controllerPanel.GetComponent<RectTransform>();
            var left = rectComponent.offsetMin.x;
            var right = rectComponent.offsetMax.x;
            rectComponent.offsetMin = new Vector2(Mathf.Abs(right), rectComponent.offsetMin.y);
            rectComponent.offsetMax = new Vector2(Mathf.Abs(left), rectComponent.offsetMax.y);
        }
        m_controllerPanel.SetActive(true);
        switch(m_playerInput.currentControlScheme)
        {
            case "Keyboard":
                m_controllerPanel.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case "Controller":   
            default:
                m_controllerPanel.transform.GetChild(1).gameObject.SetActive(true);
                break;
        }
        yield return new WaitForSeconds(7f);
        m_controllerPanel.SetActive(false);
    }
}
