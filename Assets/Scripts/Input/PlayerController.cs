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

    public Pawn ControlledPawn => controlledPawn;

    public Vector2 PlayerInputVector => m_playerInput.actions["Movement"].ReadValue<Vector2>();
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

    private void Awake()
    {
        m_inputActions = new InputActions();
        m_playerInput = GetComponent<PlayerInput>();

        if (isDevMode) ControlPawn(GetComponent<PlayerPawn>());

        //Old -- Adding Methods to inputActions using InputActions()
        //m_inputActions.Player.Jump.performed += JumpAction;
        //m_inputActions.Player.Jump.canceled += JumpAction;
        //m_inputActions.Player.Enable();

        //Adding methods to inputActions in PlayerInput
        m_playerInput.actions["Jump"].performed += JumpAction;
        m_playerInput.actions["Jump"].canceled += JumpAction;
        //Depricated Tbh
        m_playerInput.actions["UseToolPrimary"].performed += UseToolPrimaryAction;
        m_playerInput.actions["UseToolSecondary"].performed += UseToolSecondaryAction;
        m_playerInput.actions["UseToolSecondary"].canceled += UseToolSecondaryAction;
        m_playerInput.actions["Interact"].performed += InteractAction;
        m_playerInput.actions["Pause"].performed += PauseAction;
        m_playerInput.actions["Swap"].performed += SwapAction;
        //m_playerInput.actions["NextTool"].performed += NextToolAction;
        //m_playerInput.actions["Prevtool"].performed += PrevToolAction;

        //WHY???
        //m_playerInput.actions["SlideLeft"].performed += InteractAction;
        //m_playerInput.actions["SlideRight"].performed += SlideRightAction;
        m_playerInput.actions.actionMaps[0].Enable();

        //Testing Actions
        m_playerInput.actions["Swap"].Disable();
    }

    private void Start()
    {
        
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
        //Old -- Using a new InputActions()
        //controlledPawn.Move(m_inputActions.Player.Movement.ReadValue<Vector2>());

        //Using PlayerInput itself
        // Hardcoded threshold disgusting
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
            controlledPawn.Jump();
        }
        if (context.canceled)
        {
            controlledPawn.JumpCut();
        }
    }

    private void UseToolPrimaryAction(InputAction.CallbackContext context)
    {
        controlledPawn.PrimaryAction(context);
    }

    private void UseToolSecondaryAction(InputAction.CallbackContext context)
    {
        controlledPawn.SecondaryAction(context);
    }

    //private void NextToolAction(InputAction.CallbackContext context)
    //{
    //    //controlledPawn.NextTool();
    //    //GameManager.Instance.CurrLevelManager.NextLevel();
    //    SlideManager.Instance.NextSlide();
    //}

    //private void PrevToolAction(InputAction.CallbackContext context)
    //{
    //    //controlledPawn.PrevTool();
    //    //GameManager.Instance.CurrLevelManager.PrevLevel();
    //    SlideManager.Instance.PrevSlide();
    //}

    //VERY COUPPLED DO NOT PUSH FOR FINAL GAME
    private void InteractAction(InputAction.CallbackContext context)
    {
        //GameManager.Instance.CurrLevelManager.OnLevelExit();
        //SlideManager.Instance.CurrSlide.RemoveInfo();
        controlledPawn.ToggleGrabRope();
    }

    private void SlideRightAction(InputAction.CallbackContext context)
    {
        SlideManager.Instance.CurrSlide.AddInfo();
    }

    private void PauseAction(InputAction.CallbackContext context)
    {
        GameManager.Instance.TogglePause();
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
        controlledPawn = pawn;
        pawn.PC = this;
    }

    public void OnTesting()
    {
        m_playerInput.actions["Swap"].Enable();
    }
}
