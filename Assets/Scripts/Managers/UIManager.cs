using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager instance;
    public static UIManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    Debug.LogError("UIManager Prefab is required in Game!");
                }
            }
            return instance; 
        }
    }
    #endregion

    [SerializeField] private EventSystem m_eventSystem;
    [SerializeField] private InputSystemUIInputModule m_inputSystemUIInputModule;

    [Header("Pause Components")]
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_pauseButt;
    [SerializeField] private GameObject m_optionButt;

    [Header("Controls Components")]
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject control_button;
    [SerializeField] private GameObject keyboard_button;
    [SerializeField] private GameObject controller_holder;
    [SerializeField] private GameObject keyboard_holder;
    [SerializeField] private GameObject back_button;
    [SerializeField] private GameObject menu;


    [SerializeField] private TextMeshProUGUI m_playerPaused;
    public EventSystem EventSystem => m_eventSystem;

    public void backButtonClick()
    {
        keyboard_button.SetActive(false);
        keyboard_holder.SetActive(false);
        controller_holder.SetActive(false);
        control_button.SetActive(false);
        controller.SetActive(false);
        back_button.SetActive(false);
        menu.SetActive(true);
    }

    public void OptionButtonPressed()
    {
        menu.SetActive(false);
        controller.SetActive(true);
        keyboard_button.SetActive(false);
        keyboard_holder.SetActive(false);
        control_button.SetActive(true);
        controller_holder.SetActive(true);
        back_button.SetActive(true);
    }

    public void controllerClick()
    {
        control_button.SetActive(false);
        controller_holder.SetActive(false);
        keyboard_button.SetActive(true);
        keyboard_holder.SetActive(true);

    }

    public void keyboardClick()
    {
        keyboard_button.SetActive(false);
        keyboard_holder.SetActive(false);
        control_button.SetActive(true);
        controller_holder.SetActive(true);
    }

    private void Start()
    {
        if (m_eventSystem == null) m_eventSystem = GetComponentInChildren<EventSystem>();
        if (m_inputSystemUIInputModule == null) m_inputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
        controller.SetActive(false);
        back_button.SetActive(true);
    }

    public void OpenPauseMenu(PlayerController pc = null)
    {
        m_pauseMenu.SetActive(true);
        m_eventSystem.SetSelectedGameObject(m_pauseButt);
        if (pc != null)
        {
            m_inputSystemUIInputModule.actionsAsset = pc.PlayerInput.actions;
            m_playerPaused.text = pc.ControlledPawn.Data.Name + " Paused"; 
        }
    }

    public void ClosePauseMenu()
    {
        m_pauseMenu.SetActive(false);
    }

}
